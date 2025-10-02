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

        //기본 인스펙터 드로잉
        DrawDefaultInspector();

        EditorGUILayout.Space(10); //간격 띄우기
        GUI.enabled = Application.isPlaying; //플레이모드일때만 활성화

        //현재 상태 표시
        EditorGUILayout.LabelField("현재상태", enemy.CurrentStatename);

        EditorGUILayout.BeginHorizontal(); //수평 버튼 배치 시작
        if (GUILayout.Button("Idle 상태")) //버튼 클릭시 상태변경함.
        {
            enemy.ChangeState<IdleState>();
        }

        if (GUILayout.Button("Chase 상태"))
        {
            enemy.ChangeState<ChaseState>();
        }

        if (GUILayout.Button("Attack 상태"))
        {
            enemy.ChangeState<AttackState>();
        }
        EditorGUILayout.EndHorizontal(); //수평 버튼 배치 종료

        GUI.enabled = true;
    }
}
