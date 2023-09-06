using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Slime.Visuals;
using SlimeBounce.Slime.Status;
using SlimeBounce.Slime;
using SlimeBounce.Slime.Status.AuraComponents;
using Zenject;

namespace SlimeBounce.Slime
{
    public sealed class VoidSlime : DeployableSlime, IAbilitySlime
    {
        [Header("Void Slime Parameters")]
        [Space(10)]
        [SerializeField] private VoidAuraHandler _vortexPrefab;
        private bool _isEnhanced;
        private float _vortexStrength;
        private float _vortexDuration;

        [Inject]
        private VoidAuraHandler.Factory _vortexFactory;

        private void CreateVortex()
        {
            var newVortex = _vortexFactory.Create();
            newVortex.transform.position = transform.position;
            newVortex.Initialize(_isEnhanced, _vortexStrength, _vortexDuration);
        }

        protected override void OnFloorTouch()
        {
            base.OnFloorTouch();
            CreateVortex();
            Despawn();
        }

        public void SetEnhanced(bool isEnhanced)
        {
            _isEnhanced = isEnhanced;
        }

        public void ApplyParameters(List<float> parameters)
        {
            _vortexDuration = parameters[0];
            _vortexStrength = parameters[1];
        }
    }
}
