using LittleSword.Player.Weapon;
using UnityEngine;

namespace LittleSword.Player
{
    public class Archer : BasePlayer
    {
        [SerializeField] private GameObject ArrowPrefab;    //ȭ�� ������
        [SerializeField] private Transform firePoint;       //ȭ�� �߻� ��ġ

        public void OnArcherAttack()
        {
            FireArrow();
        }

        private void FireArrow()
        {
            Quaternion rot = Quaternion.Euler(0f, spriteRenderer.flipX ? 180f : 0f, 0f);
            GameObject arrow = Instantiate(ArrowPrefab, firePoint.position, rot);
            arrow.GetComponent<Arrow>().Init(playerStats.fireForce, playerStats.attackDamage);
        }
    }
}