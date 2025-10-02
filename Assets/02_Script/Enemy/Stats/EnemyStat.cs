using UnityEngine;

[CreateAssetMenu(fileName ="EnemyStats", menuName = "LittleSword/EnemyStats", order = 0)]
public class EnemyStat : ScriptableObject
{
    [Header("Hasic Stats")]
    public int maxHP = 100;
    public float moveSpeed = 3f;


    [Header("Detection Sats")]
    public float detecteInterval = 0.3f;

    [Header("Combat Stats")]
    public float chaseDistance = 5f; //5m 이면 추적시작
    public float attackDistance = 1.5f;
    public int attackDamage = 10;
    public float attackCooldown = 1.0f;
}
