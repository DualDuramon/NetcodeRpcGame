using UnityEngine;
using UnityEditor;
using LittleSword.Enemy;
using LittleSword.Enemy.FSM;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Enemy enemy = (Enemy)target;

        //�⺻ �ν����� �����
        DrawDefaultInspector();

        EditorGUILayout.Space(10); //���� ����
        GUI.enabled = Application.isPlaying; //�÷��̸���϶��� Ȱ��ȭ

        //���� ���� ǥ��
        EditorGUILayout.LabelField("�������", enemy.CurrentStatename);

        EditorGUILayout.BeginHorizontal(); //���� ��ư ��ġ ����
        if (GUILayout.Button("Idle ����")) //��ư Ŭ���� ���º�����.
        {
            enemy.ChangeState<IdleState>();
        }

        if (GUILayout.Button("Chase ����"))
        {
            enemy.ChangeState<ChaseState>();
        }

        if (GUILayout.Button("Attack ����"))
        {
            enemy.ChangeState<AttackState>();
        }
        EditorGUILayout.EndHorizontal(); //���� ��ư ��ġ ����

        GUI.enabled = true;
    }
}
