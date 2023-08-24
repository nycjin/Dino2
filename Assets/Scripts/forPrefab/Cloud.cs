using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace forPrefab
{
    public class Cloud : MonoBehaviour, IInjectable
    {
        [SerializeField] float deadZone = -10;

        static readonly List<Cloud> Clouds = new();
        const int SIZE_INSTANCE = 3;

        readonly InjectHandler _injectHandler = new();
        ILogic CurrentLogic => _injectHandler.CurrentLogic;
        public void SetLogic(ILogic logic) => _injectHandler.SetLogic(logic);


        void Start()
        {
            if ( Clouds.Count < SIZE_INSTANCE )
            {
                Clouds.Add(this);
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if ( CurrentLogic == null )
            {
                Debug.LogWarning($"CurrentLogic is not set yet on {gameObject.name}");
                return;
            } 
            
            var scaleFactor = transform.localScale.x - 0.9F;
            var gameMoveSpeed = CurrentLogic.MoveSpeed;
            var cloudMoveSpeed = scaleFactor * gameMoveSpeed * 0.3F;

            transform.position += Vector3.left * ( cloudMoveSpeed * Time.deltaTime );

            if ( !( transform.position.x <= deadZone ) ) return;
            transform.position = new Vector3(8, transform.position.y, 0);
        }
    }
}