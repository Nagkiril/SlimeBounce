using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeBounce.UI.Abilities.CooldownComponents
{
    public class CooldownSegment : MonoBehaviour
    {
        [SerializeField] private Animator _ownAnim;
        [SerializeField] private Image _fillMask;
        [SerializeField] private Image _frame;
        [SerializeField] private Color _frameEmptyColor;
        [SerializeField] private Color _frameFullColor;
        [SerializeField] private float _fillEmpty;
        [SerializeField] private float _fillFull;
        [SerializeField] private float _segmentDuration;
        private bool _isRecharging;

        public event Action<CooldownSegment> OnSegmentFilled;
        public float RechargeProgress { get; private set; }

        private void Update()
        {
            if (_isRecharging)
            {
                UpdateRecharge(RechargeProgress + Time.deltaTime / _segmentDuration);
            }
        }

        private void UpdateRecharge(float newRecharge)
        {
            RechargeProgress = newRecharge;
            CheckRechargeFinish();
            RenderRechargeProgress();
        }

        private void RenderRechargeProgress()
        {
            _fillMask.fillAmount = Mathf.Lerp(_fillEmpty, _fillFull, RechargeProgress);
            _frame.color = Color.Lerp(_frameEmptyColor, _frameFullColor, RechargeProgress);
        }

        private void CheckRechargeFinish()
        {
            if (_isRecharging && RechargeProgress >= 1f)
            {
                _isRecharging = false;
                RechargeProgress = 1f;
                _ownAnim.SetBool("Recharging", false);
                OnSegmentFilled?.Invoke(this);
            }
        }

        public void StartRecharge(bool emptyOldProgress = false)
        {
            if (emptyOldProgress)
                SetFillProgress(0);
            _isRecharging = true;
            _ownAnim.SetBool("Recharging", true);
        }

        public void SetFillProgress(float progress)
        {
            UpdateRecharge(Mathf.Clamp01(progress));
        }

        public void Hide()
        {
            _isRecharging = false;
            _ownAnim.SetBool("Shown", false);
            _ownAnim.SetBool("Recharging", false);
        }

        public void Show()
        {
            _ownAnim.SetBool("Shown", true);
        }
    }
}