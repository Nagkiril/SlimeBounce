using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.UI.LifeComponents;
using SlimeBounce.Environment;

namespace SlimeBounce.UI
{
    public class LifePanel : MonoBehaviour
    {
        [SerializeField] LifeView lifePrefab;
        [SerializeField] Transform lifeContainer;

        List<LifeView> _lifeInstances;

        // Start is called before the first frame update
        void Awake()
        {
            LevelController.OnLevelStarted += ResetMaxLives;
            LevelController.OnLivesChanged += UpdateLivesContent;
        }

        private void OnDestroy()
        {
            LevelController.OnLevelStarted -= ResetMaxLives;
            LevelController.OnLivesChanged -= UpdateLivesContent;
        }

        void ResetMaxLives()
        {
            if (_lifeInstances != null)
            {
                foreach (var life in _lifeInstances)
                {
                    Destroy(life.gameObject);
                }
            }

            _lifeInstances = new List<LifeView>();
            for (var i = 0; i < LevelController.Lives; i++)
            {
                _lifeInstances.Add(Instantiate(lifePrefab, lifeContainer));
            }
            UpdateLivesContent();
        }

        void UpdateLivesContent()
        {
            if (_lifeInstances != null)
                for (var i = 0; i < _lifeInstances.Count; i++)
                {
                    _lifeInstances[i].SetFilled(i < LevelController.Lives);
                }
        }
    }
}