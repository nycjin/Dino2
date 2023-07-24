using System.Net.Mime;
using UnityEngine;
using UnityEngine.Events;

public interface ILogic
{
    UnityEvent OnDayShift { get; }
    bool IsNight { get; }
    float MoveSpeed { get;  }
    GameObject DinoPrefab { get; }

    void GameOver();
    void Restart();
}