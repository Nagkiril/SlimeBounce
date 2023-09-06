using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Slime.ExtraModules.Suspension
{
    public class SuspendedLifetime : MonoBehaviour
    {
        [SerializeField] SlimeCore targetSlime;
        [SerializeField] private float _suspendedLifetime;
        private float _untilDespawn = 0;

        private void Awake()
        {
            targetSlime.OnSlimeSuspended += OnSlimeSuspension;
        }

        private void OnDestroy()
        {
            targetSlime.OnSlimeSuspended -= OnSlimeSuspension;
        }

        private void OnSlimeSuspension(bool isSuspended)
        {
            if (isSuspended)
                ResetTimer();
            else
                StopTimer();
        }

        private void ResetTimer()
        {
            _untilDespawn = _suspendedLifetime;
        }

        private void StopTimer()
        {
            _untilDespawn = 0;
        }

        private void FixedUpdate()
        {
            if (_untilDespawn > 0)
            {
                _untilDespawn -= Time.deltaTime;
                if (_untilDespawn <= 0)
                {
                    targetSlime.Despawn();
                }
            }
        }
    }
}