using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Environment;

namespace SlimeBounce.Slime.Control.Indicator
{
    public class SlimeIndicator : MonoBehaviour
    {
        [SerializeField] SlimeIndicatorVisual indicatorVisual;
        //There is reasoning that _floorMask should be put somewhere in the settings, and calculated out of there.
        private static int _floorMask = -1;
        bool _isShown;
        bool _isImpossibleLocation;
        Vector3 _floorLocation;

        // Start is called before the first frame update
        void Start()
        {
            if (_floorMask == -1)
            {
                _floorMask = LayerMask.GetMask("Floor");
            }
        }

        void FixedUpdate()
        {
            if (_isShown)
            {
                UpdateIndicatorLocation();
            }
        }

        private void UpdateIndicatorLocation()
        {
            Ray rayToGround = new Ray(transform.position, (transform.position - CameraController.Instance.CamPosition).normalized);
            if (Physics.Raycast(rayToGround, out RaycastHit hitResult, 90f, _floorMask))
            {
                indicatorVisual.MarkImpossibleLocation(false);
                _floorLocation = hitResult.point;
            }
            else
            {
                indicatorVisual.MarkImpossibleLocation(true);
                //Considering we're using "GetFloorPosition" twice, we might end up with some expensive operations.
                //Is it even worth the math, rather than just Raycast(onto a collider-wall near the floor)-GetFloorPosition()?
                var closestFloorPosition = Floor.GetFloorPosition(transform.position);
                var throwAngle = Vector3.Angle(closestFloorPosition - transform.position, transform.position - CameraController.Instance.CamPosition);
                var travelLength = Mathf.Tan(throwAngle * Mathf.Deg2Rad) * (closestFloorPosition - transform.position).magnitude;
                var travelDirection = Vector3.ProjectOnPlane(transform.position - CameraController.Instance.CamPosition, Floor.up).normalized;
                _floorLocation = Floor.GetFloorPosition(closestFloorPosition + travelDirection * travelLength);
            }
            indicatorVisual.MoveTo(_floorLocation);
        }

        public void Show()
        {
            _isShown = true;
            indicatorVisual.Show();
        }

        public void Hide()
        {
            UpdateIndicatorLocation();
            _isShown = false;
            indicatorVisual.Hide();
        }

        public void SetVisualProhibition(bool isProhibited)
        {
            indicatorVisual.SetVisualProhibition(isProhibited);
        }

        public Vector3 GetIndicatorPosition()
        {
            UpdateIndicatorLocation();
            return _floorLocation;
        }
    }
}