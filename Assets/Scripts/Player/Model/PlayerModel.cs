using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Save;

namespace SlimeBounce.Player.Model
{
    public class PlayerModel
    {
        private const string PLAYER_STATE_NAME = "player";
        private PlayerState _state;
        private bool _isDirty;

        public int Currency 
        { 
            get 
            {
                return _state.Currency;
            } 
            set
            {
                _state.Currency = value;
                SaveModel();
            }
        }

        public int CurrentLevel
        {
            get
            {
                return _state.CurrentLevel;
            }
            set
            {
                _state.CurrentLevel = value;
                SaveModel();
            }
        }

        public int PassedLevels
        {
            get
            {
                return _state.PassedLevels;
            }
            set
            {
                _state.PassedLevels = value;
                SaveModel();
            }
        }

        public int PlayerLevel
        {
            get
            {
                return _state.PlayerLevel;
            }
            set
            {
                _state.PlayerLevel = value;
                SaveModel();
            }
        }

        public float PlayerExperience
        {
            get
            {
                return _state.PlayerExperience;
            }
            set
            {
                _state.PlayerExperience = value;
                SaveModel();
            }
        }

        public PlayerModel()
        {
            Initialize();
        }

        ~PlayerModel()
        {
            SaveManager.OnSavesCleared -= LoadModel;
        }


        private void Initialize()
        {
            if (_state == null)
            {
                LoadModel();
                SaveManager.OnSavesCleared += LoadModel;
            }
        }


        private void LoadModel()
        {
            _state = SaveManager.Load<PlayerState>(PLAYER_STATE_NAME);
            if (_state == null)
            {
                _state = new PlayerState();
            }
        }

        private void SaveModel()
        {
            _isDirty = true;
        }

        public void ProcessSave()
        {
            if (_isDirty)
            {
                SaveManager.Save(PLAYER_STATE_NAME, _state);
                _isDirty = false;
            }
        }




        public int GetUpgradeData(int upgradeIndex)
        {
            if (_state.UpgradeLevels.ContainsKey(upgradeIndex))
            {
                return _state.UpgradeLevels[upgradeIndex];
            } else
            {
                return 0;
            }
        }

        public void SetUpgradeData(int upgradeIndex, int upgradeLevel)
        {
            if (_state.UpgradeLevels.ContainsKey(upgradeIndex))
            {
                _state.UpgradeLevels[upgradeIndex] = upgradeLevel;
            } else
            {
                _state.UpgradeLevels.Add(upgradeIndex, upgradeLevel);
            }
        }
    }
}