using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Slime.Edibility
{
    public class SlimeNutrition : MonoBehaviour
    {
        [SerializeField] private SlimeEdibility _originalEdibility;
        private bool _nutritionLocked;
        public bool CanBeConsumed => _originalEdibility != SlimeEdibility.Inedible && !_nutritionLocked;

        public SlimeEdibility Edibility => _originalEdibility;

        public void Lock()
        {
            _nutritionLocked = true;
        }
    }
}