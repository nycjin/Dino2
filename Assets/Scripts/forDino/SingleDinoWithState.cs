using System.Collections;
using forDino;
using Interface;
using Unity.Collections;
using UnityEngine;

namespace forDino
{
    public class SingleDinoWithState : MonoBehaviour, IInjectable, IStateHandler
    {
        [SerializeField] [ReadOnly] float _jumpStrength = 5F;
        [SerializeField] [ReadOnly] float _moveStrength = 3.5f;
        
        readonly InjectHandler _injectHandler = new();
        public ILogic CurrentLogic => _injectHandler.CurrentLogic;
        public void SetLogic(ILogic logic) => _injectHandler.SetLogic(logic);

        Animator _dinoAnimator;
        Rigidbody2D _rigidbody2D;

        IDinoState _currentState;

        void IStateHandler.SetState(IDinoState newState)
        {
            _currentState = newState;
            _currentState.Start(this);
        }

        void Awake()
        {
            _dinoAnimator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _currentState = new RunState();
            _currentState.Start(this); // Manually call Start method only for the first time

            Time.timeScale = 1; // After reload the scene
        }

        void Update()
        {
            if ( CurrentLogic == null )
            {
                Debug.LogWarning($"CurrentLogic is not set yet on {gameObject.name}");
                return;
            } 
            
            _currentState.Update(this);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
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
            if ( Input.GetKey(KeyCode.RightArrow) )
            {
                _rigidbody2D.velocity = Vector2.right * _moveStrength;
            }

            if ( Input.GetKey(KeyCode.LeftArrow) )
            {
                _rigidbody2D.velocity = Vector2.left * _moveStrength;
            }
        }

        public bool HandleDown()
        {
            return Input.GetKey(KeyCode.DownArrow);
        }
        
        public bool HandleDownOff()
        {
            return Input.GetKeyUp(KeyCode.DownArrow);
        }

        public bool HandleUp()
        {
            return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
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

            _rigidbody2D.velocity = Vector2.up * _jumpStrength;

            // 점프강도
            if ( transform.position.y > 0 && ( Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow) ) )
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * 0.7f);
            }
        }

        public void ForDuckStart()
        {
            _dinoAnimator.SetBool("isDuck", true);

            _rigidbody2D.velocity = Vector2.down * _jumpStrength;
        }

        public void ForDeadStart()
        {
            _dinoAnimator.SetBool("isCollide", true);

            CurrentLogic.GameOver();

            StartCoroutine(StopGame());
        }

        #endregion

        // 어색함을 피하기 위해 10 프레임 후에 멈춤.
        static IEnumerator StopGame()
        {
            yield return new WaitForSeconds(0.01f); // About 10 frames in 60fps

            Time.timeScale = 0;
        }
    }
}