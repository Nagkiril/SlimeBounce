using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;
using SlimeBounce.UI.Count;
using SlimeBounce.Settings;

namespace SlimeBounce.Player
{
    public class ExpController : MonoBehaviour
    {
        static ExpController _instance;

        public static float LastExpProgress { get; private set; }

        public static event Action OnExpChanged;
        public static event Action OnLevelUp;

        // Start is called before the first frame update
        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                LastExpProgress = GetLevelProgress();
            } else
            {
                Debug.LogWarning($"There's at least two Exp Controllers, one of the controllers is in {gameObject.name}");
                Destroy(this);
            }
        }

        public static void AddExp(float value)
        {
            LastExpProgress = GetLevelProgress();
            PlayerData.PlayerExperience += value;

            var levelUpExp = _instance.GetLevelUpExp();

            if ((levelUpExp != -1) && PlayerData.PlayerExperience >= levelUpExp)
            {
                _instance.LevelUp();
            }


            OnExpChanged?.Invoke();
        }

        private void LevelUp()
        {
            PlayerData.PlayerExperience = 0;
            PlayerData.PlayerLevel++;

            OnLevelUp?.Invoke();
        } 

        private float GetLevelUpExp()
        {
            return PlayerLevelSettings.GetLevelUpExperience(PlayerData.PlayerLevel);
        }

        public static float GetLevelProgress()
        {
            return PlayerData.PlayerExperience / _instance.GetLevelUpExp();
        }
    }
}