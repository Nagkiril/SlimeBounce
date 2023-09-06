using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce
{
    public class AEContainer : MonoBehaviour
    {
        public Action OnAnimationDone;
        public Action OnAnimationStart;
        public Action OnActionA;
        public Action OnActionB;
        public Action OnActionC;

        public void EventDone()
        {
            OnAnimationDone?.Invoke();
        }

        public void EventStart()
        {
            OnAnimationDone?.Invoke();
        }

        public void EventA()
        {
            OnActionA?.Invoke();
        }

        public void EventB()
        {
            OnActionB?.Invoke();
        }

        public void EventC()
        {
            OnActionC?.Invoke();
        }
    }
}