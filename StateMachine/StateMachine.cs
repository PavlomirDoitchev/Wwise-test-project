using UnityEngine;

namespace Assets.Scripts.StateMachine
{
    /// <summary>
    /// Manages the current state and handles state transitions.
    /// </summary>
    public abstract class StateMachine : MonoBehaviour
    {
        private State currentState;
        private State previousState;
        public State PreviousState => previousState;
        public State CurrentState => currentState;

        /// <summary>
        /// Change the current state to a new state.
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(State newState)
        {
            previousState = currentState;
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        private void Update()
        {
            currentState?.Tick(Time.deltaTime);
        }
    }
}
