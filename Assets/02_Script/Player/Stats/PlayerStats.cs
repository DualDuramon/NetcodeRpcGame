using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "LittleSword/PlayerStats", order = 0)]
public class PlayerStats : ScriptableObject
{
    public int maxHP = 100;
    public float moveSpeed = 5.0f;
    public int attackDamage = 20;

}
