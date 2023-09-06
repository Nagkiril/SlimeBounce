using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.UI.Windows.Settings
{
    public interface IWindowInstanceProvider
    {

        public T GetWindowInstance<T>(Transform newInstanceRoot) where T : WindowBase;
    }
}