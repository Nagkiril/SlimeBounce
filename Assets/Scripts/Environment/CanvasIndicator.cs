using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Environment
{
    public class CanvasIndicator : MonoBehaviour
    {
        [SerializeField] private Canvas _ownCanvas;
        [field: SerializeField] public bool IsLeftSided { get; private set; }
        const float INDICATOR_TO_ZONE_RANGE = 3; 


        public Vector3 GetScenePosition() => CameraController.Instance.OffsetPositionByDepth(transform.position, _ownCanvas.planeDistance + INDICATOR_TO_ZONE_RANGE);
    }
}
