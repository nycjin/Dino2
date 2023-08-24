using System;
using Interface;
using Mirror;
using UnityEngine;

namespace forDino
{
    public class PlayerScore : NetworkBehaviour, IInjectable
    {
        readonly InjectHandler _injectHandler = new();
        ILogic CurrentLogic => _injectHandler.CurrentLogic;
        public void SetLogic(ILogic logic) => _injectHandler.SetLogic(logic);

        [SyncVar] public int index;
        [SyncVar] public float score;
        ILogic _currentLogic;
        bool _canUpdate = true;

        public void StopUpdate()
        {
            _canUpdate = false;
        }

        void Update()
        {
            if ( isServer && _canUpdate )
            {
                if ( CurrentLogic == null )
                {
                    Debug.LogWarning($"CurrentLogic is not set yet on {gameObject.name}");
                    return;
                }

                score = CurrentLogic.GameScore;
            }
        }

        void OnGUI()
        {
            GUI.Box(new Rect(10f + ( index * 160 ), 10f, 150f, 25f), $"플레이어[{index + 1}]: {Math.Floor(score)}");
        }
    }
}