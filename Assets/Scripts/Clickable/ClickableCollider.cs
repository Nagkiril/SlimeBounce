using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SlimeBounce.Clickable
{
    public class ClickableCollider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private bool _isClickable;
        public event Action OnClickStarted;
        public event Action OnClickEnded;
        public bool IsClicked => _isClicked;
        private bool _isClicked;

        public void SetClickable(bool clickable)
        {
            _isClickable = clickable;
            gameObject.SetActive(_isClickable);
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isClickable)
            {
                _isClicked = true;
                OnClickStarted?.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isClicked = false;
            OnClickEnded?.Invoke();
        }
    }
}