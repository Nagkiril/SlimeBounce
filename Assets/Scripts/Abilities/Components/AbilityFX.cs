using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;

namespace SlimeBounce.Abilities.Components
{
    public class AbilityFX : MonoBehaviour
    {
        [SerializeField] private FX _targetFX;
        [SerializeField] private AbilityCore _targetAbility;
        [SerializeField] private AbilityTriggerType _activityType;
        [SerializeField] private bool _onlyOnEnhance;

        enum AbilityTriggerType
        {
            ActiveThroughDuration,
            ActivateOnEnd
        }

        private void Start()
        {
            if (_onlyOnEnhance && !_targetAbility.IsEnhanced)
            {
                Debug.Log("Awake destruction");
                Destroy(gameObject);
                return;
            }
            _targetAbility.OnAbilityEnded += OnAbilityEnd;

            if (_activityType == AbilityTriggerType.ActiveThroughDuration)
            {
                _targetFX.Show();
            } else
            {
                _targetFX.Hide();
            }
        }

        private void OnAbilityEnd(AbilityCore endedAbility)
        {
            if (endedAbility == _targetAbility)
            {
                switch (_activityType)
                {
                    case AbilityTriggerType.ActiveThroughDuration:
                        _targetFX.Hide();
                        break;
                    case AbilityTriggerType.ActivateOnEnd:
                        _targetFX.Show();
                        break;
                }
            }
        }
    }
}