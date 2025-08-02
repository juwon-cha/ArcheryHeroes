using UnityEngine;

// 도전과제 달성 조건 타입
public enum MissionType
{
    KillEnemy,
    CollectItem,
    ReachLevel
}

[CreateAssetMenu(fileName = "NewMission", menuName = "Achievement/Mission Data")]
public class MissionSO : ScriptableObject
{
    [Header("기본 정보")]
    public string MissionID; // 도전과제 식별자
    public string Title;       // UI에 표시될 제목
    [TextArea]
    public string Description; // UI에 표시될 설명

    [Header("달성 조건")]
    public MissionType type;    // 도전과제 타입
    public int targetValue;     // 목표 수치 (예: 10마리 처치, 5개 수집)

    [Header("보상")]
    public int expReward;
    // TODO: 보상
}