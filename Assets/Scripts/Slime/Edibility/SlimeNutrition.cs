using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Slime.Edibility
{
    public class SlimeNutrition : MonoBehaviour
    {
        [SerializeField] SlimeEdibility originalEdibility;
        bool _nutritionBlocked;
        public bool CanBeConsumed => originalEdibility != SlimeEdibility.Inedible && !_nutritionBlocked;


        public SlimeEdibility Edibility => (CanBeConsumed ? originalEdibility : SlimeEdibility.Inedible);

        public void ChangeBlock(bool isBlocked)
        {
            _nutritionBlocked = isBlocked;
        }
    }
}