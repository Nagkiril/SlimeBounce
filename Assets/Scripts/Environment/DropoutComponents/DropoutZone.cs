using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime;
using SlimeBounce.Slime.Edibility;
using DG.Tweening;

namespace SlimeBounce.Environment.DropoutComponents
{
    public class DropoutZone : MonoBehaviour
    {
        [SerializeField] DropoutVisuals visual;
        [SerializeField] DropoutCanvasIndicator indicator;
        [SerializeField] DropoutCollision collision;
        [SerializeField] float explosionCooldownMultiplier; //>> This should be in settings somewhere
        bool _poisedToEat;

        DropoutZoneData _ownData;
        SlimeCore _targetSlime;

        Sequence _cooldownTimer;

        public void Initialize(DropoutZoneData data)
        {
            _ownData = data;
            if (indicator == null)
                indicator = _ownData.SpawnedIndicator;
            transform.position = indicator.GetScenePosition();

            if (!indicator.IsLeftSided)
            {
                transform.localScale -= new Vector3(2f * transform.localScale.x, 0, 0);
                collision.SetLeftSided(indicator.IsLeftSided);
            }
            collision.OnSlimeHover += OnSlimeHovered;
            collision.OnSlimeHoverEnd += OnSlimeHoverEnd;

            visual.OnConsumptionFinish += OnSlimeConsumed;
        }

        private void Awake()
        {
            LevelController.OnLevelStarted += OnLevelStart;
            LevelController.OnLevelEnded += OnLevelEnd;
        }

        private void OnDestroy()
        {
            LevelController.OnLevelStarted -= OnLevelStart;
            LevelController.OnLevelEnded -= OnLevelEnd;
        }

        void OnLevelStart()
        {
            SetReady(true);
        }

        void OnLevelEnd(bool isWin)
        {
            SetReady(false);
        }

        void OnSlimeHovered(SlimeCore slime)
        {
            if (_poisedToEat)
            {
                _targetSlime = slime;
                _targetSlime.SetHoverMode(true);
                visual.SetHovered(true);
                _targetSlime.OnSlimeDropped += OnDropInZone;
            }
        }

        void OnSlimeHoverEnd(SlimeCore slime)
        {
            if (_poisedToEat && slime != null && _targetSlime == slime)
            {
                _targetSlime.SetHoverMode(false);
                _targetSlime = null;
                visual.SetHovered(false);
            }
        }


        void OnDropInZone()
        {
            if (_poisedToEat && _targetSlime != null && _targetSlime.PrepareForConsumption()) 
            {
                visual.ConsumeItem(_targetSlime.transform);
            }
        }

        void OnSlimeConsumed()
        {
            if (_targetSlime != null)
            {
                var consumedNutrition = _targetSlime.ExtractNutrition();
                if (_poisedToEat)
                {
                    SetReady(false);
                    if (consumedNutrition == SlimeEdibility.Explosive)
                    {
                        //>> VFX goes here <<
                        //Visual call to trigger stagger animation goes here
                        StartCooldown(_ownData.Cooldown * explosionCooldownMultiplier);
                    }
                    else
                    {
                        StartCooldown(_ownData.Cooldown);
                    }
                }
                _targetSlime = null;
            }
        }

        void StartCooldown(float cooldownTime)
        {
            _cooldownTimer = DOTween.Sequence();
            _cooldownTimer.AppendInterval(cooldownTime);
            _cooldownTimer.AppendCallback(() => { SetReady(true); });
        }

        void SetReady(bool isReady)
        {
            _poisedToEat = isReady;
            visual.SetAvailable(isReady);
        }

        public void Hide()
        {
            if (_cooldownTimer != null)
                _cooldownTimer.Kill();
            SetReady(false);
        }
    }

    public class DropoutZoneData
    {
        public DropoutCanvasIndicator SpawnedIndicator;
        public float Cooldown;
    }
}