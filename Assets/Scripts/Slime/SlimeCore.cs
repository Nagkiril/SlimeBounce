using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Visuals;
using SlimeBounce.Slime.Control;
using SlimeBounce.Slime.Movement;
using SlimeBounce.Slime.Edibility;
using SlimeBounce.Environment;
using SlimeBounce.Slime.Status;
using DG.Tweening;

namespace SlimeBounce.Slime
{
    public abstract class SlimeCore : MonoBehaviour
    {
        [Header("Common References")]
        [SerializeField] protected SlimeVisualCore visuals;
        [SerializeField] protected SlimeInputReceiver input;
        [SerializeField] protected SlimeMovementCore movement;
        [SerializeField] protected SlimeCollision collision;
        [SerializeField] protected SlimePickupControl pickupControl;
        [SerializeField] protected SlimeNutrition nutrition;
        [SerializeField] protected SlimeStatusController status;
        [Header("Common Parameters")]
        [Space(10)]
        [SerializeField] protected float baseMovementSpeed;
        [SerializeField] public int lifeDamage = 1;

        public event Action OnSlimeDropped;
        public event Action OnSlimeHovered;
        public event Action OnSlimeEscaped;
        public event Action OnSlimeDestroyed;
        public event Action OnSlimeConsumed;

        public bool IsEngaged => pickupControl.IsEngaged();

        public bool CanBeConsumed => nutrition.CanBeConsumed;

        bool _isDespawned;

        protected bool PickByPlayer()
        {
            if (pickupControl.CanBePickedUp() && status.PickUpVerified)
            {
                movement.IsMovementAllowed = false;
                visuals.PickupChange(true);
                pickupControl.Pickup();
                return true;
            }
            return false;
        }

        protected void DropByPlayer()
        {
            if (pickupControl.IsEngaged())
            {
                visuals.PickupChange(false);
                pickupControl.Falldown();
                OnSlimeDropped?.Invoke();
            }
        }

        public SlimeEdibility ExtractNutrition()
        {
            nutrition.ChangeBlock(false);
            var edibility = nutrition.Edibility;
            //>> Consumption VFX can go here <<
            Destroy(gameObject);
            OnSlimeConsumed?.Invoke();
            return edibility;
        }

        public bool PrepareForConsumption()
        {
            var isPrepared = false;
            if (CanBeConsumed)
            {
                isPrepared = true;
                pickupControl.enabled = false;
                movement.enabled = false;
                nutrition.ChangeBlock(true);
            }
            return isPrepared;
        }

        public void SetHoverMode(bool isHovered)
        {
            pickupControl.SetHoverMode(isHovered);
        }

        public void Escape()
        {
            OnSlimeEscaped?.Invoke();
            Despawn();
        }

        public int GetLifeDamage() => lifeDamage;

        public void AdjustBaseSpeed(float multiplier) => baseMovementSpeed *= multiplier;

        #region Virtual

        // Start is called before the first frame update
        protected virtual void Start()
        {
            input.OnClickStart += OnClickStart;
            input.OnClickEnd += OnClickEnd;

            Vector3 floorFallOffset = collision.GetSize();
            floorFallOffset.x = 0;
            floorFallOffset.z *= -0.1f;
            floorFallOffset /= 2f;

            pickupControl.SetFallPositionOffset(floorFallOffset);

            movement.OnMovementStarted += OnMovementStart;
            movement.OnMovementEnded += OnMovementEnd;
            movement.MovementSpeed = baseMovementSpeed;
            movement.IsMovementAllowed = true;

            collision.Slime = this;
            collision.OnFloorTouched += OnFloorTouch;

            status.OnStatusExpired += OnStatusExpired;

            LevelController.OnLevelEnded += OnLevelEnd;
        }

        protected virtual void Update()
        {
            if (!pickupControl.IsEngaged() && !status.IsBound)
            {
                var frameDelta = movement.GetMovementDelta() * status.SpeedMultiplier * Time.deltaTime;
                
                if (!Floor.ValidateOnWidth((transform.position + frameDelta).x))
                {
                    frameDelta.x = 0;
                }
                transform.position += frameDelta;
            }
        }

        protected virtual void OnFloorTouch()
        {
            if (pickupControl.IsEngaged())
            {
                pickupControl.ResetPickup();
                movement.IsMovementAllowed = true;
            }
        }

        protected virtual void OnMovementStart()
        {

        }

        protected virtual void OnMovementEnd()
        {

        }

        protected virtual void OnClickStart()
        {
            Debug.Log("Clicked");
        }

        protected virtual void OnClickEnd()
        {
            Debug.Log("Unclicked");
        }

        protected virtual void OnLevelEnd(bool isWon)
        {
            nutrition.ChangeBlock(true);
            visuals.Hide();
            pickupControl.ResetPickup();
            movement.IsMovementAllowed = false;
            input.SetClickable(false);
            Destroy(gameObject, 1f);
        }

        protected virtual void OnDestroy()
        {
            Despawn();
            OnSlimeDestroyed?.Invoke();
        }

        protected virtual void OnStatusExpired(StatusEffect targetEffect)
        {
            visuals.HideVfx(targetEffect.StatusVfx);
        }

        public virtual void Despawn()
        {
            if (!_isDespawned)
            {
                LevelController.OnLevelEnded -= OnLevelEnd;
                Destroy(gameObject);
                _isDespawned = true;
            }
        }

        public virtual void ApplyStatusEffect(StatusEffect targetEffect)
        {
            if (status.ApplyStatusEffect(targetEffect))
            {
                visuals.AttachVfx(targetEffect.StatusVfx);
            }
        }

        

        #endregion
    }
}