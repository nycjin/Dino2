using System.Collections;
using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace forPrefab
{
    public class Moon : MonoBehaviour, IInjectable
    {
        [SerializeField] List<Sprite> MoonPhase;
        int _iPhase;

        SpriteRenderer _renderer;

        readonly InjectHandler _injectHandler = new();
        ILogic CurrentLogic => _injectHandler.CurrentLogic;
        public void SetLogic(ILogic logic) => _injectHandler.SetLogic(logic);


        IEnumerator Start()
        {
            while ( CurrentLogic == null )
            {
                yield return null;
            }
            
            _renderer = GetComponent<SpriteRenderer>();
            CurrentLogic.OnDayShift.AddListener(PhaseShift);
        }

        void PhaseShift()
        {
            if ( _renderer.enabled )
            {
                _iPhase = ( _iPhase + 1 ) % MoonPhase.Count;
                _renderer.sprite = MoonPhase[ _iPhase ];
            }

            _renderer.enabled = !_renderer.enabled;
        }
    }
}
