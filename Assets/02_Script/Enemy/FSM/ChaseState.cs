using UnityEngine;
using Logger = LittleSword.Common.Logger;

namespace LittleSword.Enemy.FSM
{
    public class ChaseState : IState
    {
        private readonly float detectInterval;
        private float lastDetectTime;

        public ChaseState(float detectInterval = 0.3f)
        {
            this.detectInterval = detectInterval;
            lastDetectTime = Time.time - detectInterval;
        }

        public void Enter(Enemy enemy)
        {
            Logger.Log("Chase 진입");
            enemy.animator.SetBool(Enemy.hashIsRun, true);
        }

        public void Exit(Enemy enemy)
        {
            Logger.Log("Chase 탈출");
        }

        public void Update(Enemy enemy)
        {
            if (Time.time - lastDetectTime > detectInterval)
            {
                lastDetectTime = Time.time;
                Logger.Log("Chase 갱신");
                if (enemy.DetectPlayer())
                {
                    enemy.MoveToPlayer();
                    if (enemy.IsInAttackRange())
                    {
                        // 이동 정지 및 공격
                        enemy.StopMoving();
                        enemy.ChangeState<AttackState>();
                    }
                }
                else
                {
                    enemy.StopMoving();
                    enemy.ChangeState<IdleState>();
                }
            }
        }
    }
}
