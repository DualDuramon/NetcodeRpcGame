using UnityEngine;

namespace LittleSword.Player.Controller
{
    public class AnimationController
    {
        private readonly Animator animator;

        //애니메이션 파라미터 해시값
        private static readonly int hashIsRun = Animator.StringToHash("IsRun");
        private static readonly int hashAttack = Animator.StringToHash("Attack");
        private static readonly int hashDie = Animator.StringToHash("Die");
        private static readonly int hashHit = Animator.StringToHash("Hit");

        //생성자 (의존성 주입)
        public AnimationController(Animator animator)
        {
            this.animator = animator;
        }

        //애니메이션 상태변경 메서드
        public void Move(bool isMoving)
        {
            animator.SetBool(hashIsRun, isMoving);
        }
        public void Attack()
        {
            animator.SetTrigger(hashAttack);
        }
        public void Die()
        {
            animator.SetTrigger(hashDie);
        }
        public void Hit()
        {
            animator.SetTrigger(hashHit);
        }
    }
}