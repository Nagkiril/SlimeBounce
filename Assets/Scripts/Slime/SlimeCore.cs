using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Visuals;
using SlimeBounce.Slime.Control;
using SlimeBounce.Slime.Movement;
using SlimeBounce.Slime.Edibility;
using SlimeBounce.Slime.ExtraModules;
using SlimeBounce.Environment;
using SlimeBounce.Slime.Status;
using DG.Tweening;
using Zenject;

namespace SlimeBounce.Slime
{
    public abstract class SlimeCore : MonoBehaviour
    {
        [Header("Common References")]
        //Here we have a massive list of mandatory, one-per-type components; Right now I'd say this approach is be flawed, but still better than not dividing into any components at all
        //Ideally, we may want to avoid such structures altogether and divide components in such a way that they either don't need much supervision or subdivide fluently (Status Effects work in analogous fashion, for example)
        //Alternatively, if current approach is preferred*, then we would want to only ever work with abstractions (interfaces\abstract classes); currently it is not the case with some components below
        //It should be noted that abstract classes are preferred over interfaces if we plan to reference them over Inspector;
        // * It is attractive because of simplicity, transparency that shows all mandatory modules clearly, yet allows for some polymorphism, while explicitly saying that only 1 module of each time can be used.
        //   This works, but produces a rigid, poorly-scalable design with issues akin to Singleton (things are coupled, 1 instance = 1 module); solution would be same as with Singleton - implement a more elaborate deisgn.
        [SerializeField] protected SlimeVisualCore _visuals;
        [SerializeField] protected SlimeInputReceiver _input;
        [SerializeField] protected SlimeMovementCore _movement;
        [SerializeField] protected SlimeCollision _collision;
        [SerializeField] protected SlimePickupControl _pickupControl;
        [SerializeField] protected SlimeNutrition _nutrition;
        [SerializeField] protected SlimeStatusController _status;
        [Header("Common Parameters")]
        [Space(10)]
        [SerializeField] protected float _baseMovementSpeed;
        [SerializeField] protected int _lifeDamage = 1;
        private bool _isDespawned;
        //Suspension module is optional for a slime - it may not exist; For such modules, I think registering them is the best way to accomodate them in current architecture
        private ISuspensionModule _suspension;

        [Inject]
        protected ILevelStateProvider _levelState;
        [Inject]
        protected IFloorBoundsProvider _floorBounds;


        public event Action OnSlimeDropped;
        public event Action OnSlimePicked;
        public event Action OnSlimeEscaped;
        public event Action OnSlimeDestroyed;
        public event Action OnSlimeConsumed;
        public event Action<bool> OnSlimeSuspended;

        public bool IsEngaged => _pickupControl.IsEngaged();
        public bool IsSuspended => (_suspension != null ? _suspension.IsSuspended : false);
        public int LifeDamage { get => _lifeDamage; }
        public bool CanBeConsumed => _nutrition.CanBeConsumed;

        protected bool PickByPlayer()
        {
            if (_pickupControl.CanBePickedUp() && _status.PickUpVerified)
            {
                _movement.IsMovementAllowed = false;
                _visuals.PickupChange(true);
                _pickupControl.Pickup();
                OnSlimePicked?.Invoke();
                return true;
            }
            return false;
        }

        protected void DropByPlayer()
        {
            if (_pickupControl.IsEngaged())
            {
                _visuals.PickupChange(false);
                _pickupControl.Falldown();
                OnSlimeDropped?.Invoke();
            }
        }

        public SlimeEdibility ExtractNutrition()
        {
            var edibility = _nutrition.Edibility;
            Despawn();
            OnSlimeConsumed?.Invoke();
            return edibility;
        }

        public bool PrepareForConsumption()
        {
            var isPrepared = false;
            if (CanBeConsumed)
            {
                isPrepared = true;
                _pickupControl.enabled = false;
                _nutrition.Lock();
                _visuals.PrepareConsumption();
            }
            return isPrepared;
        }

        public void SetHoverMode(bool isHovered)
        {
            _pickupControl.SetHoverMode(isHovered);
        }

