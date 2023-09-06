using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Visuals.Face.Settings;
using Zenject;


namespace SlimeBounce.Slime.Visuals.Face.Implementations.Sprites
{
    public class SpriteSlimeFace : SlimeFace
    {
        [SerializeField] private Transform _pieceContainer;
        private List<FaceSpritePiece> _facePieces;
        private Color _faceTint;

        [Inject]
        private IFaceSpriteSettings _spriteSettings;

        private void ClearFace()
        {
            if (_facePieces != null)
            {
                foreach (var piece in _facePieces)
                {
                    piece.Dispose();
                }
            }
            _facePieces = new List<FaceSpritePiece>();
        }

        private void CreateCurrentFace()
        {
            //We can at least cache this for each face\apply Flyweight pattern if forming a set of features would become difficult in real time
            //There's also a lot of other options like pre computing these connections or even instantiating them in advance\pooling and more, however there's little reason in optimizing something that doesn't stand an issue
            var prefabSet = _spriteSettings.GetFeatureSet(_currentFace.EyesType, _currentFace.MouthType);

            if (prefabSet.MouthPrefab != null)
            {
                AddPiece(prefabSet.MouthPrefab);
            }

            //For simplicity, if Slime has a pair of same eyes (overwhelmingly common case), we need a reference to just one; if that's not the case, we'll follow the feature set to the letter
            if (prefabSet.EyePrefabs.Length == 1)
            {
                AddPiece(prefabSet.EyePrefabs[0]);
                AddPiece(prefabSet.EyePrefabs[0]).Mirror();
            }
            else
            {
                foreach (var eyePrefab in prefabSet.EyePrefabs)
                {
                    AddPiece(eyePrefab).Mirror();
                }
            }
        }

        private FaceSpritePiece AddPiece(FaceSpritePiece piecePrefab)
        {
            var newPiece = Instantiate(piecePrefab, _pieceContainer);
            newPiece.Tint(_faceTint);
            if (_currentFace.CanBlink && newPiece is IBlinkHandler blinkHandler)
                blinkHandler.AllowBlinking();
            _facePieces.Add(newPiece);
            return newPiece;
        }

        protected override void ApplyCurrentFaceData()
        {
            ClearFace();
            CreateCurrentFace();
        }

        protected override void FaceRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public override void Hide()
        {
            ClearFace();
        }

        public override void ApplyTint(Color tintColor)
        {
            _faceTint = tintColor;
            if (_facePieces != null)
            {
                foreach (var piece in _facePieces)
                {
                    piece.Tint(tintColor);
                }
            }
        }
    }
}