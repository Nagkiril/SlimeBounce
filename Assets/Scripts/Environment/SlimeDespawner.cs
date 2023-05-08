using SlimeBounce.Slime.Control;
using System;
using UnityEngine;
using SlimeBounce.Slime;

namespace SlimeBounce.Environment
{
    public class SlimeDespawner : MonoBehaviour
    {
        public static SlimeDespawner Instance { get; private set; }
        
        public event Action<SlimeCore> OnSlimeReached;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } else
            {
                Debug.LogWarning("There should only be 1 Slime Despawner active at any given time! Check the scene setup or make sure none more are spawned.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var enteredCollision = other.GetComponent<SlimeCollision>();
            if (enteredCollision != null)
            {
                enteredCollision.Slime.Escape();
                OnSlimeReached?.Invoke(enteredCollision.Slime);
            }
        }
    }
}