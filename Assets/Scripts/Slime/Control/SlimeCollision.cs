using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;

namespace SlimeBounce.Slime.Control
{
    public class SlimeCollision : MonoBehaviour
    {
        public event Action OnFloorTouched;
        public SlimeCore Slime 
        { 
            get => _slimeReference;
            set
            {
                if (_slimeReference == null)
                {
                    _slimeReference = value;
                }
                else
                {
                    Debug.LogWarning("SlimeCollision's CoreSlime reference must be set only once per instance!");
                }
            }
        }

        private SlimeCore _slimeReference;

        private void OnTriggerEnter(Collider other)
        {
            var floor = other.GetComponent<Floor>();
            if (floor != null)
            {
                OnFloorTouched?.Invoke();
            }
        }

        public Vector3 GetSize()
        {
            return transform.localScale * 0.6f;
        }
    }
}