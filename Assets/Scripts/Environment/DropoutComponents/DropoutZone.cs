using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime;
using SlimeBounce.Slime.Edibility;
using DG.Tweening;
using Zenject;

namespace SlimeBounce.Environment.DropoutComponents
{
    public class DropoutZone : MonoBehaviour
    {
        [SerializeField] private DropoutVisuals _visual;
        [SerializeField] private DropoutIndicator _indicator;
        [SerializeField] private DropoutCollision _collision;
        [SerializeField] private float _explosionCooldownMultiplier; //>> This should be in settings somewhere
        private bool _poisedToEat;
        private DropoutZoneData _ownData;
        private SlimeCore _targetSlime;
        [Inject]
        private ILevelStateProvider _levelState;


        public event Action OnSlimeConsumed;



        private void Awake()
        {
            _levelState.OnLevelStarted += OnLevelStart;
            _levelState.OnLevelEnded += OnLevelEnd;
        }

        public void Initialize(DropoutZoneData data)
        {
            if (_ownData == null)
            {
                if (_indicator == null)
                    _indicator = data.SpawnedIndicator;
                _indicator.OnCooldownEnded += OnCooldownEnd;
                transform.position = _indicator.GetScenePosition();
                if (!_indicator.IsLeftSided)
                {
                    transform.localScale -= new Vector3(2f * transform.localScale.x, 0, 0);
                    _collision.SetLeftSided(_indicator.IsLeftSided);
                }
                _collision.OnSlimeHoverStart += OnSlimeHover;
                _collision.OnSlimeHoverEnd += OnSlimeHoverEnd;
                _visual.OnConsumptionFinish += OnSlimeConsume;
            }
            _ownData = data;
            if (_levelState.IsLevelInProgress)
                OnLevelStart();
        }

        private void OnDestroy()
        {
            _levelState.OnLevelStarted -= OnLevelStart;
            _levelState.OnLevelEnded -= OnLevelEnd;
            _indicator.OnCooldownEnded -= OnCooldownEnd;
        }

        private void OnLevelStart()
        {
            SetReady(true);
        }

        private void OnLevelEnd(bool isWin)
        {
            SetReady(false);
        }

        private void OnSlimeHover(SlimeCore slime)
        {
            if (_poisedToEat && _targetSlime == null)
            {
                _targetSlime = slime;
                _targetSlime.SetHoverMode(true);
                _visual.SetHovered(true);
                _targetSlime.OnSlimeDropped += OnDropInZone;
            }
        }

        private void OnSlimeHoverEnd(SlimeCore slime)
        {
            if (_poisedToEat && slime != null && _targetSlime == slime)
            {
                _targetSlime.SetHoverMode(false);
                _targetSlime = null;
                _visual.SetHovered(false);
            }
        }


        private void OnDropInZone()
        {
            if (_poisedToEat && _targetSlime != null && _targetSlime.PrepareForConsumption()) 
            {
                _visual.ConsumeItem(_targetSlime.transform);
            }
        }

        private void OnSlimeConsume()
        {
            if (_targetSlime != null)
            {
                var consumedNutrition = _targetSlime.ExtractNutrition();
                if (_poisedToEat)
                {
                    SetReady(false);
                    if (consumedNutrition == SlimeEdibility.Explosive)
                    {
                        StartCooldown(_ownData.Cooldown * _explosionCooldownMultiplier, true);
                    }
                    else
                    {
                        StartCooldown(_ownData.Cooldown);
                    }
                }
                _targetSlime = null;
                OnSlimeConsumed?.Invoke();
            }
        }

        private void StartCooldown(float cooldownTime, bool isCriticalCooldown = false)
        {
            _indicator.StartCooldown(cooldownTime, isCriticalCooldown);
        }

        private void SetReady(bool isReady)
        {
            _poisedToEat = isReady;
            _visual.SetAvailable(isReady);
        }

        private void OnCooldownEnd()
        {
            SetReady(true);
        }

        public void Hide()
        {
            _indicator.EndCooldown();
            SetReady(false);
        }

        public void MultiplyCooldown(float multiplier)
        {
            _ownData.Cooldown *= multiplier;
        }

        public void ResetCooldown()
        {
            _indicator.EndCooldown();
        }

        public class Factory : PrefabFactory<DropoutZone>
        {

        }
    }

    public class DropoutZoneData
    {
        public DropoutIndicator SpawnedIndicator;
        public float Cooldown;
    }
}