using Interface;
using UnityEngine;

namespace forPrefab
{
    public class Cactus : MonoBehaviour, IInjectable
    {
        const float DeadZone = -10;

        readonly InjectHandler _injectHandler = new();
        ILogic CurrentLogic => _injectHandler.CurrentLogic;
        public void SetLogic(ILogic logic) => _injectHandler.SetLogic(logic);


        void Update()
        {
            if ( CurrentLogic == null )
            {
                Debug.LogWarning($"CurrentLogic is not set yet on {gameObject.name}");
                return;
            } 
            
            var gameMoveSpeed = CurrentLogic.MoveSpeed;
            transform.position += Vector3.left * (gameMoveSpeed * Time.deltaTime);
        
            if( transform.position.x <= DeadZone ) {
                Destroy( gameObject );
            }
        }
    }
}
