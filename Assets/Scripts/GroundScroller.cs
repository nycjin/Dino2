using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    public GameObject another;
    float endZone;
    public float groundWidth;

    void Start()
    {
        groundWidth = GetComponent<SpriteRenderer>().bounds.size.x - 1;
        endZone = -groundWidth;
    }

    void Update()
    {
        if( transform.position.x <= endZone ) {
            transform.position = new Vector3( 
                another.transform.position.x + groundWidth, 
                transform.position.y, 
                transform.position.z );
        } else {
            transform.position += Vector3.left * (GlobalLogic.CurrentLogic.MoveSpeed * Time.deltaTime);
        }
    }
}
