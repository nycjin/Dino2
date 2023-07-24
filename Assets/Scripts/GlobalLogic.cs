using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalLogic : MonoBehaviour
{
    static GlobalLogic Instance { get; set; }
    ILogic GameLogic { get; set; }
    public static ILogic CurrentLogic => Instance.GameLogic;

    void Awake()
    {
        if ( Instance != null && Instance != this )
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SingleMode()
    {
        StartCoroutine(RoutineByMode("SinglePlay"));
    }
    
    public void MultiMode()
    {
        StartCoroutine(RoutineByMode("MultiPlay"));
    }
    
    IEnumerator RoutineByMode(string sceneName)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene is fully loaded
        while ( !asyncLoad.isDone )
        {
            yield return null;
        }

        // Now you can interact with scripts in the new scene
        if ( sceneName == "SinglePlay" )
        {
            GameLogic = SingleLogic.Instance;
            // GameLogic = new SingleLogic();
            // GameLogic = CreateSingleLogicObject();
        }
        else if ( sceneName == "MultiPlay")
        {
            GameLogic = MultiLogic.Instance;
        }
    }

    static ILogic CreateSingleLogicObject()
    {
        var gm = new GameObject("LogicObject");

        return gm.AddComponent<SingleLogic>();
    }

    
}