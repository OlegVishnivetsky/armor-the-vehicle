using System;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using UnityEngine;

namespace _Project._Code.Features.UI.Panels.Base
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Panel : MonoBehaviour
    {
        [SerializeField] private List<PanelItem> _panelItems;
        [SerializeField] private float _duration;
        [SerializeField] private float _appearDelayMultiplier;
        
        [Space(10f)]
        [SerializeField] private UIParticle _particle;
        [SerializeField] private float _delayBeforeParticle;

        private CanvasGroup _canvasGroup;

        protected virtual void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

        public virtual Tween Show()
        {
            _canvasGroup.blocksRaycasts = true;

            Sequence sequence = BuildSequence(1f, (item, delay) => item.Show(delay));

            if (_particle != null)
                sequence.InsertCallback(_delayBeforeParticle, () => _particle.Play());

            return sequence;
        }

        public virtual Tween Hide()
        {
            _canvasGroup.blocksRaycasts = false;
            return BuildSequence(0f, (item, delay) => item.Hide(delay));
        }

        private Sequence BuildSequence(float targetAlpha, Func<PanelItem, float, Tween> animate)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Join(_canvasGroup.DOFade(targetAlpha, _duration));

            for (int i = 0; i < _panelItems.Count; i++)
            {
                PanelItem item = _panelItems[i];
                float delay = i * _appearDelayMultiplier;
                sequence.Join(animate(item, delay));
            }

            return sequence;
        }
    }
}