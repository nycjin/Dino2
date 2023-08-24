using Interface;
using UnityEngine;

namespace forDino
{
    // State Pattern(FSM) for dino
    public abstract class State : IDinoState
    {
        public abstract void Start(IStateHandler dino);
        public abstract void Update(IStateHandler dino);
        public virtual void OnCollisionEnter(Collision2D collision, IStateHandler dino)
        {
            if ( collision.gameObject.tag is not ("Ground" or "Player") )
            {
                dino.SetState(new DeadState());
            }
        }
    }

    public class RunState : State
    {
        public override void Start(IStateHandler dino)
        {
            dino.ForRunStart();
            dino.HandleLeftRight();
        }

        public override void Update(IStateHandler dino)
        {
            // Handle move inputs
            dino.HandleLeftRight();

            // Transition to Jump state
            if ( dino.HandleUp() )
            {
                dino.SetState(new JumpState());
            }

            // Transition to Duck state
            if ( dino.HandleDown() )
            {
                dino.SetState(new DuckState());
            }
        }
    }

    public class DuckState : State
    {
        public override void Start(IStateHandler dino)
        {
            dino.ForDuckStart();
        }

        public override void Update(IStateHandler dino)
        {
            if ( dino.HandleDownOff() )
            {
                dino.SetState(new RunState());
            }
        }
    }

    public class JumpState : State
    {
        public override void Start(IStateHandler dino)
        {
            dino.ForJumpStart();
        }

        public override void Update(IStateHandler dino)
        {
            dino.HandleDown();
        }

        public override void OnCollisionEnter(Collision2D collision, IStateHandler dino)
        {
            if ( collision.gameObject.CompareTag("Ground") )
            {
                dino.SetState(new RunState());
            }
            else
            {
                base.OnCollisionEnter(collision, dino);
            }
        }
    }

    public class DeadState : State
    {
        public override void Start(IStateHandler dino)
        {
            dino.ForDeadStart();
        }

        public override void Update(IStateHandler dino)
        {
            // pending if dead
        }
    }
}