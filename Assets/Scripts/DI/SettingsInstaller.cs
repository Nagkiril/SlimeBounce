using UnityEngine;
using Zenject;
using SlimeBounce.Abilities.Settings;
using SlimeBounce.Environment.Settings;
using SlimeBounce.Settings;
using SlimeBounce.Player.Settings;
using SlimeBounce.UI.Settings;
using SlimeBounce.UI.Windows;
using SlimeBounce.UI.Windows.Settings;
using SlimeBounce.Slime.Visuals.Face.Settings;
using SlimeBounce.Slime.Status.Settings;

namespace SlimeBounce.DI
{
    public class SettingsInstaller : MonoInstaller
    {
        //This installer is pretty messy, but I don't have any particularly good ideas of how to manage it, beside categorizing it further
        [SerializeField] private DropoutScriptableSettings _dropoutSettings;
        [SerializeField] private AbilityScriptableSettings _abilitySettings;
        [SerializeField] private CooldownViewScriptableSettings _cooldownViewSettings;
        [SerializeField] private FaceSlimeScriptableSettings _faceSlimeSettings;
        [SerializeField] private FaceSpriteScriptableSettings _faceSpriteSettings;
        [SerializeField] private LevelScriptableSettings _levelSettings;
        [SerializeField] private PlayerLevelScriptableSettings _playerLevelSettings;
        [SerializeField] private PlayerRewardViewSettings _rewardViewSettings;
        [SerializeField] private SpawnScriptableSettings _spawnSettings;
        [SerializeField] private StatusEffectSettings _statusSettings;
        [SerializeField] private UpgradeScriptableSettings _upgradeSettings;
        [SerializeField] private UpgradeViewSettings _upgradeViewSettings;
        [SerializeField] private WindowScriptableSettings _windowSettings;
        [SerializeField] private WindowSceneSettings _windowSceneSettings;
        [SerializeField] private WindowPoolCanvas _poolCanvas;



        public override void InstallBindings()
        {
            Container.Bind<IDropoutSettings>().FromInstance(_dropoutSettings);
            Container.Bind<IAbilitySettings>().FromInstance(_abilitySettings);
            Container.Bind<ICooldownViewSettings>().FromInstance(_cooldownViewSettings);
            Container.Bind<IFaceSlimeSettings>().FromInstance(_faceSlimeSettings);
            Container.Bind<IFaceSpriteSettings>().FromInstance(_faceSpriteSettings);
            Container.Bind<ILevelSettings>().FromInstance(_levelSettings);
            Container.Bind<IPlayerLevelSettings>().FromInstance(_playerLevelSettings);
            Container.Bind<IPlayerRewardViewSettings>().FromInstance(_rewardViewSettings);
            Container.Bind<ISpawnSettings>().FromInstance(_spawnSettings);
            Container.Bind<IStatusSettings>().FromInstance(_statusSettings);
            Container.Bind<IUpgradeSettings>().FromInstance(_upgradeSettings);
            Container.Bind<IUpgradeViewSettings>().FromInstance(_upgradeViewSettings);
            Container.Bind<IWindowSettings>().FromInstance(_windowSettings);
            Container.Bind<IWindowInstanceProvider>().FromInstance(_poolCanvas);
        }
    }
}