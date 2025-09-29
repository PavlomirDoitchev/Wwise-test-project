using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachine.Player
{
    /// <summary>
    /// The base state for all player states.
    /// </summary>
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine _playerStateMachine;

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this._playerStateMachine = stateMachine;
        }

    }
}
