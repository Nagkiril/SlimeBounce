using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime;

namespace SlimeBounce.Abilities.Components
{
    public class SlimeEffector : MonoBehaviour
    {
        private List<SlimeCore> _affectedSlimes;

        public event Action<SlimeCore> OnSlimeEffect;
        public event Action<SlimeCore> OnSlimeRevert;

        private void Awake()
        {
            _affectedSlimes = new List<SlimeCore>();
        }


        public void ApplyEffects(SlimeCore target)
        {
            if (!IsAffected(target))
                _affectedSlimes.Add(target);
            OnSlimeEffect?.Invoke(target);
        }

        public void RevertEffects(SlimeCore target)
        {
            if (IsAffected(target))
                _affectedSlimes.Remove(target);
            OnSlimeRevert?.Invoke(target);
        }

        public bool IsAffected(SlimeCore target) => _affectedSlimes.Contains(target);
    }
}