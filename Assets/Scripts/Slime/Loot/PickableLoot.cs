using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Clickable;
using SpecialEffects;
using DG.Tweening;
using SlimeBounce.Environment;

namespace SlimeBounce.Slime.Loot
{
    public abstract class PickableLoot : MonoBehaviour
    {
        [SerializeField] ClickableCollider clickZone;
        [SerializeField] FX indicatorFX;
        [SerializeField] FX pickupFX;
        [SerializeField] Animator ownAnim;
        [SerializeField] float defaultDisperseDuration;
        [SerializeField] float defaultLifetime;

        bool _isPicked;

        Tween _disperseTween;
        Sequence _lifetimeSequence;

        // Start is called before the first frame update
        void Start()
        {

            clickZone.OnClickEnd += Pickup;
            LevelController.OnLevelEnded += OnLevelEnd;
            if (!LevelController.IsLevelInProgress) 
            {
                Pickup();
            }
            if (!_isPicked)
            {
                _lifetimeSequence = DOTween.Sequence();
                _lifetimeSequence.AppendInterval(defaultLifetime).AppendCallback(Hide);
                ownAnim.SetBool("Show", true);
            }
        }

        private void OnDestroy()
        {
            LevelController.OnLevelEnded -= OnLevelEnd;
        }

        void OnLevelEnd(bool isWin)
        {
            Pickup();
        }

        public void Pickup()
        {
            if (!_isPicked)
            {
                if (_disperseTween != null)
                {
                    _disperseTween.Kill();
                    _disperseTween = null;
                }
                _isPicked = true;
                ApplyLootEffects();
                Hide();
                Instantiate(pickupFX, transform.position, transform.rotation);
            }
        }

        public void DisperseToPosition(Vector3 targetPosition, float disperseDuration = -1f)
        {
            if (_disperseTween == null && !_isPicked)
            {
                _disperseTween = transform.DOMove(targetPosition, (disperseDuration <= 0f ? defaultDisperseDuration : disperseDuration));
            }
        }

        public virtual bool CheckSpawnAllowed() => true;

        protected abstract void ApplyLootEffects();

        protected virtual void Hide()
        {
            ownAnim.SetBool("Show", false);
            indicatorFX.Hide();
            clickZone.gameObject.SetActive(false);
            if (_lifetimeSequence != null)
            {
                _lifetimeSequence.Kill();
                _lifetimeSequence = null;
            }
            Destroy(gameObject, 2f);
        }
    }
}