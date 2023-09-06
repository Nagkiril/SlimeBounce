using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Slime.ExtraModules.Suspension
{
    public class ParentSuspensionModule : MonoBehaviour, ISuspensionModule
    {
        [SerializeField] private SlimeCore _targetSlime;
        private Transform _suspensionParent;
        public bool IsSuspended => _suspensionParent != null;

        private void Awake()
        {
            if (_targetSlime != null)
            {
                _targetSlime.AddSuspensionModule(this);
            }
        }

        public bool Suspend(Transform suspension)
        {
            if (!IsSuspended)
            {
                ReparentSuspension(suspension);
            }
            return IsSuspended;
        }

        public bool Unsuspend(Transform suspension)
        {
            if (IsSuspended && suspension == _suspensionParent)
            {
                ReparentSuspension(null);
            }
            return !IsSuspended;
        }

        private void ReparentSuspension(Transform newParent)
        {
            _suspensionParent = newParent;
            transform.SetParent(newParent, (newParent == null));
        }
    }
}