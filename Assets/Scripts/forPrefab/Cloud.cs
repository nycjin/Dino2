using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    static readonly List<Cloud> Clouds = new();
    const int SIZE_INSTANCE = 3;
    [SerializeField] float deadZone = -10;

    void Start()
    {
        if ( Clouds.Count < SIZE_INSTANCE )
        {
            Clouds.Add(this);
            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        var transform1 = transform;
        var scaleFactor = transform1.localScale.x - 0.9F;
        var moveSpeed = scaleFactor * GlobalLogic.CurrentLogic.MoveSpeed * 0.3F;

        transform1.position += Vector3.left * (moveSpeed * Time.deltaTime);

        if ( !( transform1.position.x <= deadZone ) ) return;
        transform1.position = new Vector3(8, transform1.position.y, 0);
    }
}
