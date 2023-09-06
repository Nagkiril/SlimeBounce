using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Player
{
    public interface IPlayerExpManager
    {
        public float LastExpProgress { get; }
        public event Action OnExpChanged;
        public event Action OnLevelUp;

        public float GetLevelProgress();

        public void AddExp(float value);
    }
}