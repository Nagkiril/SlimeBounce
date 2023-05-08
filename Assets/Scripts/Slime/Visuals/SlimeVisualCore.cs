using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;
using DG.Tweening;

namespace SlimeBounce.Slime.Visuals
{
    public abstract class SlimeVisualCore : MonoBehaviour
    {
        [SerializeField] protected Animator ownAnim;
        [SerializeField] protected string[] implementedStates;
        [SerializeField] protected Transform vfxAttachmentPoint;

        List<FX> _attachedVfx;


        protected virtual void Awake()
        {
            _attachedVfx = new List<FX>();
        }

        public virtual bool SetCustomState(string stateName, bool isActive)
        {
            var stateExists = Array.Exists(implementedStates, x => x == stateName);
            if (stateExists)
            {
                ownAnim.SetBool(stateName, isActive);
            }
            return stateExists;
        }

        public virtual void PickupChange(bool isPicked)
        {
            ownAnim.SetBool("Picked", isPicked);
        }

        public virtual void Hide()
        {
            var hideSequence = DOTween.Sequence();
            hideSequence.Append(transform.DOScale(Vector3.zero, 2f).SetEase(Ease.OutExpo));
        }

        public virtual void AttachVfx(FX vfx)
        {
            var newVfx = Instantiate(vfx, vfxAttachmentPoint);
            newVfx.gameObject.name = vfx.name;
            _attachedVfx.Add(newVfx);
        }

        public virtual void HideVfx(FX vfx)
        {
            var attachedVfx = FindVfxByName(vfx.name);
            if (attachedVfx != null)
                attachedVfx.Hide();
        }


        protected FX FindVfxByName(string name)
        {
            FX foundVfx = null;
            foreach (var activeVfx in _attachedVfx)
            {
                if (activeVfx != null && name == activeVfx.name)
                {
                    foundVfx = activeVfx;
                    break;
                }
            }
            return foundVfx;
        }
    }
}