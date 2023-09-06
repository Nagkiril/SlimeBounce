using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Animations.Controllers;

namespace SlimeBounce.UI.LifeComponents
{
    public class LifeView : MonoBehaviour
    {
        [SerializeField] private ShowHideController _showController;
        private bool _isFilled;

        public void SetFilled(bool isFilled)
        {
            if (_isFilled != isFilled)
            {
                _isFilled = isFilled;
                if (isFilled)
                {
                    if (!_showController.IsShown)
                        _showController.Show();
                }
                else
                {
                    if (_showController.IsShown)
                        _showController.Hide();
                }
            }
        }
    }
}