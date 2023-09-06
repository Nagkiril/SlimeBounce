using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player.Model;
using SlimeBounce.Player.Settings;

namespace SlimeBounce.Player
{
    public class PlayerData : MonoBehaviour
    {
        private static PlayerModel _loadedModel;
        private static PlayerModel _model
        {
            get 
            {
                if (_loadedModel == null)
                    _loadedModel = new PlayerModel();
                return _loadedModel;
            }
        }

        public static int Currency
        {
            get => _model.Currency;
            set => _model.Currency = value;
        }

        public static int CurrentLevel
        {
            get => _model.CurrentLevel;
            set => _model.CurrentLevel = value;
        }

        public static int PassedLevels
        {
            get => _model.PassedLevels;
            set => _model.PassedLevels = value;
        }

        public static int PlayerLevel
        {
            get => _model.PlayerLevel;
            set => _model.PlayerLevel = value;
        }

        public static float PlayerExperience
        {
            get => _model.PlayerExperience;
            set => _model.PlayerExperience = value;
        }


        private void Update()
        {
            _model.ProcessSave();
        }

        public static void SetUpgradeLevel(UpgradeType type, int level)
        {
            _model.SetUpgradeData((int)type, level);
        }

        public static int GetUpgradeLevel(UpgradeType type)
        {
            return _model.GetUpgradeData((int)type);
        }
    }
}