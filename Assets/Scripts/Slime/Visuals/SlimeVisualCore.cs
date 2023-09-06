using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;
using DG.Tweening;
using SlimeBounce.Animations.Modules;
using SlimeBounce.Settings;
using SlimeBounce.Slime.Visuals.Face;
using SlimeBounce.Slime.Visuals.Face.Settings;
using Zenject;

namespace SlimeBounce.Slime.Visuals
{
    public class SlimeVisualCore : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private string[] _implementedStates;
        [SerializeField] private Transform _vfxAttachmentPoint;
        [Header("Face Setup")]
        [SerializeField] private SlimeType _slimeMoodType;
        [SerializeField] private SlimeFace _face;
        [SerializeField] private Color _faceTint;
        [Header("Visual Event Modules")]
        [SerializeField] private VisualModule[] _pickupModules;
        [SerializeField] private VisualModule[] _groundedModules;
        [SerializeField] private VisualModule[] _hideModules;
        [SerializeField] private CustomState[] _customStates;

        [Inject]
        private IFaceSlimeSettings _faceSettings;


        [Serializable]
        private class CustomState
        {
            public string StateName;
            public VisualModule[] Modules;
        }

        private List<FX> _attachedVfx;


        protected void Awake()
        {
            _attachedVfx = new List<FX>();
        }

        protected void Start()
        {
            _face.SetFaceData(_faceSettings.GetFaceData(_slimeMoodType));
            _face.SetFaceType(FaceType.Idle);
            _face.ApplyTint(_faceTint);
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

        public bool SetCustomState(string stateName, bool isActive)
        {
            var customState = Array.Find(_customStates, x => x.StateName == stateName);
            if (customState != null)
            {
                foreach (var module in customState.Modules)
                {
                    if (isActive)
                        module.Activate();
                    else
                        module.Deactivate();
                }
            }
            return customState != null;
        }

        public void PickupChange(bool isPicked)
        {
            foreach (var module in _pickupModules)
            {
                if (isPicked)
                    module.Activate();
                else
                    module.Deactivate();
            }
            _face.SetFaceType((isPicked ? FaceType.Picked : FaceType.Idle));
        }

        public void Hide()
        {
            foreach (var module in _hideModules)
            {
                module.Activate();
            }
            _face.Hide();
        }

        public void TouchFloor()
        {
            foreach (var module in _groundedModules)
            {
                module.Activate();
            }
        }

        public void AttachVfx(FX vfx)
        {
            var newVfx = Instantiate(vfx, _vfxAttachmentPoint);
            newVfx.gameObject.name = vfx.name;
            _attachedVfx.Add(newVfx);
        }

        public void HideVfx(FX vfx)
        {
            var attachedVfx = FindVfxByName(vfx.name);
            if (attachedVfx != null)
                attachedVfx.Hide();
        }

        public void PrepareConsumption()
        {
            _face.SetFaceType(FaceType.Dead);
        }
    }
}