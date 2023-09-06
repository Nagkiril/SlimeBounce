using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.UI.LifeComponents;
using SlimeBounce.Environment;
using Zenject;

namespace SlimeBounce.UI
{
    public class LifePanel : MonoBehaviour
    {
        [SerializeField] private LifeView _lifePrefab;
        [SerializeField] private Transform _lifeContainer;

        private List<LifeView> _lifeInstances;
        [Inject]
        private ILevelStateProvider _levelState;
        [Inject]
        private ILivesStateProvider _livesState;


        private void Awake()
        {
            _levelState.OnLevelStarted += ResetMaxLives;
            _livesState.OnLivesChanged += UpdateLivesContent;
        }

        private void OnDestroy()
        {
            _levelState.OnLevelStarted -= ResetMaxLives;
            _livesState.OnLivesChanged -= UpdateLivesContent;
        }

        private void ResetMaxLives()
        {
            if (_lifeInstances != null)
            {
                foreach (var life in _lifeInstances)
                {
                    Destroy(life.gameObject);
                }
            }

            _lifeInstances = new List<LifeView>();
            for (var i = 0; i < _livesState.Lives; i++)
            {
                _lifeInstances.Add(Instantiate(_lifePrefab, _lifeContainer));
            }
            UpdateLivesContent();
        }

        private void UpdateLivesContent()
        {
            if (_lifeInstances != null)
            {
                for (var i = 0; i < _lifeInstances.Count; i++)
                {
                    _lifeInstances[i].SetFilled(i < _livesState.Lives);
                }
            }
        }
    }
}