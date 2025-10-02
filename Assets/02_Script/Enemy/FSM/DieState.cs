using UnityEngine;

namespace LittleSword.Enemy.FSM
{
    public class DieState : IState
    {
        public void Enter(Enemy enemy)
        {
            enemy.animator.SetTrigger(Enemy.hashDie);
            enemy.StopMoving();
            enemy.GetComponent<Collider2D>().enabled = false;
            enemy.rb.bodyType = RigidbodyType2D.Kinematic; //물리 충돌 방지
            Object.Destroy(enemy.gameObject, 5f);
        }


        public void Exit(Enemy enemy) {}

        public void Update(Enemy enemy) {}
    }
}