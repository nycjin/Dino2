using System.Collections;
using Interface;
using Mirror;
using UnityEngine;

namespace forDino
{
    public class MultiDinoWithState : NetworkBehaviour, IInjectable, IStateHandler
    {
        [SerializeField] float jumpStrength = 5F;
        [SerializeField] float moveStrength = 3.5f;

        readonly InjectHandler _injectHandler = new();
        ILogic CurrentLogic => _injectHandler.CurrentLogic;
        public void SetLogic(ILogic logic) => _injectHandler.SetLogic(logic);

        Animator _dinoAnimator;
        Rigidbody2D _rigidbody2D;

        IDinoState _currentState;
        PlayerScore _playerScore;

        void IStateHandler.SetState(IDinoState newState)
        {
            _currentState = newState;
            _currentState.Start(this);
        }

        void Awake()
        {
            _dinoAnimator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerScore = gameObject.GetComponent<PlayerScore>();

            _currentState = new RunState();
            _currentState.Start(this); // Manually call Start method only for the first time
        }

        void Update()
        {
            _currentState.Update(this);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if ( collision.gameObject.CompareTag("Player") )
                return;

            _currentState.OnCollisionEnter(collision, this);
        }

        #region HandleMove

        /// <summary>
        ///  사용자 입력을 체크만 담당
        ///  Multi 모드에서 isLocalPlayer 사용을 위함.
        ///  (HandleLeftRight 메소드는 제외)
        /// </summary>
        public void HandleLeftRight()
        {
            // 본인의 캐릭터만 제어
            if ( !isLocalPlayer )
                return;

            if ( Input.GetKey(KeyCode.RightArrow) )
            {
                _rigidbody2D.velocity = Vector2.right * moveStrength;
            }

            if ( Input.GetKey(KeyCode.LeftArrow) )
            {
                _rigidbody2D.velocity = Vector2.left * moveStrength;
            }
        }

        public bool HandleDown()
        {
            if ( isLocalPlayer && Input.GetKeyDown(KeyCode.DownArrow) )
            {
                // 다운점프(공중에서 아래로 가속)를 위함
                _rigidbody2D.velocity = Vector2.down * jumpStrength;

                return true;
            }

            return false;
        }

        public bool HandleDownOff()
        {
            return isLocalPlayer && Input.GetKeyUp(KeyCode.DownArrow);
        }

        public bool HandleUp()
        {
            return isLocalPlayer && ( Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) );
        }

        #endregion

        #region ForStartFunctions

        public void ForRunStart()
        {
            _dinoAnimator.SetBool("isJump", false);
            _dinoAnimator.SetBool("isDuck", false);
            _dinoAnimator.SetBool("isCollide", false);
        }

        public void ForJumpStart()
        {
            _dinoAnimator.SetBool("isJump", true);

            _rigidbody2D.velocity = Vector2.up * jumpStrength;

            // 점프강도
            if ( transform.position.y > 0 && ( Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow) ) )
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * 0.7f);
            }
        }

        public void ForDuckStart()
        {
            _dinoAnimator.SetBool("isDuck", true);

            _rigidbody2D.velocity = Vector2.down * jumpStrength;
        }

        public void ForDeadStart()
        {
            _dinoAnimator.SetBool(name: "isCollide", true);

            if ( isLocalPlayer )
            {
                if ( CurrentLogic == null )
                {
                    Debug.LogError($"CurrentLogic is not set yet on {gameObject.name}");
                    return;
                }

                CurrentLogic.GameOver();
            }

            StartCoroutine(StopGame());

            // Stop updating player score
            _playerScore.StopUpdate();
        }

        #endregion

        // 어색함을 피하기 위해 10 프레임 후에 멈춤.
        static IEnumerator StopGame()
        {
            yield return new WaitForSeconds(0.01f); // About 10 frames in 60fps

            // Time.timeScale = 0;
        }
    }
}