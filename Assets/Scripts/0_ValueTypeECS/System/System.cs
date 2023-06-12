namespace Asteroids.ValueTypeECS.System
{
    public interface ISystem { }

    public interface IInitializableSystem : ISystem
    {
        void Initialize();
    }

    public interface IExecutableSystem : ISystem
    {
        void Execute();
    }

    public interface IStateSystem : ISystem
    {
        void WorldWillReset();
    }
}
