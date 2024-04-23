using Asteroids.Configuration;
using Asteroids.Gameplay.Controllers;
using Asteroids.Gameplay.States;
using Asteroids.GameplayECS.Factories;
using Asteroids.Services;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Components;
using Asteroids.ValueTypeECS.EntityContainer;
using Asteroids.ValueTypeECS.System;
using Zenject;

namespace Asteroids.Installation
{
    public class CommonInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindAsSingle<IResourcesService, ResourcesService>();
            Container.BindAsSingle<IInstanceSpawner, DiInstanceSpawner>();
        }
    }

    public class ConfigurationInstaller : Installer
    {
        private const string ConfigurationsContainerPath = "Assets/Resources/Configurations/Container";

        public override void InstallBindings()
        {
            var resourcesService = Container.Resolve<IResourcesService>();
            var configurationContainer = resourcesService.GetAsset<UnityConfigurationContainer>(ConfigurationsContainerPath);
            var gameWorld = Container.Resolve<IGameWorld>();

            Container.BindAsSingle<ViewPathsContainer>(() => configurationContainer.ViewPathsContainer);
            Container.BindAsSingle<GameConfiguration>((() => configurationContainer.GameConfiguration));
            Container.BindAsSingle<ContainersConfiguration>(() => configurationContainer.ContainersConfiguration);

            Container.BindAsSingle<FieldConfiguration>(() => new FieldConfiguration()
            {
                Rect = gameWorld.WorldRect,
            });
        }
    }

    public class ECSInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindAsSingle<World>();
            Container.BindAsSingle<IComponentsContainer, ComponentsContainer>();
            Container.BindAsSingle<SystemsManager>();
            Container.BindAsSingle<EntityFactory>();
        }
    }

    public class ControllersInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindAsSingle<GameController>();
        }
    }

    public class GameStatesInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindAsSingle<IStateContext, StateContext>();
            Container.BindAsSingle<InitializationState>();
            Container.BindAsSingle<MenuState>();
            Container.BindAsSingle<StartGameState>();
            Container.BindAsSingle<GameState>();
            Container.BindAsSingle<PauseState>();
            Container.BindAsSingle<EndGameState>();
        }
    }
}
