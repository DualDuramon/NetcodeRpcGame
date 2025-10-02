using LittleSword.Interfaces;
using UnityEngine;

namespace LittleSword.Player
{
    public class Warrior : BasePlayer
    {
        [SerializeField] private LayerMask enemyLayer;

        [SerializeField] private Vector2 size = new Vector2(1.0f, 2.0f);
        [SerializeField] private float offset = 0.5f;

        //애니메이션 이벤트에서 호출
        public void OnWarriorAttack()
        {
            Vector2 dir = spriteRenderer.flipX ? Vector2.left : Vector2.right; //방향체크
            Vector2 center = (Vector2)transform.position + dir * offset;


            Collider2D[] colls = Physics2D.OverlapBoxAll(center, size, 0.0f, enemyLayer);

            foreach(var collider in colls)
            {
                collider.GetComponent<IDamageable>()?.TakeDamage(playerStats.attackDamage);

            }
        }

        private void OnDrawGizmos()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            Vector2 center = (Vector2)transform.position + direction * offset;

            Gizmos.color = new Color(1.0f, 0f, 0f, 0.3f);
            Gizmos.DrawCube(center, new Vector3(size.x, size.y, 0.0f));
        }
    }
}
