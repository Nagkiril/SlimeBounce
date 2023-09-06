using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Environment;
using Zenject;

namespace SlimeBounce.Slime.Control.Indicator
{
    public class SlimeIndicator : MonoBehaviour
    {
        [SerializeField] private SlimeIndicatorVisual _indicatorVisual;
        //There is reasoning that _floorMask should be put somewhere in the settings, and calculated out of there.
        private static int _floorMask = -1;
        private bool _isShown;
        private bool _isImpossibleLocation;
        private Vector3 _floorLocation;
        [Inject]
        IFloorBoundsProvider _floorBounds;

        private void Start()
        {
            if (_floorMask == -1)
            {
                _floorMask = LayerMask.GetMask("Floor");
            }
        }

        private void FixedUpdate()
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
                _indicatorVisual.MarkImpossibleLocation(false);
                _floorLocation = hitResult.point;
            }
            else
            {
                _indicatorVisual.MarkImpossibleLocation(true);
                //Considering we're using "GetFloorPosition" twice, we might end up with some expensive operations.
                //Is it even worth the math, rather than just Raycast(onto a collider-wall near the floor)-GetFloorPosition()?
                var closestFloorPosition = _floorBounds.GetFloorPosition(transform.position);
                var throwAngle = Vector3.Angle(closestFloorPosition - transform.position, transform.position - CameraController.Instance.CamPosition);
                var travelLength = Mathf.Tan(throwAngle * Mathf.Deg2Rad) * (closestFloorPosition - transform.position).magnitude;
                var travelDirection = Vector3.ProjectOnPlane(transform.position - CameraController.Instance.CamPosition, _floorBounds.Up).normalized;
                _floorLocation = _floorBounds.GetFloorPosition(closestFloorPosition + travelDirection * travelLength);
            }
            _indicatorVisual.MoveTo(_floorLocation);
        }

        public void Show()
        {
            _isShown = true;
            _indicatorVisual.Show();
        }

        public void Hide()
        {
            UpdateIndicatorLocation();
            _isShown = false;
            _indicatorVisual.Hide();
        }

        public void SetVisualProhibition(bool isProhibited)
        {
            _indicatorVisual.SetVisualProhibition(isProhibited);
        }

        public Vector3 GetIndicatorPosition()
        {
            UpdateIndicatorLocation();
            return _floorLocation;
        }
    }
}