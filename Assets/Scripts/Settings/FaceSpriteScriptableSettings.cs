using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime.Visuals.Face;
using SlimeBounce.Slime.Visuals.Face.Settings;
using SlimeBounce.Slime.Visuals.Face.Implementations.Sprites;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "FaceSpriteScriptableSettings", menuName = "SlimeBounce/New Face Sprite Settings", order = 10)]
    public class FaceSpriteScriptableSettings : ScriptableObject, IFaceSpriteSettings
    {
        [SerializeField] private EyeSet[] _eyes;
        [SerializeField] private MouthSet[] _mouths;

        [Serializable]
        private class EyeSet
        {
            public FaceEyesType Type;
            public FaceSpritePiece[] Prefabs;
        }

        [Serializable]
        private class MouthSet
        {
            public FaceMouthType Type;
            public FaceSpritePiece Prefab;
        }

        public FaceFeatureSet GetFeatureSet(FaceEyesType eyeType, FaceMouthType mouthType)
        {
            var newSet = new FaceFeatureSet();

            foreach (var mouth in _mouths)
            {
                if (mouth.Type == mouthType)
                {
                    newSet.MouthPrefab = mouth.Prefab;
                    break;
                }
            }

            foreach (var eyes in _eyes)
            {
                if (eyes.Type == eyeType)
                {
                    newSet.EyePrefabs = eyes.Prefabs;
                    break;
                }
            }


            return newSet;
        }
    }
}