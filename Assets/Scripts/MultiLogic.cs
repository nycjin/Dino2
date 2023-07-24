using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiLogic : MonoBehaviour, ILogic
{
    public static MultiLogic Instance { get; private set; }
    public UnityEvent OnDayShift { get; } = new(); // Event Listener for Moon phase

    [SerializeField] bool _isNight;
    [SerializeField] float _moveSpeed = 3.7f; // 기준 게임 속도. 점점 증가.
    [SerializeField] double _playerScore;
    [SerializeField] double DAY_SHIFT_INTERVAL = 100;
    [SerializeField] float SPEED_INCREASE_INTERVAL = 10f;
    float _speedIntervalElapsed;
    float _dayTimeElapsed;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Text scoreText;
    [SerializeField] Camera _camera;

    [SerializeField] GameObject _dinoPrefab; //

    public bool IsNight => _isNight;
    public float MoveSpeed => _moveSpeed;

    public GameObject DinoPrefab => _dinoPrefab;

    void Awake()
    {
        if ( Instance != null && Instance != this )
        {
            Destroy(( this ));
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        IncreaseSpeed();
        AddScore();
        ShiftDaylight();
    }

    void IncreaseSpeed()
    {
        if ( _speedIntervalElapsed > SPEED_INCREASE_INTERVAL )
        {
            _moveSpeed += 0.5f; // 시간당 이동 좌표
            _speedIntervalElapsed = 0;
        }
        else
        {
            _speedIntervalElapsed += Time.deltaTime;
        }
    }

    void AddScore()
    {
        _playerScore += Time.deltaTime * _moveSpeed;
        _dayTimeElapsed += Time.deltaTime * _moveSpeed; //
        scoreText.text = ( ( int )_playerScore ).ToString();
    }

    void ShiftDaylight()
    {
        if ( _dayTimeElapsed < DAY_SHIFT_INTERVAL * _moveSpeed ) // change to 200
        {
            return;
        }

        _camera.backgroundColor = _isNight ? new Color32(247, 247, 247, 0) : new Color32(0, 0, 0, 0);

        _dayTimeElapsed = 0;
        _isNight = !_isNight;
        
        OnDayShift?.Invoke();
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        Debug.Log("Restart button pressed!"); //
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _playerScore = 0;
        Time.timeScale = 1;

        if ( gameOverScreen.activeSelf )
        {
            gameOverScreen.SetActive(false);
        }
    }
}