using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Environment.FloorModules;
using Zenject;

namespace SlimeBounce.Environment
{
    public class Floor : MonoBehaviour, IFloorBoundsProvider, IFloorScreenEvents 
    {
        //A lot of this can either be refactored into different components (and should, really), despite the script not being awfully large
        [SerializeField] private BoxCollider _box;
        [SerializeField] private SpriteRenderer _floorGlow;
        [SerializeField] private Renderer _floorBackgroundRenderer;
        [SerializeField] private Renderer _floorRenderer;
        [SerializeField] private FloorColorSettings _loseColors;
        [SerializeField] private FloorColorSettings _winColors;
        [SerializeField] private FloorColorSettings _levelColors;
        [SerializeField] private WinScreen _winScreen;
        [SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private LobbyScreen _lobbyScreen;

        private Vector2 _floorXBounds;
        private Tween _floorColoring;
        private Tween _glowColoring;
        [Inject]
        private ILevelStateProvider _levelState;

        [Serializable]
        private class FloorColorSettings
        {
            public Color BackgroundColor;
            public Color FloorColor;
            public Color GlowColor;
            public float BlendDuration;
        }


        public Vector3 Up => transform.up;
        public event Action OnLevelStartPressed;
        public event Action OnShopPressed;
        public event Action OnMenuPressed;
        public event Action OnNextLevelPressed;
        public event Action OnRetryPressed;

        private void Awake()
        {
            var floorHalfWidth = transform.localScale.x * _box.size.x / 2f;
            _floorXBounds = new Vector2(transform.position.x - floorHalfWidth, transform.position.x + floorHalfWidth);


            _levelState.OnLevelEnded += OnLevelEnd;
            _levelState.OnLevelStarted += OnLevelStart;
            _levelState.OnLobbyEntered += OnLobbyEntry;

            //Bit of a crutch to allow animators preload, otherwise some screen animators (specifically winScreen) will show editor's state for 1 frame before actually using animator's state 
            _winScreen.gameObject.SetActive(true);
            _loseScreen.gameObject.SetActive(true);
            _lobbyScreen.gameObject.SetActive(true);

            _lobbyScreen.OnLevelStartPressed += OnLevelStartPress;
            _lobbyScreen.OnShopPressed += OnShopPress;
            _winScreen.OnMenuPressed += OnMenuPress;
            _winScreen.OnNextLevelPressed += OnNextLevelPress;
            _loseScreen.OnRetryPressed += OnRetryPress;
            _loseScreen.OnMenuPressed += OnMenuPress;
        }

        private void Start()
        {
            _lobbyScreen.Show();
        }

        private void OnDestroy()
        {
            _levelState.OnLevelEnded -= OnLevelEnd;
            _levelState.OnLevelStarted -= OnLevelStart;
            _levelState.OnLobbyEntered -= OnLobbyEntry;
        }

        private void OnLevelStartPress()
        {
            OnLevelStartPressed?.Invoke();
        }

        private void OnShopPress()
        {
            OnShopPressed?.Invoke();
        }

        private void OnNextLevelPress()
        {
            OnNextLevelPressed?.Invoke();
        }

        private void OnMenuPress()
        {
            OnMenuPressed?.Invoke();
        }

        private void OnRetryPress()
        {
            OnRetryPressed?.Invoke();
        }

        private void OnLobbyEntry()
        {
            ApplyColorSettings(_levelColors);
            _lobbyScreen.Show();
        }

        private void OnLevelStart()
        {
            //This is not necessary if floor should have same colors during lobby and active level
            ApplyColorSettings(_levelColors);
        }


        private void OnLevelEnd(bool isWin)
        {
            ApplyColorSettings(( isWin ? _winColors : _loseColors ));
            ((FloorScreen)(isWin ? _winScreen : _loseScreen)).Show();
        }

        private void ApplyColorSettings(FloorColorSettings settings)
        {
            if (_floorColoring != null)
                _floorColoring.Kill();
            if (_glowColoring != null)
                _glowColoring.Kill();

            _floorGlow.DOColor(settings.GlowColor, settings.BlendDuration);
            _floorBackgroundRenderer.material.DOColor(settings.BackgroundColor, settings.BlendDuration);
            _floorRenderer.material.DOColor(settings.FloorColor, settings.BlendDuration);
        }

        /// <summary>
        /// Projects a world position onto a floor's bounding box
        /// </summary>
        public Vector3 GetFloorPosition(Vector3 position)
        {
            return _box.ClosestPointOnBounds(position);
        }

        /// <summary>
        /// Validates provided world X coordinate: true if it exists within floor's bounds, false otherwise. This should run much faster than GetFloorPosition, but doesn't account for floor's rotation.
        /// </summary>
        public bool ValidateOnWidth(float worldX)
        {
            return worldX >= _floorXBounds.x && worldX < _floorXBounds.y;
        }
    }
}