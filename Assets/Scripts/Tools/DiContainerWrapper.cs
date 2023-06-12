using Zenject;

namespace Asteroids.Tools
{
    public interface IInstanceSpawner
    {
        T Instantiate<T>();
    }

    public interface IDiContainerWrapper : IInstanceSpawner
    {
        void Inject(object target);
        void Bind<T>(T instance);
        void Bind<TFrom, TTarget>() where TTarget : TFrom;
        T Resolve<T>();
        IDiContainerWrapper CreateChildContainer();

        T Instantiate<T>();
    }

    public class DiContainerWrapper : IDiContainerWrapper
    {
        private readonly DiContainer _container;

        public DiContainerWrapper(DiContainer container)
        {
            _container = container;
        }

        public void Inject(object target)
        {
            _container.Inject(target);
        }

        public void Bind<T>(T instance)
        {
            _container.Bind<T>().FromInstance(instance).AsSingle();
        }

        public void Bind<TFrom, TTarget>() where TTarget : TFrom
        {
            _container.Bind<TFrom>().To<TTarget>().AsSingle();
        }

        public T Instantiate<T>()
        {
            return _container.Instantiate<T>();
        }

        public IDiContainerWrapper CreateChildContainer()
        {
            return new DiContainerWrapper(_container.CreateSubContainer());
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