        public void Escape()
        {
            OnSlimeEscaped?.Invoke();
            Despawn();
        }

        public void AddSuspensionModule(ISuspensionModule suspension)
        {
            if (_suspension == null)
            {
                _suspension = suspension;
            }
        }

        public int GetLifeDamage() => LifeDamage;

        public void AdjustBaseSpeed(float multiplier) => _baseMovementSpeed *= multiplier;

        #region Virtual
        protected virtual void Start()
        {
            _input.OnClickStarted += OnClickStart;
            _input.OnClickEnded += OnClickEnd;

            Vector3 floorFallOffset = _collision.GetSize();
            floorFallOffset.x = 0;
            floorFallOffset.z = 0;
            floorFallOffset.y *= 0.4f;

            _pickupControl.SetFallPositionOffset(floorFallOffset);

            _movement.OnMovementStarted += OnMovementStart;
            _movement.OnMovementEnded += OnMovementEnd;
            _movement.MovementSpeed = _baseMovementSpeed;
            _movement.IsMovementAllowed = true;

            _collision.Slime = this;
            _collision.OnFloorTouched += OnFloorTouch;

            _status.OnStatusExpired += OnStatusExpired;

            _levelState.OnLevelEnded += OnLevelEnd;
        }

        protected virtual void Update()
        {
            if (!_pickupControl.IsEngaged() && !IsSuspended)
            {
                var frameDelta = Vector3.zero;
                if (!_status.IsBound)
                {
                    frameDelta += _movement.GetMovementDelta() * _status.SpeedMultiplier * Time.deltaTime;

                    if (!_floorBounds.ValidateOnWidth((transform.position + frameDelta).x))
                    {
                        frameDelta.x = 0;
                    }
                }
                frameDelta += _status.GetStatusMove() * 100f * Time.deltaTime;
                transform.position += frameDelta;
            }
        }

        protected virtual void OnFloorTouch()
        {
            if (_pickupControl.IsEngaged())
            {
                _visuals.TouchFloor();
                _pickupControl.ResetPickup();
                _movement.IsMovementAllowed = true;
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

        }

        protected virtual void OnClickEnd()
        {

        }

        protected virtual void OnLevelEnd(bool isWon)
        {
            Despawn();
        }

        protected virtual void OnDestroy()
        {
            //Despawn();
            OnSlimeDestroyed?.Invoke();
        }

        protected virtual void OnStatusExpired(StatusEffect targetEffect)
        {
            if (targetEffect.StatusVfx != null)
                _visuals.HideVfx(targetEffect.StatusVfx);
        }

        public virtual bool Suspend(Transform targetSuspension)
        {
            bool suspended = false;
            if (!_isDespawned && _suspension != null)
            {
                suspended = _suspension.Suspend(targetSuspension);
                if (suspended)
                {
                    _movement.IsMovementAllowed = false;
                    OnSlimeSuspended?.Invoke(true);
                }
            }
            return suspended;
        }

        public virtual bool Unsuspend(Transform targetSuspension)
        {
            bool unsuspended = false;
            if (!_isDespawned && _suspension != null)
            {
                _movement.IsMovementAllowed = true;
                if (unsuspended)
                    OnSlimeSuspended?.Invoke(false);
            }
            return (_suspension == null ? false : _suspension.Unsuspend(targetSuspension));
        }

        public virtual void Despawn()
        {
            if (!_isDespawned)
            {
                _levelState.OnLevelEnded -= OnLevelEnd;
                _status.Clear();
                _nutrition.Lock();
                _visuals.Hide();
                _pickupControl.ResetPickup();
                _movement.IsMovementAllowed = false;
                _input.SetClickable(false);
                Destroy(gameObject, 1f);
                _isDespawned = true;
            }
        }

        public virtual void ApplyStatusEffect(StatusEffect targetEffect)
        {
            if (_status.ApplyStatusEffect(targetEffect))
            {
                if (targetEffect.StatusVfx != null)
                    _visuals.AttachVfx(targetEffect.StatusVfx);
            }
        }

        #endregion

        public class Factory : PrefabFactory<SlimeCore>
        {
        }
    }
}