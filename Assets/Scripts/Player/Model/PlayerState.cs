using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlimeBounce.Player.Model
{
    [Serializable]
    public class PlayerState
    {
        public int Currency;
        public int CurrentLevel;
        public int PassedLevels;
        public int PlayerLevel;
        public float PlayerExperience;

        public Dictionary<int, int> UpgradeLevels;


        public PlayerState()
        {
            UpgradeLevels = new Dictionary<int, int>();
        }
    }
}