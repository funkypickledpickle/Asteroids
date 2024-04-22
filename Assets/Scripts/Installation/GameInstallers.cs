using Asteroids.Tools;
using Asteroids.UI.Input;
using Zenject;

namespace Asteroids.Installation
{
    public class ViewServicesInstaller : Installer
    {
        public override void InstallBindings()
        {

        }
    }

    public class InputInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindAsSingle<GameplayInputCollection>();
            Container.BindAsSingle<NavigationInputCollection>();
        }
    }

}
