using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Visuals.Decals;
using SpecialEffects;

namespace SlimeBounce.Animations.Modules
{
    public class VfxSpawnModule : VisualModule
    {
        [SerializeField] private FX _prefabFX;



        public override void Activate(bool animate = true)
        {
            if (animate)
                Instantiate(_prefabFX, transform.position, _prefabFX.transform.rotation);
            NotifyTransitionDone();
        }
    }
}