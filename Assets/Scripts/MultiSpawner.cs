using System.Collections.Generic;
using Interface;
using Mirror;
using UnityEngine;

public class MultiSpawner : NetworkBehaviour, IInjectable
{
    [SerializeField] List<GameObject> spawnablePrefabs;
    [SerializeField] float spawnInterval = 50;

    float _objectMoved;

    readonly InjectHandler _injectHandler = new();
    ILogic CurrentLogic => _injectHandler.CurrentLogic;
    public void SetLogic(ILogic logic) => _injectHandler.SetLogic(logic);


    void Update()
    {
        if ( CurrentLogic == null )
        {
            Debug.Log("Logic is not set on Spawner!!");
            return;
        }

        var gameMoveSpeed = CurrentLogic.MoveSpeed;

        if ( _objectMoved < spawnInterval )
        {
            _objectMoved += gameMoveSpeed * Time.deltaTime;
        }
        else
        {
            // Spawn is only available on server.
            if ( isServer )
            {
                SpawnPrefab();
            }

            spawnInterval = Random.Range(3, 20);
            _objectMoved = 0;
        }
    }

    void SpawnPrefab()
    {
        var i = Random.Range(0, spawnablePrefabs.Count);

        var newObstacle = Instantiate(spawnablePrefabs[ i ], transform.position, Quaternion.identity);

        if ( spawnablePrefabs[ i ].name == "Ptero" && spawnInterval % 2 == 0 )
        {
            newObstacle.transform.position += Vector3.up * 0.3f;
        }

        // DI for new gameObject
        var injectable = newObstacle.GetComponent<IInjectable>();
        if ( injectable != null )
        {
            injectable.SetLogic(CurrentLogic);
            Debug.Log($"DI to {newObstacle.name} !!");
        }

        // Add to Network Server!
        NetworkServer.Spawn(newObstacle);
    }
}