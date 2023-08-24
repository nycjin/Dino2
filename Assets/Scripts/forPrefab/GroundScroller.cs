using Interface;
using UnityEngine;

namespace forPrefab
{
    public class GroundScroller : MonoBehaviour, IInjectable
    {
        public GameObject another;
        float _endZone;
        float _groundWidth;

        readonly InjectHandler _injectHandler = new();
        ILogic CurrentLogic => _injectHandler.CurrentLogic;
        public void SetLogic(ILogic logic) => _injectHandler.SetLogic(logic);


        void Start()
        {
            _groundWidth = GetComponent<SpriteRenderer>().bounds.size.x - 1;
            _endZone = -_groundWidth;
        }

        void Update()
        {
            if ( CurrentLogic == null )
            {
                Debug.LogWarning($"CurrentLogic is not set yet on {gameObject.name}");
                return;
            }
            
            if( transform.position.x <= _endZone ) {
                transform.position = new Vector3( 
                    another.transform.position.x + _groundWidth, 
                    transform.position.y, 
                    transform.position.z );
            } else
            {
                var gameMoveSpeed = CurrentLogic.MoveSpeed;
                transform.position += Vector3.left * ( gameMoveSpeed * Time.deltaTime);
            }
        }
    }
}
