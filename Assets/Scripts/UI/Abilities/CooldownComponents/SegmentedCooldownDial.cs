using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.UI.Abilities.CooldownComponents
{
    public class SegmentedCooldownDial : MonoBehaviour
    {
        [SerializeField] private Animator _ownAnim;
        [SerializeField] private CooldownSegment[] _segments;
        private int _rechargingIndex;

        public event Action OnCooldownPassed;
        public bool IsInCooldown => _rechargingIndex != -1;


        private void Awake()
        {
            foreach (var segment in _segments)
            {
                segment.OnSegmentFilled += OnCooldownSegmentFill;
            }
            _rechargingIndex = -1;
        }

        private void OnCooldownSegmentFill(CooldownSegment filledSegment)
        {
            if (filledSegment == GetRechargeSegment())
            {
                _rechargingIndex++;

                var nextRechargeSegment = GetRechargeSegment();
                if (nextRechargeSegment != null)
                {
                    nextRechargeSegment.StartRecharge();
                }
                else
                {
                    _rechargingIndex = -1;
                    Hide();
                    OnCooldownPassed?.Invoke();
                }
            }
        }

        private CooldownSegment GetRechargeSegment()
        {
            if (_rechargingIndex >= 0 && _rechargingIndex < _segments.Length)
            {
                return _segments[_rechargingIndex];
            }
            else
            {
                return null;
            }
        }

        public void RestartCooldown()
        {
            _ownAnim.SetBool("Shown", true);
            foreach (var segment in _segments)
            {
                segment.Show();
                segment.SetFillProgress(0f);
            }
            _rechargingIndex = 0;
            GetRechargeSegment().StartRecharge();
        }
        

        public void Hide()
        {
            _ownAnim.SetBool("Shown", false);
            foreach (var segment in _segments)
            {
                segment.Hide();
            }
        }
        
        public void RechargeSegment()
        {
            float oldProgress;
            var segmentToFill = GetRechargeSegment();
            if (segmentToFill != null)
            {
                oldProgress = segmentToFill.RechargeProgress;
                segmentToFill.SetFillProgress(1f);

                var nextSegment = GetRechargeSegment();
                if (nextSegment != null)
                    nextSegment.SetFillProgress(oldProgress);
            }
        }
    }
}