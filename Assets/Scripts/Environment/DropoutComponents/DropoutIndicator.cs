using SpecialEffects;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeBounce.Environment.DropoutComponents
{
    public class DropoutIndicator : CanvasIndicator
    {
        [SerializeField] private CooldownImage[] _cooldownImages;
        //Maybe make it share-able component of sorts? with inspector buttons? that'd be cool
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private float _scaleDuration;
        [SerializeField] private float _startOpacity;
        [SerializeField] private float _endOpacity;
        [SerializeField] private Color _criticalColor;
        [SerializeField] private float _criticalFrequency;
        [SerializeField] private FX _regularCooldownFX;
        [SerializeField] private FX _criticalCooldownFX;

        private Color _defaultColor;

        public event Action OnCooldownEnded;

        private float _cooldownProgress;
        private float _cooldownDuration;
        private bool _isCritical;

        //We don't really want to populate it with many variables as to not take too much memory during intensive cooldown update (by having to allocate a new color for each piece, etc)
        [Serializable]
        private class CooldownImage
        {
            public Image TargetImage;
            public bool UseThickColor;
        }


        private void Awake()
        {
            //First image is considered "main" one, altho we could derive it specifically
            _defaultColor = _cooldownImages[0].TargetImage.color;
            EndCooldown();
        }

        private void Update()
        {
            //We're not usng DOTween here because the way we animate certain values (i.e. color) may be complex, and using multiple Tweens for it seems excessive
            if (_cooldownProgress >= 0)
            {
                _cooldownProgress += Time.deltaTime / _cooldownDuration;
                var targetColorThin = GetCooldownThinColor();
                var targetColorThick = targetColorThin;
                targetColorThick.a *= 1.2f;
                var targetFillAmount = GetCooldownFillValue();
                var targetLocalScale = GetCooldownLocalScale();

                foreach (var imageEntry in _cooldownImages)
                {
                    var cooldownImage = imageEntry.TargetImage;
                    cooldownImage.color = ( imageEntry.UseThickColor ? targetColorThick : targetColorThin);
                    cooldownImage.fillAmount = targetFillAmount;
                    cooldownImage.transform.localScale = targetLocalScale;
                }
                if (_cooldownProgress >= 1)
                {
                    EndCooldown();
                }
            }
        }

        private Color GetCooldownThinColor()
        {
            var targetOpacity = Mathf.Lerp(_startOpacity, _endOpacity, _cooldownProgress);
            Color targetColor = _defaultColor;
            targetColor.a = targetOpacity;

            if (_isCritical)
            {
                var criticalHueRatio = Mathf.Sin(Mathf.PI * _cooldownProgress * _cooldownDuration * _criticalFrequency) * (1.1f - _cooldownProgress);
                targetColor = Color.Lerp(targetColor, _criticalColor, criticalHueRatio);
            }
            return targetColor;
        }

        private Vector3 GetCooldownLocalScale()
        {
            return Vector3.Lerp(_startScale, _endScale, _cooldownProgress * _cooldownDuration / _scaleDuration);
        }

        private float GetCooldownFillValue()
        {
            return 1f - _cooldownProgress;
        }

        public void StartCooldown(float cooldown, bool isCritical = false)
        {
            _cooldownDuration = cooldown;
            _cooldownProgress = 0;
            _isCritical = isCritical;
            foreach (var imageEntry in _cooldownImages)
            {
                imageEntry.TargetImage.gameObject.SetActive(true);
                imageEntry.TargetImage.fillAmount = 1f;
                imageEntry.TargetImage.transform.localScale = _startScale;
            }
            (isCritical ? _criticalCooldownFX : _regularCooldownFX).Show();
            (isCritical ? _regularCooldownFX : _criticalCooldownFX).Hide();
        }

        public void EndCooldown()
        {
            _cooldownProgress = -1f;
            foreach (var imageEntry in _cooldownImages)
            {
                imageEntry.TargetImage.gameObject.SetActive(false);
            }
            _criticalCooldownFX.Hide();
            _regularCooldownFX.Hide();
            OnCooldownEnded?.Invoke();
        }
    }
}
