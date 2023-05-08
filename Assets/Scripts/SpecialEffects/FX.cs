using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialEffects
{
    public class FX : MonoBehaviour
    {
        [SerializeField] ParticleSystem[] controlledParticles;
        [SerializeField] Animator controlAnim;
        [SerializeField] float autoExpiration;
        [SerializeField] float autoDestruction;

        float _expirationTimer;
        float _destructionTimer;

        private void Start()
        {
            _expirationTimer = autoExpiration;
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
                    Destroy(gameObject);
                }
            }
        }

        public void Hide()
        {
            _destructionTimer = autoDestruction;
            foreach (var particle in controlledParticles)
            {
                if (particle != null)
                    particle.Stop();
            }
            if (controlAnim != null)
                controlAnim.SetBool("Show", false);
        }

        public void Show()
        {
            foreach (var particle in controlledParticles)
            {
                particle.Play();
            }
            if (controlAnim != null)
                controlAnim.SetBool("Show", true);
        }
    }
}