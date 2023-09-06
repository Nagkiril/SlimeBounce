using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

namespace SlimeBounce.Slime.Visuals.Decals
{
    //We could totally abstract this into a feature that could be used in other games without much effort, however, I'm new to decals and not sure how much of it is actually optimal\useful
    public class SlimeDecal : MonoBehaviour
    {
        [SerializeField] private DecalProjector _projector;
        [SerializeField] private Material[] _randomizedMaterials;
        [SerializeField] private float _appearDuration;
        [SerializeField] private float _stayDuration;
        [SerializeField] private float _cleanDuration;
        [SerializeField] private Vector3 _startSize;
        [SerializeField] private Vector3 _fullSize;
        [SerializeField] private Vector3 _cleanSize;

        private Color _startColor;
        private Color _fullColor;

        private float _transitionProgress;

        private Sequence _decalSequence;

        private void Play()
        {
            _decalSequence = DOTween.Sequence();
            _decalSequence.Append(DOTween.To(() => _transitionProgress, x => SetAppearProgress(x), 1f, _appearDuration));
            _decalSequence.AppendInterval(_stayDuration);
            _decalSequence.Append(DOTween.To(() => _transitionProgress, x => SetCleanProgress(x), 0f, _cleanDuration));
            _decalSequence.AppendCallback(Remove);
        }

        private void Remove()
        {
            Destroy(gameObject);
        }

        private void SetAppearProgress(float newProgress)
        {
            SetStateProgress(newProgress);
            _projector.size = Vector3.Lerp(_startSize, _fullSize, newProgress);
            _projector.material.color = Color.Lerp(_startColor, _fullColor, newProgress);
        }

        private void SetCleanProgress(float newProgress)
        {
            SetStateProgress(newProgress);
            _projector.size = Vector3.Lerp(_cleanSize, _fullSize, newProgress);
        }

        private void SetStateProgress(float newProgress)
        {
            _transitionProgress = newProgress;
            _projector.fadeFactor = newProgress;
        }

        private void Randomize()
        {
            _projector.material = _randomizedMaterials[Random.Range(0, _randomizedMaterials.Length)];
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Random.Range(0, 360), transform.rotation.eulerAngles.z);
        }

        public void PlayColorized(Color startColor, Color fullColor)
        {
            _startColor = startColor;
            _fullColor = fullColor;
            Randomize();
            Play();
        }
    }
}