using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SlimeBounce.UI.Settings;

namespace SlimeBounce.UI.Windows.LevelUpComponents
{
    public class RewardView : MonoBehaviour
    {
        [SerializeField] private Animator _ownAnim;
        [SerializeField] private TextMeshProUGUI _rewardHeader;
        [SerializeField] private TextMeshProUGUI _rewardDecription;
        [SerializeField] private Transform _headerContainer;


        public void Initialize(RewardViewData data)
        {
            _ownAnim.Update(0);
            _rewardHeader.text = data.Name;
            _rewardDecription.text = data.Description;
            Instantiate(data.Header, _headerContainer);
        }

        public void Show()
        {
            _ownAnim.SetBool("Shown", true);
        }
    }
}