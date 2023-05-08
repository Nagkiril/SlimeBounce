using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;
using SlimeBounce.Slime.Control.Indicator;

namespace SlimeBounce.Slime.Control
{
    public class SlimePickupControl : MonoBehaviour
    {
        [SerializeField] SlimeIndicator indicator;
        private const float PICKUP_FALL_SPEED = 30f;
        public const float PICKUP_FOLLOW_RATE = 8f;
        Vector3 _fallPosition;
        Vector3 _fallPositionOffset;

        private SlimePickupState state;
        public enum SlimePickupState
        {
            Disabled = 0,
            PickedUp = 1,
            FallingDown = 2
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (IsEngaged())
            {
                if (state == SlimePickupState.PickedUp)
                {
                    transform.position = Vector3.Lerp(transform.position, CameraController.Instance.GetMouseClosePosition(), PICKUP_FOLLOW_RATE * Time.deltaTime);
                }
                if (state == SlimePickupState.FallingDown)
                {
                    Vector3 fallDirection = (_fallPosition - transform.position).normalized;
                    transform.position += fallDirection * Time.deltaTime * PICKUP_FALL_SPEED;
                }
            }
        }

        public bool IsEngaged() => state != SlimePickupState.Disabled;

        public bool CanBePickedUp() => state == SlimePickupState.Disabled;

        public void Pickup()
        {
            state = SlimePickupState.PickedUp;
            _fallPosition = Vector3.zero;
            indicator.Show();
        }

        public void SetFallPositionOffset(Vector3 newOffset)
        {
            _fallPositionOffset = newOffset;
        }

        public void Falldown()
        {
            state = SlimePickupState.FallingDown;
            _fallPosition = indicator.GetIndicatorPosition() + _fallPositionOffset;
            indicator.Hide();
        }

        public void SetHoverMode(bool isHoverMode)
        {
            indicator.SetVisualProhibition(isHoverMode);
        }

        public void ResetPickup()
        {
            state = SlimePickupState.Disabled;
            indicator.SetVisualProhibition(false);
            indicator.Hide();
        }
    }
}