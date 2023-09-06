using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;
using SlimeBounce.Slime.Control.Indicator;

namespace SlimeBounce.Slime.Control
{
    public class SlimePickupControl : MonoBehaviour
    {
        [SerializeField] private SlimeIndicator _indicator;
        private const float PICKUP_FALL_SPEED = 30f;
        private const float PICKUP_FOLLOW_RATE = 12f;
        private const float PICKUP_FORWARD_OFFSET = -0.85f;
        private Vector3 _fallPosition;
        private Vector3 _fallPositionOffset;
        private SlimePickupState _state;

        public enum SlimePickupState
        {
            Disabled = 0,
            PickedUp = 1,
            FallingDown = 2
        }

        private void Update()
        {
            if (IsEngaged())
            {
                if (_state == SlimePickupState.PickedUp)
                {
                    transform.position = Vector3.Lerp(transform.position, CameraController.Instance.GetMouseClosePosition() + transform.forward * PICKUP_FORWARD_OFFSET, PICKUP_FOLLOW_RATE * Time.deltaTime);
                }
                if (_state == SlimePickupState.FallingDown)
                {
                    Vector3 fallTarget = _fallPosition - transform.position;
                    Vector3 fallDirection = fallTarget.normalized;
                    float fallMagnitude = Time.deltaTime * PICKUP_FALL_SPEED;
                    if (fallMagnitude * fallMagnitude > fallTarget.sqrMagnitude)
                    {
                        fallMagnitude = fallTarget.magnitude;
                    }
                    transform.position += fallDirection * fallMagnitude;
                }
            }
        }

        public bool IsEngaged() => _state != SlimePickupState.Disabled;

        public bool CanBePickedUp() => _state == SlimePickupState.Disabled;

        public void Pickup()
        {
            _state = SlimePickupState.PickedUp;
            _fallPosition = Vector3.zero;
            _indicator.Show();
        }

        public void SetFallPositionOffset(Vector3 newOffset)
        {
            _fallPositionOffset = newOffset;
        }

        public void Falldown()
        {
            _state = SlimePickupState.FallingDown;
            _fallPosition = _indicator.GetIndicatorPosition() + _fallPositionOffset;
            _indicator.Hide();
        }

        public void SetHoverMode(bool isHoverMode)
        {
            _indicator.SetVisualProhibition(isHoverMode);
        }

        public void ResetPickup()
        {
            _state = SlimePickupState.Disabled;
            _indicator.SetVisualProhibition(false);
            _indicator.Hide();
        }
    }
}