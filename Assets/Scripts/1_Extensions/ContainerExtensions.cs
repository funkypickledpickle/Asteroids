using System;
using Zenject;

namespace Asteroids.Extensions
{
    public static class ContainerExtensions
    {
        public static void BindAsSingleFromInstance<TFrom>(this DiContainer container, TFrom instance)
        {
            container.Bind<TFrom>().FromInstance(instance).AsSingle();
        }

        public static void BindAsSingleFromInstance(this DiContainer container, object instance, params Type[] types)
        {
            container.Bind(types).FromInstance(instance).AsSingle();
        }

        public static void BindAsSingle<TFrom>(this DiContainer container)
        {
            container.Bind<TFrom>().AsSingle();
        }

        public static void BindAsSingle<TFrom, TTarget>(this DiContainer container) where TTarget : TFrom
        {
            container.Bind<TFrom>().To<TTarget>().AsSingle();
        }

        public static void BindAsSingle<TFrom1, TFrom2, TTarget>(this DiContainer container) where TTarget : TFrom1, TFrom2
        {
            container.Bind(typeof(TFrom1), typeof(TFrom2)).To<TTarget>().AsSingle();
        }
    }
}
