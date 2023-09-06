using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Player
{
    public interface ICoinActor
    {
        public bool ChangeCoins(int delta);
        public bool ChangeCoins(int value, Vector3 worldPosition);
    }
}