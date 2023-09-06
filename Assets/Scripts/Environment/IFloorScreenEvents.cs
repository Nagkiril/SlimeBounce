using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Environment
{
    public interface IFloorScreenEvents
    {
        public event Action OnLevelStartPressed;
        public event Action OnShopPressed;
        public event Action OnMenuPressed;
        public event Action OnNextLevelPressed;
        public event Action OnRetryPressed;
    }
}