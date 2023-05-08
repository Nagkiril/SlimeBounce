using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;

namespace SlimeBounce.Environment.DropoutComponents
{
    public class DropoutCanvasIndicator : MonoBehaviour
    {
        [SerializeField] Canvas ownCanvas;
        [field: SerializeField] public bool IsLeftSided { get; private set; }
        const float INDICATOR_TO_ZONE_RANGE = 3; 

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public Vector3 GetScenePosition() => CameraController.Instance.OffsetPositionByDepth(transform.position, ownCanvas.planeDistance + INDICATOR_TO_ZONE_RANGE);
    }
}
