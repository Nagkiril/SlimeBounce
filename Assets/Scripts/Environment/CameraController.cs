using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Environment
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _closePositionDepth;
        private Camera _ownCamera;

        public Vector3 CamPosition => transform.position;
        public static CameraController Instance { get; private set; }


        // Start is called before the first frame update
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("There should only be 1 Camera Controller active at any given time! Check the scene setup.");
            }
            _ownCamera = GetComponent<Camera>();
        }

        public Vector3 GetMouseClosePosition()
        {
            Vector3 targetPosition = Vector2.zero;
            if (Input.GetMouseButton(0))
            {
                targetPosition = Input.mousePosition;
            } else
            {
                targetPosition = new Vector2(Screen.width, Screen.height) / 2f;
            }
            targetPosition.z = _closePositionDepth;

            return _ownCamera.ScreenToWorldPoint(targetPosition);
        }

        public Vector3 OffsetPositionByDepth(Vector3 worldPosition, float newPositionDepth)
        {
            var screenPosition = _ownCamera.WorldToScreenPoint(worldPosition);
            screenPosition.z = newPositionDepth;
            return _ownCamera.ScreenToWorldPoint(screenPosition);
        }

        public Vector3 GetScreenFromWorld(Vector3 worldPosition) => _ownCamera.WorldToScreenPoint(worldPosition);
    }
}