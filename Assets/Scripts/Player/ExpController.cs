using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;
using SlimeBounce.Player.Settings;
using SlimeBounce.UI.Count;
using SlimeBounce.Settings;
using Zenject;

namespace SlimeBounce.Player
{
    public class ExpController : MonoBehaviour, IPlayerExpManager
    {
        [Inject]
        private IPlayerLevelSettings _playerLevelSettings;

        public float LastExpProgress { get; private set; }
        public event Action OnExpChanged;
        public event Action OnLevelUp;


        private void LevelUp()
        {
            PlayerData.PlayerExperience = 0;
            PlayerData.PlayerLevel++;

            OnLevelUp?.Invoke();
        } 

        private float GetLevelUpExp()
        {
            return _playerLevelSettings.GetLevelUpExperience(PlayerData.PlayerLevel);
        }

        public float GetLevelProgress()
        {
            return PlayerData.PlayerExperience / GetLevelUpExp();
        }

        public void AddExp(float value)
        {
            LastExpProgress = GetLevelProgress();
            PlayerData.PlayerExperience += value;

            var levelUpExp = GetLevelUpExp();

            if ((levelUpExp != -1) && PlayerData.PlayerExperience >= levelUpExp)
            {
                LevelUp();
            }


            OnExpChanged?.Invoke();
        }
    }
}