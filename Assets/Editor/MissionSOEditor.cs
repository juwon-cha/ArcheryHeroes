using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MissionSO))]
public class MissionSOEditor : Editor
{
    SerializedProperty missionIDProp;
    SerializedProperty missionNameProp;
    SerializedProperty descriptionProp;
    SerializedProperty typeProp;
    SerializedProperty targetValueProp;
    SerializedProperty targetEnemyTypeProp;
    SerializedProperty expRewardProp;

    private void OnEnable()
    {
        missionIDProp = serializedObject.FindProperty("MissionID");
        missionNameProp = serializedObject.FindProperty("MissionName");
        descriptionProp = serializedObject.FindProperty("Description");
        typeProp = serializedObject.FindProperty("Type");
        targetValueProp = serializedObject.FindProperty("TargetValue");
        targetEnemyTypeProp = serializedObject.FindProperty("TargetEnemyType");
        expRewardProp = serializedObject.FindProperty("ExpReward");
    }

    // 인스펙터를 다시 그림
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 기본 정보
        EditorGUILayout.LabelField("기본 정보", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(missionIDProp);
        EditorGUILayout.PropertyField(missionNameProp);
        EditorGUILayout.PropertyField(descriptionProp);

        EditorGUILayout.Space(10); // 여백 추가

        // 달성 조건
        EditorGUILayout.LabelField("달성 조건", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(typeProp); // ChallengeType enum 드롭다운

        // typeProp의 현재 값(enum index)을 가져와서 조건부로 필드를 표시
        // (MissionType)typeProp.enumValueIndex를 통해 현재 선택된 enum 값을 가져올 수 있다.
        if ((MissionType)typeProp.enumValueIndex == MissionType.KillEnemyByType)
        {
            // type이 KillEnemyByType일 때만 targetMonsterType 필드를 그림
            EditorGUILayout.PropertyField(targetEnemyTypeProp);
        }

        EditorGUILayout.PropertyField(targetValueProp);

        EditorGUILayout.Space(10);

        // 보상
        EditorGUILayout.LabelField("보상", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(expRewardProp);

        // 수정한 프로퍼티 값들을 실제 객체에 적용
        serializedObject.ApplyModifiedProperties();
    }
}
