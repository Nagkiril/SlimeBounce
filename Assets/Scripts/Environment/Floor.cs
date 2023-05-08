using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Environment.FloorModules;

namespace SlimeBounce.Environment
{
    public class Floor : MonoBehaviour
    {
        [SerializeField] BoxCollider box;
        [SerializeField] SpriteRenderer floorGlow;
        [SerializeField] Renderer floorBackgroundRenderer;
        [SerializeField] Renderer floorRenderer;
        [SerializeField] FloorColorSettings loseColors;
        [SerializeField] FloorColorSettings winColors;
        [SerializeField] FloorColorSettings levelColors;
        [SerializeField] WinScreen winScreen;
        [SerializeField] LoseScreen loseScreen;
        [SerializeField] LobbyScreen lobbyScreen;

        private static Floor _instance;
        public static Vector3 up => _instance.transform.up;

        [Serializable]
        private class FloorColorSettings
        {
            public Color BackgroundColor;
            public Color FloorColor;
            public Color GlowColor;
            public float BlendDuration;
        }

        Vector2 _floorXBounds;

        Tween _floorColoring;
        Tween _glowColoring;

        public static event Action OnLevelStartPressed;
        public static event Action OnShopPressed;
        public static event Action OnMenuPressed;
        public static event Action OnNextLevelPressed;
        public static event Action OnRetryPressed;

        private void Awake()
        {
            _instance = this;
            var floorHalfWidth = transform.localScale.x * box.size.x / 2f;
            _floorXBounds = new Vector2(transform.position.x - floorHalfWidth, transform.position.x + floorHalfWidth);


            LevelController.OnLevelEnded += OnLevelEnd;
            LevelController.OnLevelStarted += OnLevelStart;
            LevelController.OnLobbyEntered += OnLobbyEntry;

            //Bit of a crutch to allow animators preload, otherwise some screen animators (specifically winScreen) will show editor's state for 1 frame before actually using animator's state 
            winScreen.gameObject.SetActive(true);
            loseScreen.gameObject.SetActive(true);
            lobbyScreen.gameObject.SetActive(true);

            lobbyScreen.OnLevelStartPressed += OnLevelStartPress;
            lobbyScreen.OnShopPressed += OnShopPress;
            winScreen.OnMenuPressed += OnMenuPress;
            winScreen.OnNextLevelPressed += OnNextLevelPress;
            loseScreen.OnRetryPressed += OnRetryPress;
        }

        private void Start()
        {
            lobbyScreen.Show();
        }

        private void OnDestroy()
        {
            LevelController.OnLevelEnded -= OnLevelEnd;
            LevelController.OnLevelStarted -= OnLevelStart;
            LevelController.OnLobbyEntered -= OnLobbyEntry;
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
            ApplyColorSettings(levelColors);
            lobbyScreen.Show();
        }

        private void OnLevelStart()
        {
            //This is not necessary if floor should have same colors during lobby and active level
            ApplyColorSettings(levelColors);
        }


        private void OnLevelEnd(bool isWin)
        {
            ApplyColorSettings(( isWin ? winColors : loseColors ));
            ((FloorScreen)(isWin ? winScreen : loseScreen)).Show();
        }

        private void ApplyColorSettings(FloorColorSettings settings)
        {
            if (_floorColoring != null)
                _floorColoring.Kill();
            if (_glowColoring != null)
                _glowColoring.Kill();

            floorGlow.DOColor(settings.GlowColor, settings.BlendDuration);
            floorBackgroundRenderer.material.DOColor(settings.BackgroundColor, settings.BlendDuration);
            floorRenderer.material.DOColor(settings.FloorColor, settings.BlendDuration);
        }

        /// <summary>
        /// Projects a world position onto a floor's bounding box
        /// </summary>
        public static Vector3 GetFloorPosition(Vector3 position)
        {
            return _instance.box.ClosestPointOnBounds(position);
        }

        /// <summary>
        /// Validates provided world X coordinate: true if it exists within floor's bounds, false otherwise. This should run much faster than GetFloorPosition, but doesn't account for floor's rotation.
        /// </summary>
        public static bool ValidateOnWidth(float worldX)
        {
            return worldX >= _instance._floorXBounds.x && worldX < _instance._floorXBounds.y;
        }
    }
}