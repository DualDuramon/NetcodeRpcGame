using LittleSword.Player;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Warrior))] // type�� � ��ũ��Ʈ�� �����ų�� ���ϴ� ��. �θ�Ŭ������ �ƴ� �������� Ŭ������ �����ؾ���.
public class BasePlayerEditor : Editor
{
    //�ν����Ϳ� ǥ���ҷ��� OnInspectorGUI() �������̵�
    public override void OnInspectorGUI()
    {
        BasePlayer basePlayer = (BasePlayer)target; // target�� Warrior�� ����Ŵ. Editor�ȿ� ����.
        DrawDefaultInspector(); //���� �ν����Ϳ� �ִ� ������ public �������� �׸���

        //PlayerStats�ʵ�
        basePlayer.playerStats.maxHP = EditorGUILayout.IntField("Max HP", basePlayer.playerStats.maxHP); //�ִ� hp �� ���
        //���� HP�ʵ�
        EditorGUILayout.LabelField("Current HP", basePlayer.CurrentHP.ToString());

        //��ư
        if (GUILayout.Button("�ı�"))
        {
            basePlayer.TakeDamage(10);
        }

        //�ʱ�ȭ
        if (GUILayout.Button("�ʱ�ȭ"))
        {
            basePlayer.CurrentHP = basePlayer.playerStats.maxHP;
        }
    }
}