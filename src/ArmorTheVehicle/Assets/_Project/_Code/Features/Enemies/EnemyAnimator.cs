using UnityEngine;

namespace _Project._Code.Features.Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        public void SetRunning(bool running) => _animator.SetBool(IsRunning, running);
    }
}