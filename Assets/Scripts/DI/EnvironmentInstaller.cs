using UnityEngine;
using Zenject;
using SlimeBounce.Environment;
using SlimeBounce.Slime.Loot;
using SlimeBounce.UI.Windows;

namespace SlimeBounce.DI
{
    public class EnvironmentInstaller : MonoInstaller
    {
        [SerializeField] private SceneLootEnvironment _sceneLootEnvironment;
        [SerializeField] private DeployableContainer _deploymentContainer;
        [SerializeField] private Floor _floor;
        [SerializeField] private GameScriptableSetup _gameSetup;

        public override void InstallBindings()
        {
            Container.Bind<ILootEnvironmentProvider>().FromInstance(_sceneLootEnvironment);
            Container.Bind<IDeploymentActor>().FromInstance(_deploymentContainer);
            Container.Bind<IFloorBoundsProvider>().FromInstance(_floor);
            Container.Bind<IFloorScreenEvents>().FromInstance(_floor);
            Container.Bind<IGameSetupHandler>().FromInstance(_gameSetup);
        }
    }
}
