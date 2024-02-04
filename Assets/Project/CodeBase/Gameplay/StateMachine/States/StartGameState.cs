using CodeBase.Core.StateMachine;

namespace CodeBase.Gameplay.StateMachine.States
{
    public sealed class StartGameState : StateBase
    {
        public override void Enter()
        {
            StateMachine.Enter<PlayState>();
        }

        public override void Exit() { }
    }
}