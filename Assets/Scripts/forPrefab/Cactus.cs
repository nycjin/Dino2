using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    [SerializeField] float deadZone = -10;
    void Start()
    {
        
    }

    void Update()
    {
        var moveSpeed = GlobalLogic.CurrentLogic.MoveSpeed;
        transform.position += Vector3.left * (GlobalLogic.CurrentLogic.MoveSpeed * Time.deltaTime);

        if( transform.position.x <= deadZone ) {
            Destroy( gameObject );
        }
    }
}
