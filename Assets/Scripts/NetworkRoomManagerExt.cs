using System.Collections;
using forDino;
using Interface;
using Mirror;
using UnityEngine;

[AddComponentMenu("")]
public class NetworkRoomManagerExt : NetworkRoomManager
{
    [Header("Spawner Setup")] [Tooltip("Reward Prefab for the Spawner")]
    public GameObject rewardPrefab;

    public new static NetworkRoomManagerExt singleton { get; private set; }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        singleton = this;
    }

    /// <summary>
    /// This is called on the server when a networked scene finishes loading.
    /// </summary>
    /// <param name="sceneName">Name of the new scene.</param>
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        // spawn the initial batch of Rewards
        // if (sceneName == GameplayScene)
        // Spawner.InitialSpawn(); // 삭제
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
    /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
    /// into the GamePlayer object as it is about to enter the Online scene.
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="roomPlayer"></param>
    /// <param name="gamePlayer"></param>
    /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer,
        GameObject gamePlayer)
    {
        // index 설정
        var playerScore = gamePlayer.GetComponent<PlayerScore>();
        playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;

        // logicManager 주입
        var multiLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<ILogic>();
        gamePlayer.GetComponent<IInjectable>().SetLogic(multiLogic);

        ( ( IInjectable )playerScore ).SetLogic(multiLogic); // TODO 이게 최선??

        return true;
    }

    public override void OnRoomClientSceneChanged()
    {
        base.OnRoomClientSceneChanged();
        StartCoroutine(AwaitSceneLoad());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    static IEnumerator AwaitSceneLoad()
    {
        var logicObject = GameObject.FindGameObjectWithTag("Logic");
        while ( !logicObject )
        {
            yield return null;
            logicObject = GameObject.FindGameObjectWithTag("Logic");
        }

        GameObject[ ] gamePlayers = { };
        while ( gamePlayers.Length == 0 )
        {
            gamePlayers = GameObject.FindGameObjectsWithTag("Player");
            yield return null;
        }

        var multiLogic = logicObject.GetComponent<ILogic>();
        foreach ( var player in gamePlayers )
        {
            var injectable = player.GetComponent<IInjectable>();
            injectable.SetLogic(multiLogic);
            Debug.Log("client player logic is set!!");
        }
    }

    public override void OnRoomStopClient()
    {
        base.OnRoomStopClient();
    }

    public override void OnRoomStopServer()
    {
        base.OnRoomStopServer();
    }

    /*
        This code below is to demonstrate how to do a Start button that only appears for the Host player
        showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
        all players are ready, but if a player cancels their ready state there's no callback to set it back to false
        Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
        Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
        is set as DontDestroyOnLoad = true.
    */

    bool showStartButton;

    public override void OnRoomServerPlayersReady()
    {
        // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#else
        showStartButton = true;
#endif
    }

    public override void OnGUI()
    {
        if ( !showRoomGUI )
            return;

        if ( NetworkServer.active && Utils.IsSceneActive(GameplayScene) )
        {
            GUILayout.BeginArea(new Rect(Screen.width - 150f, 10f, 140f, 30f));
            if ( GUILayout.Button("대기실로 돌아가기") )
                ServerChangeScene(RoomScene);
            GUILayout.EndArea();
        }

        if ( Utils.IsSceneActive(RoomScene) )
            GUI.Box(new Rect(10f, 180f, 520f, 150f), "대기실");

        if ( allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "게임 시작") )
        {
            // set to false to hide it in the game scene
            showStartButton = false;

            ServerChangeScene(GameplayScene);
        }
    }
}