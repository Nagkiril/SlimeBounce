using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialEffects
{
    public class FX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _controlledParticles;
        [SerializeField] private Animator _controlAnim;
        [SerializeField] private float _autoExpiration;
        [SerializeField] private float _autoDestruction;

        private float _expirationTimer;
        private float _destructionTimer;

        public event Action<FX> OnDestruction;

        private void Start()
        {
            _expirationTimer = _autoExpiration;
        }

        private void FixedUpdate()
        {
            if (_expirationTimer > 0)
            {
                _expirationTimer -= Time.deltaTime;
                if (_expirationTimer <= 0)
                {
                    Hide();
                }
            }
            if (_destructionTimer > 0)
            {
                _destructionTimer -= Time.deltaTime;
                if (_destructionTimer <= 0)
                {
                    OnDestruction?.Invoke(this);
                    Destroy(gameObject);
                }
            }
        }

        public void Hide()
        {
            _destructionTimer = _autoDestruction;
            foreach (var particle in _controlledParticles)
            {
                if (particle != null)
                    particle.Stop();
            }
            if (_controlAnim != null)
                _controlAnim.SetBool("Show", false);
        }

        public void Show()
        {
            foreach (var particle in _controlledParticles)
            {
                particle.Play();
            }
            if (_controlAnim != null)
                _controlAnim.SetBool("Show", true);
        }

        public void ApplyEvent(string eventName)
        {
            //This can be expanded to support a whole lot of event handling - not just animator triggers - via interfaces and bonus components; I just decide to keep it simple because that's more than enough for now
            //We can (and perhaps should) replace eventName with an enum; and convert it to trigger hash later; we can do it seamlessly by adding another signature for ApplyEvent; this only makes sense if we'll expand Fx events later
            if (_controlAnim != null)
            {
                _controlAnim.SetTrigger(eventName);
            }
        }
    }
}