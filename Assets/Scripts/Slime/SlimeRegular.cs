using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Slime
{
    public sealed class SlimeRegular : SlimeCore
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        protected override void OnClickStart()
        {
            base.OnClickStart();
            PickByPlayer();
        }

        protected override void OnClickEnd()
        {
            base.OnClickEnd();
            DropByPlayer();
        }

        public override void Despawn()
        {
            base.Despawn();
            //Spawn some VFX if need be
        }
    }
}
