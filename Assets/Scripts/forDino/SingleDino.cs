using System.Collections;
using Interface;
using UnityEngine;

namespace forDino
{
    public class SingleDino : MonoBehaviour, IInjectable
    {
        [SerializeField] float _jumpStrength = 5F;
        [SerializeField] float _moveStrength = 3.5f;
        
        readonly InjectHandler _injectHandler = new();
        public ILogic CurrentLogic => _injectHandler.CurrentLogic;
        public void SetLogic(ILogic logic) => _injectHandler.SetLogic(logic);

        bool _onTheGround;
        bool _isDuck;
        bool _isDead;

        Animator _animator;
        Rigidbody2D _rigidbody2D;

        void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            Debug.Log("Time scale: " + Time.timeScale); //
            Time.timeScale = 1;
        }

        void Update()
        {
            HandleMove();
            HandleJump();
            HandleDuck();
            HandleDeath();

            _animator.SetBool("isJump", !_onTheGround);
            _animator.SetBool("isCollide", _isDead);
            _animator.SetBool("isDuck", _isDuck);
        }

        void HandleJump()
        {
            if ( _onTheGround && ( Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) ) )
            {
                _rigidbody2D.velocity = Vector2.up * _jumpStrength;
            }

            // 점프강도
            if ( transform.position.y > 0 && ( Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow) ) )
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * 0.7f);
            }
        }

        void HandleMove()
        {
            if ( _onTheGround && Input.GetKey(KeyCode.RightArrow) )
            {
                _rigidbody2D.velocity = Vector2.right * _moveStrength;
            }

            if ( _onTheGround && Input.GetKey(KeyCode.LeftArrow) )
            {
                _rigidbody2D.velocity = Vector2.left * _moveStrength;
            }
        }

        void HandleDuck()
        {
            if ( Input.GetKeyDown(KeyCode.DownArrow) )
            {
                if ( _onTheGround )
                {
                    _isDuck = true;
                }
                else
                {
                    _rigidbody2D.velocity = Vector2.down * _jumpStrength;
                }
            }

            if ( Input.GetKeyUp(KeyCode.DownArrow) )
            {
                _isDuck = false;
            }
        }

        void HandleDeath()
        {
            if ( !_isDead ) return;

            CurrentLogic.GameOver();
            StartCoroutine(StopGame());
        }

        static IEnumerator StopGame()
        {
            yield return new WaitForSeconds(0.01f); // About 10 frames in 60fps

            Time.timeScale = 0;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if ( collision.gameObject.CompareTag("Ground") ) // Landing
            {
                _onTheGround = true;
            }
            else if ( !collision.gameObject.CompareTag("Player") )
            {
                _isDead = true;
            }
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if ( collision.gameObject.CompareTag("Ground") )
            {
                _onTheGround = false;
            }
        }
    }
}