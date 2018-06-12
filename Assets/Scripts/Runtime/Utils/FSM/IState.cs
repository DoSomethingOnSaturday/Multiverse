namespace Multiverse.Runtime
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Update();
    }
}