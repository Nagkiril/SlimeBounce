using System;
using SlimeBounce.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SlimeBounce.Abilities
{
    public abstract class AbilityCore : MonoBehaviour
    {
        public event Action<AbilityCore> OnAbilityEnded;

        public bool IsEnhanced { get; protected set; }
        public bool IsActivated { get; protected set; }

        [Inject]
        protected ILevelStateProvider _levelState;

        protected virtual void Awake()
        {
            _levelState.OnLevelEnded += OnLevelEnd;
        }

        protected virtual void OnDestroy()
        {
            _levelState.OnLevelEnded -= OnLevelEnd;
        }

        protected virtual void OnLevelEnd(bool isWin)
        {
            EndAbility();
        }

        protected void NotifyAbilityEnd()
        {
            OnAbilityEnded?.Invoke(this);
        }

        protected void EndAbility()
        {
            if (IsActivated)
            {
                IsActivated = false;
                Finish();
                OnAbilityEnded?.Invoke(this);
            }
        }

        protected abstract void Initialize(List<float> abilityStats, bool isEnhanced);
        protected abstract void Finish();

        public void StartAbility(List<float> abilityStats, bool isEnhanced)
        {
            if (!IsActivated)
            {
                IsActivated = true;
                Initialize(abilityStats, isEnhanced);
            }
        }

        public class Factory : PrefabFactory<AbilityCore>
        {

        }
    }
}