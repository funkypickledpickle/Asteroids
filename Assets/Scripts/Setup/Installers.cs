using Asteroids.Configuration.Game;
using Asteroids.Extensions;
using Asteroids.GameplayComponents.Controllers;
using Asteroids.GameplayComponents.Generated;
using Asteroids.GameplayECS.Containers;
using Asteroids.GameplayECS.Factories;
using Asteroids.Services.EntityView;
using Asteroids.Services.Project;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Components;
using Asteroids.ValueTypeECS.EntityContainer;
using Asteroids.ValueTypeECS.System;
using Zenject;

namespace Asteroids.Common
{
    public class ServicesInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindAsSingle<IInstanceSpawner, IDiContainerWrapper, DiContainerWrapper>();
            Container.BindAsSingle<IUnityResourcesService, UnityResourcesService>();
            Container.BindAsSingle<IConfigurationService, ScriptableObjectConfigurationService>();
            Container.BindAsSingle<IViewConfigurationService, UnityViewConfigurationService>();
            Container.BindAsSingle<IActionSchedulingService, ActionSchedulingService>();
        }
    }

    public class InputInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindAsSingle<GameplayInputCollection>();
            Container.BindAsSingle<NavigationInputCollection>();
            Container.BindAsSingle<InputController>();
        }
    }

    public class ECSInstaller : Installer
    {
        public override void InstallBindings()
        {
            var worldConfiguration = Container.Resolve<IConfigurationService>().Get<ContainersConfiguration>().WorldConfiguration;
            Container.BindAsSingleFromInstance(worldConfiguration);
            Container.BindAsSingle<World>();
            Container.BindAsSingle<IComponentsContainer, ComponentsContainer>();
            Container.BindAsSingle<SystemsManager>();
            Container.BindAsSingle<EntityFactory>();

            Container.BindAsSingle<IEntityViewService, EntityViewService>();
            Container.BindAsSingle<IEntityViewContainer, EntityViewContainer>();
        }
    }

    public class ControllersInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindAsSingle<GameController>();
        }
    }
}
