using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Slime.Visuals.Face
{
    //We're making Slime Face not a subcategory of visual modules, because of the following:
    // 1) Every slime will have a face - and only one face, it is certain.
    // 2) Every face will likely behave exactly the same in the big picture, however I still leave room for extra implementations if specific slimes would need special faces.
    // 3) I would like to limit the scope of what faces can achieve, because they're supposed to realize a very well-defined, rigid feature.
    public abstract class SlimeFace : MonoBehaviour
    {
        protected FaceDataset _faceSet;
        protected FaceData _currentFace;

        //From my experience, caching main camera is nowadays pointless, but I'll do it because other devs seem to prefer this way (side note - we can also face different cameras then)
        //Ideally we could just inject Player's PoV, so that Faces could look at it without having to deal with camera at all
        private Camera _targetCamera;

        protected abstract void ApplyCurrentFaceData();

        protected abstract void FaceRotation(Quaternion rotation);

        protected virtual void FixedUpdate()
        {
            if (_targetCamera == null)
                _targetCamera = Camera.main;
            FaceRotation(Quaternion.LookRotation(_targetCamera.transform.position - transform.position));
        }
        

        public abstract void ApplyTint(Color tintColor);

        public abstract void Hide();

        public virtual void SetFaceData(FaceDataset data)
        {
            _faceSet = data;
        }

        public virtual void SetFaceType(FaceType targetFace)
        {
            var newFace = _faceSet.Faces[targetFace];
            if (newFace != _currentFace)
            {
                _currentFace = _faceSet.Faces[targetFace];
                ApplyCurrentFaceData();
            }
        }
    }

    public class FaceDataset
    {
        public Dictionary<FaceType, FaceData> Faces;

        public FaceDataset()
        {
            Faces = new Dictionary<FaceType, FaceData>();
        }
    }

    public class FaceData
    {
        public FaceEyesType EyesType;
        public FaceMouthType MouthType;
        public bool CanBlink;
    }

    public enum FaceType
    {
        Idle,
        Picked,
        Dead
    }

    public enum FaceEyesType
    {
        Regular,
        Tired,
        Angry,
        Happy,
        Dead
    }

    public enum FaceMouthType
    {
        Regular,
        Scared,
        Smiling,
        Distressed
    }
}