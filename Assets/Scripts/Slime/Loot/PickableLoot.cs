using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Clickable;
using SpecialEffects;
using DG.Tweening;
using SlimeBounce.Environment;
using Zenject;

namespace SlimeBounce.Slime.Loot
{
    public abstract class PickableLoot : MonoBehaviour
    {
        [SerializeField] private ClickableCollider _clickZone;
        [SerializeField] private FX _indicatorFX;
        [SerializeField] private FX _pickupFX;
        [SerializeField] private Animator _ownAnim;
        [SerializeField] private float _defaultDisperseDuration;
        [SerializeField] private float _defaultLifetime;
        private bool _isPicked;

        [Inject]
        protected ILevelStateProvider _levelState;


        private Tween _disperseTween;
        private Sequence _lifetimeSequence;

        private void Start()
        {

            _clickZone.OnClickEnded += Pickup;
            _levelState.OnLevelEnded += OnLevelEnd;
            if (!_levelState.IsLevelInProgress) 
            {
                Pickup();
            }
            if (!_isPicked)
            {
                _lifetimeSequence = DOTween.Sequence();
                _lifetimeSequence.AppendInterval(_defaultLifetime).AppendCallback(Hide);
                _ownAnim.SetBool("Show", true);
            }
        }

        private void OnDestroy()
        {
            _levelState.OnLevelEnded -= OnLevelEnd;
        }

        private void OnLevelEnd(bool isWin)
        {
            Pickup();
        }

        protected abstract void ApplyLootEffects();

        protected virtual void Hide()
        {
            _ownAnim.SetBool("Show", false);
            _indicatorFX.Hide();
            _clickZone.gameObject.SetActive(false);
            if (_lifetimeSequence != null)
            {
                _lifetimeSequence.Kill();
                _lifetimeSequence = null;
            }
            Destroy(gameObject, 2f);
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
                Instantiate(_pickupFX, transform.position, transform.rotation);
            }
        }

        public void DisperseToPosition(Vector3 targetPosition, float disperseDuration = -1f)
        {
            if (_disperseTween == null && !_isPicked)
            {
                _disperseTween = transform.DOMove(targetPosition, (disperseDuration <= 0f ? _defaultDisperseDuration : disperseDuration));
            }
        }

        public virtual bool CheckSpawnAllowed(LootEnvironment environment) => true;

        public class Factory : PrefabFactory<PickableLoot>
        {

        }
    }
}