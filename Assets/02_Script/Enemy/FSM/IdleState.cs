using Unity.VisualScripting;
using UnityEngine;
using Logger = LittleSword.Common.Logger;

namespace LittleSword.Enemy.FSM
{
    public class IdleState : IState
    {
        private readonly float detectInterval;
        private float lastDetectTime;

        public IdleState(float detectInterval = 0.3f)
        {
            this.detectInterval = detectInterval;
            lastDetectTime = Time.time - detectInterval;
        }

        public void Enter(Enemy enemy)
        {
            Logger.Log("Idle 진입");
            enemy.animator.SetBool(Enemy.hashIsRun, false);
        }

        public void Update(Enemy enemy)
        {
            if(Time.time - lastDetectTime > detectInterval)
            {
                Logger.Log("Idle 갱신");
                lastDetectTime = Time.time;
                if (enemy.DetectPlayer())
                {
                    enemy.ChangeState<ChaseState>();
                }
            }
        }
        public void Exit(Enemy enemy)
        {
            Logger.Log("Idle 탈출");
        }
    }
}