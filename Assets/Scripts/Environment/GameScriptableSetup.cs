using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment.Settings;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;

namespace SlimeBounce.Environment
{
    //Probably will end up being reusable
    [CreateAssetMenu(fileName = "GameScriptableSetup", menuName = "SlimeBounce/New Game Scriptable Setup", order = 10)]
    public class GameScriptableSetup : ScriptableObject, IGameSetupHandler
    {
        [SerializeField] private int _targetFramerate;


        public void RunSetup()
        {
            Application.targetFrameRate = _targetFramerate;
        }
    }
}