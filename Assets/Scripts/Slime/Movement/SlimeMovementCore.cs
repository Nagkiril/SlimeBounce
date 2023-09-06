using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Slime.Movement
{
    public abstract class SlimeMovementCore : MonoBehaviour
    {
        protected bool _isMovementAllowed;
        protected float _movementSpeed;

        public event Action OnMovementStarted;
        public event Action OnMovementEnded;

        protected void NotifyMovementStart()
        {
            OnMovementEnded?.Invoke();
        }

        protected void NotifyMovementEnd()
        {
            OnMovementStarted?.Invoke();
        }


        protected virtual void Initialize()
        {
            IsMovementAllowed = true;
        }

        public virtual bool IsMovementAllowed
        {
            get => _isMovementAllowed;
            set
            {
                if (_isMovementAllowed != value)
                {
                    _isMovementAllowed = value;
                    if (_isMovementAllowed)
                        NotifyMovementStart();
                }
            }
        }

        public virtual float MovementSpeed
        {
            get => _movementSpeed;
            set
            {
                _movementSpeed = Mathf.Max(0, value);
            }
        }

        public abstract Vector3 GetMovementDelta();
    }
}