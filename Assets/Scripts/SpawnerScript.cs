using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] List<GameObject> obstacles;
    [SerializeField] float spawnInterval = 20;
    // public float spawnRate = 2; // default
    // public float respawnPoint = 7.8F;

    float _playerMoved;

    void Update()
    {
        var Logic = GlobalLogic.CurrentLogic;
        
        if ( _playerMoved < spawnInterval ) {
            _playerMoved += Logic.MoveSpeed * Time.deltaTime;
        }
        else {
            var i = Random.Range(0, 3);
            
            if ( i == 2 && spawnInterval % 2 == 0 ) { 
	            Instantiate(obstacles[ i ], new Vector3(transform.position.x, transform.position.y + 0.3f, 0),
                    transform.rotation);
	        } else { 
                Instantiate(obstacles[ i ], new Vector3(transform.position.x, transform.position.y, 0),
                    transform.rotation);
	        }

            spawnInterval = Random.Range( 3, 20 );
            _playerMoved = 0;
        }
    }
}
