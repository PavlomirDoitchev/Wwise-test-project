namespace Assets.Scripts.StateMachine
{
    /// <summary>
    /// Base class for all states in the state machine.
    /// </summary>
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public abstract void Exit();
    }
}