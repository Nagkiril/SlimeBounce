using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs.Content.BarComponents
{
    public class BarGlow : MonoBehaviour
    {
        [SerializeField] SegmentedBar targetBar;
        [SerializeField] Image glowImage;
        [SerializeField] Color glowColor;
        [SerializeField] float glowDuration;

        Sequence _colorSequence;
        bool _isActive;
        bool _isInitialized;

        // Start is called before the first frame update
        void Awake()
        {
            targetBar.OnBarChanged += CheckGlowActivity;
        }


        private void OnDestroy()
        {
            targetBar.OnBarChanged -= CheckGlowActivity;
        }

        void CheckGlowActivity()
        {
            bool shouldBeActive = CheckTargetActivity();
            if (_isActive != shouldBeActive || !_isInitialized)
            {
                _isInitialized = true;
                _isActive = shouldBeActive;
                if (_colorSequence != null)
                {
                    _colorSequence.Kill();
                }
                _colorSequence = DOTween.Sequence();
                _colorSequence.Append(glowImage.DOColor(GetTargetColor(), glowDuration));
            }
        }



        bool CheckTargetActivity()
        {
            //Here we might implement any additional activation parameters, if we'll need any
            return targetBar.Progress == targetBar.MaxProgress;
        }

        Color GetTargetColor()
        {
            return (_isActive ? glowColor : Color.clear); 
        }
    }
}
