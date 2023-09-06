using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings;
using SlimeBounce.Slime.Visuals.Face;
using SlimeBounce.Slime.Visuals.Face.Implementations.Sprites;

namespace SlimeBounce.Slime.Visuals.Face.Settings
{
    public interface IFaceSpriteSettings
    {
        public FaceFeatureSet GetFeatureSet(FaceEyesType eyeType, FaceMouthType mouthType);
    }

    [Serializable]
    public class FaceFeatureSet
    {
        public FaceSpritePiece[] EyePrefabs;
        public FaceSpritePiece MouthPrefab;
    }
}