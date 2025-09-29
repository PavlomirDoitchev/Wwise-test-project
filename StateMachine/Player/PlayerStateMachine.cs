using Assets.Scripts.StateMachine.Player.States;
namespace Assets.Scripts.StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {
        private void Start()
        {
            ChangeState(new PlayerLocomotionState(this));
        }
    }
}
