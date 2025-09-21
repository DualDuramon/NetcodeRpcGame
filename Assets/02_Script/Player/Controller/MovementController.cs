using UnityEngine;

namespace LittleSword.Player.Controller
{
    public class MovementController
    {
        private readonly Rigidbody2D rb; //변경될 필요가 없으므로 readonly
        private readonly SpriteRenderer spriteRenderer;

        //생성자 주입
        public MovementController(Rigidbody2D rb, SpriteRenderer spriteRenderer)
        {
            this.rb = rb;
            this.spriteRenderer = spriteRenderer;
        }

        //이동 메서드
        public void Move(Vector2 dir, float moveSpeed)
        {
            rb.linearVelocity = dir * moveSpeed;
            if(dir != Vector2.zero) //좌우대칭로직
            {
                if(dir.x == 0.0f) return;
                spriteRenderer.flipX = dir.x < 0.0f;        
            }
        }
    }

}