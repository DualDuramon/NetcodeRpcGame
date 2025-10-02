using LittleSword.Player.Weapon;
using UnityEngine;

namespace LittleSword.Player
{
    public class Archer : BasePlayer
    {
        [SerializeField] private GameObject ArrowPrefab;    //화살 프리팹
        [SerializeField] private Transform firePoint;       //화살 발사 위치

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