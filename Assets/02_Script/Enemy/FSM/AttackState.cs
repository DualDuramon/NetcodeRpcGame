using LittleSword.Player;
using UnityEngine;
using Logger = LittleSword.Common.Logger;


namespace LittleSword.Enemy.FSM
{
    public class AttackState : IState
    {
        private readonly float attackCooldown;
        private float lastAttackTime;

        public AttackState(float attackCooldown = 1.0f)
        {
            this.attackCooldown = attackCooldown;
            lastAttackTime = Time.time - attackCooldown;
        }

        public void Enter(Enemy enemy)
        {
            Logger.Log("Attack 진입");
            enemy.animator.SetTrigger(Enemy.hashAttack);
        }

        public void Exit(Enemy enemy)
        {
            Logger.Log("Attack 탈출");
        }

        public void Update(Enemy enemy)
        {
            Logger.Log("Attack 갱신");
            if(Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;

                //타겟이 없거나 사망했을 경우 idle로 전환
                if (enemy.Target == null || enemy.Target.GetComponent<BasePlayer>().IsDead)
                {
                    enemy.ChangeState<IdleState>();
                    return;
                }


                if (enemy.IsInAttackRange())
                {
                    enemy.animator.SetBool(Enemy.hashIsRun, false);
                    enemy.animator.SetTrigger(Enemy.hashAttack);
                }
                else
                {
                    enemy.ChangeState<ChaseState>();
                }
            }
        }
    }
}
