using UnityEngine.Events;

namespace Interface
{
    public interface ILogic
    {
        UnityEvent OnDayShift { get; }
        float MoveSpeed { get; }
        float GameScore { get; }

        void GameOver();
        void Restart();
    }
}