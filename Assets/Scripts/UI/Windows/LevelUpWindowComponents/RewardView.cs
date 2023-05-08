using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SlimeBounce.Settings;

namespace SlimeBounce.UI.Windows.LevelUpComponents
{
    public class RewardView : MonoBehaviour
    {
        [SerializeField] Animator ownAnim;
        [SerializeField] TextMeshProUGUI rewardHeader;
        [SerializeField] TextMeshProUGUI rewardDecription;
        [SerializeField] Transform headerContainer;


        public void Initialize(RewardViewData data)
        {
            ownAnim.Update(0);
            rewardHeader.text = data.Name;
            rewardDecription.text = data.Description;
            Instantiate(data.Header, headerContainer);
        }

        public void Show()
        {
            ownAnim.SetBool("Shown", true);
        }
    }
}