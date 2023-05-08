using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SlimeBounce.Clickable
{
    public class ClickableCollider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] bool isClickable;
        public event Action OnClickStart;
        public event Action OnClickEnd;
        public bool IsClicked => _isClicked;
        private bool _isClicked;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void SetClickable(bool clickable)
        {
            isClickable = clickable;
            gameObject.SetActive(isClickable);
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (isClickable)
            {
                _isClicked = true;
                OnClickStart?.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isClicked = false;
            OnClickEnd?.Invoke();
        }
    }
}