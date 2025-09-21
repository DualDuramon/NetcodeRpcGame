using LittleSword.Player;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Warrior))] // type은 어떤 스크립트에 적용시킬지 정하는 것. 부모클래스가 아닌 직접적인 클래스를 선택해야함.
public class BasePlayerEditor : Editor
{
    //인스펙터에 표시할려면 OnInspectorGUI() 오버라이드
    public override void OnInspectorGUI()
    {
        BasePlayer basePlayer = (BasePlayer)target; // target은 Warrior를 가리킴. Editor안에 있음.
        DrawDefaultInspector(); //기존 인스펙터에 있는 변수들 public 변수들을 그리기

        //PlayerStats필드
        basePlayer.playerStats.maxHP = EditorGUILayout.IntField("Max HP", basePlayer.playerStats.maxHP); //최대 hp 값 출력
        //현재 HP필드
        EditorGUILayout.LabelField("Current HP", basePlayer.CurrentHP.ToString());

        //버튼
        if (GUILayout.Button("파괴"))
        {
            basePlayer.TakeDamage(10);
        }

        //초기화
        if (GUILayout.Button("초기화"))
        {
            basePlayer.CurrentHP = basePlayer.playerStats.maxHP;
        }
    }
}