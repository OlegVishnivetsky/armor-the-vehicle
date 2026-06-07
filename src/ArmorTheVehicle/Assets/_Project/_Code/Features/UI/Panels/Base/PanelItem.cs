using DG.Tweening;
using UnityEngine;

namespace _Project._Code.Features.UI.Panels.Base
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelItem : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private bool _hideOnStart = true;

        [Header("Scale Appear")] 
        [SerializeField] private AnimationCurve _scaleEase;
        
        [Header("Fade Appear")] 
        [SerializeField] private AnimationCurve _fadeEase;

        private CanvasGroup _canvasGroup;

        private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

        private void Start()
        {
            if (!_hideOnStart)
                return;
            
            HideImmediate();
        }

        public Tween Show(float delay = 0) => 
            DoPanelAnimation(1f, 1f, delay, true);

        public Tween Hide(float delay = 0) => 
            DoPanelAnimation(0f, 0f, delay, false);

        public void HideImmediate()
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0f;
            transform.localScale = Vector3.zero;
        }

        private Tween DoPanelAnimation(float scaleEndValue, float alphaEndValue,
            float delay, bool blockRaycasts)
        {
            _canvasGroup.blocksRaycasts = blockRaycasts;

            return DOTween.Sequence()
                .Join(transform.DOScale(scaleEndValue, _duration).SetDelay(delay).SetEase(_scaleEase))
                .Join(_canvasGroup.DOFade(alphaEndValue, _duration).SetDelay(delay).SetEase(_fadeEase));
        }
    }
}