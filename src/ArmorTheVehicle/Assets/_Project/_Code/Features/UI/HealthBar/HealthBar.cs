using System;
using _Project._Code.Features.Combat;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Project._Code.Features.UI.HealthBar
{
    [RequireComponent(typeof(CanvasGroup))]
    public class HealthBar : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Image _fill;

        [Header("Reveal / Hide")]
        [SerializeField] private float _revealDuration = 0.25f;
        [SerializeField] private float _hideDuration = 0.35f;
        [SerializeField] private float _visibleDuration = 1.5f;

        [Header("Fill")]
        [SerializeField] private float _fillDuration = 0.25f;

        [Header("Damage Reaction")]
        [SerializeField] private float _shakeDuration = 0.4f;
        [SerializeField] private float _shakeAngle = 25f;
        [SerializeField] private float _punchScale = 0.25f;

        private RectTransform _root;
        private CanvasGroup _canvasGroup;

        private IDisposable _subscription;
        private Tween _hideTween;

        private float _previous;
        private bool _initialized;
        private bool _visible;

        private void Awake()
        {
            _root = (RectTransform)transform;
            _canvasGroup = GetComponent<CanvasGroup>();

            _canvasGroup.alpha = 0f;
            _root.localScale = Vector3.zero;
        }

        public void Bind(Health health)
        {
            _subscription?.Dispose();
            _initialized = false;

            _subscription = health.Current.Subscribe(current =>
            {
                float max = health.Max <= 0f ? 1f : health.Max;
                float ratio = Mathf.Clamp01(current / max);

                if (!_initialized)
                {
                    _fill.fillAmount = ratio;
                    _previous = current;
                    _initialized = true;
                    return;
                }

                _fill.DOKill();
                _fill.DOFillAmount(ratio, _fillDuration).SetEase(Ease.OutQuad);

                if (current < _previous)
                    PlayDamage();

                _previous = current;
            });
        }

        public void Reveal()
        {
            Show();
            ScheduleHide();
        }

        private void PlayDamage()
        {
            if (!_visible)
                Show();

            _root.DOComplete();

            _root.DOShakeRotation(
                _shakeDuration,
                new Vector3(0f, 0f, _shakeAngle),
                vibrato: 12, randomness: 90f);
            
            _root.DOPunchScale(Vector3.one * _punchScale, _shakeDuration, vibrato: 8, elasticity: 0.6f);
            
            ScheduleHide();
        }

        private void Show()
        {
            if (_visible)
                return;

            _visible = true;
            _hideTween?.Kill();
            
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(1f, _revealDuration);
            
            _root.localRotation = Quaternion.identity;
            _root.DOScale(1f, _revealDuration).SetEase(Ease.OutBack);
        }

        private void ScheduleHide()
        {
            _hideTween?.Kill();
            _hideTween = DOVirtual.DelayedCall(_visibleDuration, Hide);
        }

        private void Hide()
        {
            _visible = false;
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0f, _hideDuration);
            _root.DOScale(0.7f, _hideDuration).SetEase(Ease.InQuad);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
            _hideTween?.Kill();
            _root.DOKill();
            _canvasGroup.DOKill();
        }
    }
}