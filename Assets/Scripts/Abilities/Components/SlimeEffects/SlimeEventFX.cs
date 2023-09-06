using SlimeBounce.Slime;
using SpecialEffects;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Abilities.Components.SlimeEffects
{
    public class SlimeEventFX : MonoBehaviour
    {
        [SerializeField] private SlimeEffector _targetEffector;
        [SerializeField] private string _eventName;
        [SerializeField] private FX[] _effects;


        private void Awake()
        {
            _targetEffector.OnSlimeEffect += TriggerEvent;
        }

        private void OnDestroy()
        {
            _targetEffector.OnSlimeEffect -= TriggerEvent;
        }

        private void TriggerEvent(SlimeCore target)
        {
            foreach (var effect in _effects)
            {
                effect.ApplyEvent(_eventName);
            }
        }
    }
}