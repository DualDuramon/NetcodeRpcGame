using UnityEngine;

namespace LittleSword.Player.Controller
{
    public class MovementController
    {
        private readonly Rigidbody2D rb; //����� �ʿ䰡 �����Ƿ� readonly
        private readonly SpriteRenderer spriteRenderer;

        //������ ����
        public MovementController(Rigidbody2D rb, SpriteRenderer spriteRenderer)
        {
            this.rb = rb;
            this.spriteRenderer = spriteRenderer;
        }

        //�̵� �޼���
        public void Move(Vector2 dir, float moveSpeed)
        {
            rb.linearVelocity = dir * moveSpeed;
            if(dir != Vector2.zero) //�¿��Ī����
            {
                if(dir.x == 0.0f) return;
                spriteRenderer.flipX = dir.x < 0.0f;        
            }
        }
    }

}