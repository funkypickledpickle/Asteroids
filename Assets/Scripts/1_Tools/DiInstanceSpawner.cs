using Zenject;

namespace Asteroids.Tools
{
    public interface IInstanceSpawner
    {
        T Instantiate<T>();
    }

    public class DiInstanceSpawner : IInstanceSpawner
    {
        private readonly DiContainer _container;

        public DiInstanceSpawner(DiContainer container)
        {
            _container = container;
        }

        public T Instantiate<T>()
        {
            return _container.Instantiate<T>();
        }
    }
}
