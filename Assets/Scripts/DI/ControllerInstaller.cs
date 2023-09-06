using UnityEngine;
using Zenject;
using SlimeBounce.Environment;
using SlimeBounce.Player;
using SlimeBounce.Abilities;
using SlimeBounce.UI.Windows;

namespace SlimeBounce.DI
{
    public class ControllerInstaller : MonoInstaller
    {
        [SerializeField] private UpgradeController _upgradeController;
        [SerializeField] private AbilityController _abilityController;
        [SerializeField] private DropoutController _dropoutController;
        [SerializeField] private ExpController _expController;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private WindowController _windowController;
        [SerializeField] private CoinController _coinController;

        public override void InstallBindings()
        {
            Container.Bind<IUpgradeActor>().FromInstance(_upgradeController);
            Container.Bind<IUpgradeDataProvider>().FromInstance(_upgradeController);
            Container.Bind<IAbilityActor>().FromInstance(_abilityController);
            Container.Bind<IAbilityCooldownHub>().FromInstance(_abilityController);
            Container.Bind<IDropoutCooldownManager>().FromInstance(_dropoutController);
            Container.Bind<IPlayerExpManager>().FromInstance(_expController);
            Container.Bind<ILevelStateProvider>().FromInstance(_levelController);
            Container.Bind<ILivesStateProvider>().FromInstance(_levelController);
            Container.Bind<IWindowActor>().FromInstance(_windowController);
            Container.Bind<ICoinActor>().FromInstance(_coinController);
        }
    }
}