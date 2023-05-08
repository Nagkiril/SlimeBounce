using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs.Content.BarComponents
{
    public class BarSegment : MonoBehaviour
    {
        [SerializeField] RectTransform ownRect;
        [SerializeField] Image segmentVisual;
        [SerializeField] Animator ownAnim;



        public void SetSegmentReach(bool isReached, bool animate)
        {
            if (animate)
                ownAnim.SetTrigger("Reach");

            ownAnim.SetBool("Reached", isReached);
        }

        public Vector2 GetSegmentPosition() => ownRect.anchoredPosition;
    }
}