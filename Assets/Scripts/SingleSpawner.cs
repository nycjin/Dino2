using System.Collections.Generic;
using forDino;
using Interface;
using UnityEngine;

public class SpawnerScript : MonoBehaviour, IInjectable
{
    [SerializeField] List<GameObject> obstacles;

    [SerializeField] float spawnInterval = 20;
    // public float spawnRate = 2; // default
    // public float respawnPoint = 7.8F;

    float _playerMoved;

    readonly InjectHandler _injectHandler = new();
    ILogic CurrentLogic => _injectHandler.CurrentLogic;
    public void SetLogic(ILogic logic) => _injectHandler.SetLogic(logic);


#if !NETWORK_TEST
    void Start()
    {
        Debug.Log("NETWORK_TEST IS NOT SET!!");
    }

    void Update()
    {
        var gameMoveSpeed = 0f;
        if ( CurrentLogic != null )
        {
            gameMoveSpeed = CurrentLogic.MoveSpeed;
        }
        else
        {
            Debug.LogError("_logicManager is not set!!");
        }

        if ( _playerMoved < spawnInterval )
        {
            _playerMoved += gameMoveSpeed * Time.deltaTime;
        }
        else
        {
            var i = Random.Range(0, 3);

            var newObstacle = Instantiate(obstacles[ i ], transform.position, transform.rotation);

            if ( i == 2 && spawnInterval % 2 == 0 )
            {
                newObstacle.transform.position += Vector3.up * 0.3f;
            }

            // DI for new gameObject!
            newObstacle.GetComponent<IInjectable>().SetLogic(CurrentLogic);

            spawnInterval = Random.Range(3, 20);
            _playerMoved = 0;
        }
    }
#endif
}