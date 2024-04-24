namespace Asteroids.ValueTypeECS.System
{
    public interface ISystem { }

    public interface IExecutableSystem : ISystem
    {
        void Execute();
    }
}
