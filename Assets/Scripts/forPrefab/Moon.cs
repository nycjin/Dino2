using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Moon : MonoBehaviour
{
    [SerializeField] List<Sprite> MoonPhase;
    [SerializeField] int _iPhase;
    
    SpriteRenderer _renderer;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();

        GlobalLogic.CurrentLogic.OnDayShift.AddListener(PhaseShift);
        
        gameObject.SetActive( false );
    }

    void PhaseShift()
    {
        if ( GlobalLogic.CurrentLogic.IsNight )
        {
            _iPhase = ( _iPhase + 1 ) % MoonPhase.Count;
            _renderer.sprite = MoonPhase[ _iPhase ];
        }
        gameObject.SetActive( !gameObject.activeSelf );
    }
}
