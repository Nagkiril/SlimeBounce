using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;
using SlimeBounce.Slime.Visuals.Face;
using SlimeBounce.Slime.Visuals.Face.Settings;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "FaceSlimeSettings", menuName = "SlimeBounce/New Face Slime Settings", order = 10)]
    public class FaceSlimeScriptableSettings : ScriptableObject, IFaceSlimeSettings
    {
        [SerializeField] private SlimeFaceSet[] _faces;

        [Serializable]
        private class SlimeFaceSet
        {
            public SlimeType Slime;
            public SlimeFaceTypes[] Faces;
        }

        [Serializable]
        private class SlimeFaceTypes
        {
            public FaceType FaceType;
            public EyesTypes Eyes;
            public MouthTypes Mouths;
        }

        [Serializable]
        private class MouthTypes
        {
            public FaceMouthType[] AvailableTypes;
        }

        [Serializable]
        private class EyesTypes
        {
            public FaceEyesType[] AvailableTypes;
        }

        private FaceDataset RandomizeDataset(SlimeFaceSet faceSet)
        {
            var newDataset = new FaceDataset();

            if (faceSet != null)
            {
                foreach (var face in faceSet.Faces)
                {
                    var newFace = new FaceData();
                    newFace.EyesType = face.Eyes.AvailableTypes[UnityEngine.Random.Range(0, face.Eyes.AvailableTypes.Length)];
                    newFace.MouthType = face.Mouths.AvailableTypes[UnityEngine.Random.Range(0, face.Mouths.AvailableTypes.Length)];
                    //This can be wrapped into a method, but if we're making slimes that can play dead, maybe those can still blink
                    newFace.CanBlink = newFace.EyesType != FaceEyesType.Dead;
                    newDataset.Faces.Add(face.FaceType, newFace);
                }
            }

            return newDataset;
        }

        public FaceDataset GetFaceData(SlimeType slimeType)
        {
            SlimeFaceSet faceSet = null;
            foreach (var face in _faces)
            {
                if (face.Slime == slimeType)
                {
                    faceSet = face;
                    break;
                }
            }
            return RandomizeDataset(faceSet);
        }
    }
}