using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime;
using DG.Tweening;
using Zenject;

namespace SlimeBounce.Environment
{
    public class DeployableContainer : MonoBehaviour, IDeploymentActor
    {
        [SerializeField] private Transform _containerCore;
        [SerializeField] private float _containerMoveDuration;
        [SerializeField] private CanvasIndicator _containerIndicator;

        private Queue<DeployableSlime> _heldSlimes;
        private DeployableSlime _activeSlime;
        private Vector3 _activeContainerPosition;
        private Vector3 _inactiveContainerPosition;
        private Tween _containerMoveTween;

        [Inject]
        private SlimeCore.Factory _slimeFactory;
        [Inject]
        private ILevelStateProvider _levelState;

        void Awake()
        {
            _levelState.OnLevelEnded += OnLevelEnd;
            _heldSlimes = new Queue<DeployableSlime>();
        }

        private void Start()
        {
            _inactiveContainerPosition = _containerCore.position;
            _activeContainerPosition = _containerIndicator.GetScenePosition();
        }

        private void OnDestroy()
        {
            _levelState.OnLevelEnded -= OnLevelEnd;
        }

        private void OnLevelEnd(bool isWin)
        {
            EmptyContainer();
        }

        private void EmptyContainer()
        {
            if (_activeSlime != null)
            {
                _activeSlime.Despawn();
            }
            while (_heldSlimes.Count > 0)
            {
                var nextSlime = _heldSlimes.Dequeue();
                nextSlime.Despawn();
            }
        }

        private void MoveContainer()
        {
            if (_containerMoveTween != null)
            {
                _containerMoveTween.Kill();
            }
            Vector3 nextPosition = (_activeSlime != null ? _activeContainerPosition : _inactiveContainerPosition);
            _containerMoveTween = _containerCore.DOMove(nextPosition, _containerMoveDuration);
        }

        private void OnActiveSlimeTaken()
        {
            if (_activeSlime != null)
            {
                _activeSlime.OnSlimePicked -= OnActiveSlimeTaken;
                _activeSlime.OnSlimeDestroyed -= OnActiveSlimeTaken;
                _activeSlime.Unsuspend(_containerCore);
                _activeSlime = null;
            }
            EnableQueuedSlime();
        }

        private void EnableQueuedSlime()
        {
            if (_heldSlimes.Count > 0)
            {
                ActivateSlime(_heldSlimes.Dequeue());
            }
            MoveContainer();
        }

        private void ActivateSlime(DeployableSlime slime)
        {
            _activeSlime = slime;
            _activeSlime.gameObject.SetActive(true);
            _activeSlime.OnSlimePicked += OnActiveSlimeTaken;
            _activeSlime.OnSlimeDestroyed += OnActiveSlimeTaken;
        }

        private DeployableSlime CreateSlime(DeployableSlime prefab)
        {
            var newSlime = (DeployableSlime)_slimeFactory.Create(prefab);
            newSlime.transform.SetParent(_containerCore, false);
            newSlime.gameObject.SetActive(false);
            _heldSlimes.Enqueue(newSlime);
            EnableQueuedSlime();
            _activeSlime.Suspend(_containerCore);
            return newSlime;
        }

        public DeployableSlime DeploySlime(DeployableSlime prefab)
        {
            return CreateSlime(prefab);
        }
    }
}