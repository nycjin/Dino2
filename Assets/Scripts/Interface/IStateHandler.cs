namespace Interface
{
    public interface IStateHandler
    {
        void SetState(IDinoState state);
        void ForRunStart();
        void ForJumpStart();
        void ForDuckStart();
        void ForDeadStart();
        void HandleLeftRight();
        bool HandleDown();
        bool HandleDownOff();
        bool HandleUp();
    }
}