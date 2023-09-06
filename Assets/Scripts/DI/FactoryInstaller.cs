using UnityEngine;
using Zenject;
using SlimeBounce.Abilities;
using SlimeBounce.Slime;
using SlimeBounce.UI.Abilities;
using SlimeBounce.UI.Windows;
using SlimeBounce.Slime.Loot;
using SlimeBounce.Slime.Status;
using SlimeBounce.Slime.Status.AuraComponents;
using SlimeBounce.Environment.DropoutComponents;

namespace SlimeBounce.DI
{
    public class FactoryInstaller : MonoInstaller
    {
        [SerializeField] private AbilityControl _abilityControlPrefab;
        [SerializeField] private VoidAuraHandler _voidAuraPrefab;

        public override void InstallBindings()
        {
            Container.BindFactory<AbilityControl, AbilityControl.Factory>().FromComponentInNewPrefab(_abilityControlPrefab);
            Container.BindFactory<VoidAuraHandler, VoidAuraHandler.Factory>().FromComponentInNewPrefab(_voidAuraPrefab);
            Container.Bind<SlimeCore.Factory>().AsSingle();
            Container.Bind<AbilityCore.Factory>().AsSingle();
            Container.Bind<StatusEffect.Factory>().AsSingle();
            Container.Bind<WindowBase.Factory>().AsSingle();
            Container.Bind<PickableLoot.Factory>().AsSingle();
            Container.Bind<DropoutZone.Factory>().AsSingle();
        }
    }
}