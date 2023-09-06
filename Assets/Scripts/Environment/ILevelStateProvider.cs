using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Environment
{
    public interface ILevelStateProvider
    {
        public event Action OnLevelStarted;
        public event Action<int, int> OnLevelProgress;
        public event Action<bool> OnLevelEnded;
        public event Action OnLobbyEntered;

        public bool IsLevelInProgress { get; }
    }
}