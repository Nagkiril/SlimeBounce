using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Visuals.Decals;

namespace SlimeBounce.Animations.Modules
{
    public class DecalSpawnModule : VisualModule
    {
        [SerializeField] private SlimeDecal _decalPrefab;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _fullColor;

        //There is reasoning that _floorMask should be put somewhere in the settings, and calculated out of there.
        private static int _floorMask = -1;

        protected override void Awake()
        {
            if (_floorMask == -1)
            {
                _floorMask = LayerMask.GetMask("Floor");
            }
        }


        public override void Activate(bool animate = true)
        {
            Ray rayToGround = new Ray(transform.position, transform.up * -1f);
            if (Physics.Raycast(rayToGround, out RaycastHit hitResult, 1f, _floorMask))
            {
                //Would be really cool to just pool those, and abstract their creation from spawn module
                var newDecal = Instantiate(_decalPrefab, transform.position, _decalPrefab.transform.rotation);
                newDecal.PlayColorized(_startColor, _fullColor);
            }
            NotifyTransitionDone();
        }

    }
}