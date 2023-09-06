using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Environment
{
    public interface ILivesStateProvider
    {
        public event Action OnLivesChanged;
        public int Lives { get; }
        public int LastLivesDelta { get; }
    }
}