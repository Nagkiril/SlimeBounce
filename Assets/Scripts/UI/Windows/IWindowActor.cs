using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.UI.Windows
{
    public interface IWindowActor
    {
        public T OpenWindow<T>(WindowOpenType openType = WindowOpenType.Default) where T : WindowBase;
    }
}