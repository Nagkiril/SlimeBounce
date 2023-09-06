using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;
using SlimeBounce.Slime;

namespace SlimeBounce.Abilities.Components.SlimeEffects
{
    public class SlimeSpawnerFX : MonoBehaviour
    {
        [SerializeField] private SlimeEffector _targetEffector;
        [SerializeField] private FX _targetPrefab;
        [SerializeField] private bool _isImpulse;

        private Dictionary<SlimeCore, FX> _spawnedEffects;


        private void Awake()
        {
            _targetEffector.OnSlimeEffect += SpawnEffectNearSlime;
            if (!_isImpulse)
            {
                _spawnedEffects = new Dictionary<SlimeCore, FX>();
                _targetEffector.OnSlimeRevert += RemoveEffectNearSlime;
            }
        }

        private void OnDestroy()
        {
            _targetEffector.OnSlimeEffect -= SpawnEffectNearSlime;
        }

        private void SpawnEffectNearSlime(SlimeCore target)
        {
            if (_isImpulse || !_spawnedEffects.ContainsKey(target))
            {
                var fx = Instantiate(_targetPrefab, target.transform.position, _targetPrefab.transform.rotation);
                if (!_isImpulse)
                {
                    _spawnedEffects.Add(target, fx);
                    fx.OnDestruction += (fx) => { FreeEntry(target); };
                }
            }
        }

        private void RemoveEffectNearSlime(SlimeCore target)
        {
            var fx = _spawnedEffects[target];
            if (fx != null)
            {
                fx.Hide();
                FreeEntry(target);
            }
        }

        private void FreeEntry(SlimeCore target)
        {
            _spawnedEffects.Remove(target);
        }
    }
}