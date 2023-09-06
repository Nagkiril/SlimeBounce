using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings;

namespace SlimeBounce.Slime.Visuals.Face.Settings
{
    public interface IFaceSlimeSettings
    {
        public FaceDataset GetFaceData(SlimeType slimeType);
    }
}