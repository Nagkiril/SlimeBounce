using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.UI.LifeComponents
{
    public class LifeView : MonoBehaviour
    {
        [SerializeField] Animator ownAnim;
        bool _isFilled;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetFilled(bool isFilled)
        {
            if (_isFilled != isFilled)
            {
                _isFilled = isFilled;
                ownAnim.SetBool("Enabled", isFilled);
            }
        }
    }
}