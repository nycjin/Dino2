using Interface;
using Mirror;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Logic Manager of multi-mode
public class MultiLogic : NetworkBehaviour, ILogic
{
    [SyncVar] [SerializeField] float moveSpeed = 3.7f;
    [SerializeField] bool isNight;
    [SerializeField] float dayShiftInterval = 30f;
    [SerializeField] float speedIncreaseInterval = 50f;

    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] Text scoreText;
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject dinoPrefab;

    float _speedIntervalElapsed;
    float _dayTimeElapsed;

    public UnityEvent OnDayShift { get; private set; }
    public float MoveSpeed => moveSpeed;
    public float GameScore { get; private set; }

    void Awake()
    {
        OnDayShift = new UnityEvent();

        // Tag of player prefab is "Player", so it needs to be injected with logic.
        // However, in Multi-mode, CustomNetworkManager handles it,
        // so the prefab is instantiated dynamically after the network is connected.
    }

    void Update()
    {
        IncreaseSpeed();
        AddScore();
        ShiftDaylight();
    }

    void IncreaseSpeed()
    {
        if ( _speedIntervalElapsed > speedIncreaseInterval )
        {
            moveSpeed += 0.5f; // 시간당 이동 좌표
            _speedIntervalElapsed = 0;
        }
        else
        {
            _speedIntervalElapsed += Time.deltaTime;
        }
    }

    void AddScore()
    {
        GameScore += moveSpeed * Time.deltaTime;

        scoreText.text = ( ( int )GameScore ).ToString();
    }

    void ShiftDaylight()
    {
        if ( _dayTimeElapsed < dayShiftInterval * moveSpeed )
        {
            _dayTimeElapsed += moveSpeed * Time.deltaTime;
            return;
        }

        _dayTimeElapsed = 0;

        playerCamera.backgroundColor = isNight ? new Color32(247, 247, 247, 0) : new Color32(0, 0, 0, 0);
        isNight = !isNight;
        OnDayShift.Invoke();
    }

    public void GameOver()
    {
        gameOverCanvas.enabled = true;
    }

    public void Restart()
    {
        Debug.Log("Restart button pressed!"); //
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}