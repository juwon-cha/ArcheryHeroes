using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AchievementManager : Singleton<AchievementManager>
{
    // Inspector창에서 MissionSO 연결
    public List<MissionSO> AllMissions = new List<MissionSO>();

    // 각 도전과제의 현재 진행도를 저장할 딕셔너리 (Key: 도전과제 ID, Value: 현재 달성 수치)
    private Dictionary<string, int> missionProgress = new Dictionary<string, int>();

    // 도전과제 진행도 업데이트 시 호출되는 이벤트
    public static event Action<string> OnChallengeProgressUpdated;

    // 도전과제 달성 시 호출되는 이벤트
    public static event Action<MissionSO> OnMissionCompleted;

    protected override void Initialize()
    {
        InitializeProgress();
    }

    private void InitializeProgress()
    {
        // TODO: 실제 게임에서는 저장된 데이터를 불러와야 함
        foreach (var challenge in AllMissions)
        {
            if (!missionProgress.ContainsKey(challenge.MissionID))
            {
                missionProgress.Add(challenge.MissionID, 0);
            }
        }
    }

    public int GetProgress(string missionID)
    {
        // 특정 도전과제의 현재 진행도 반환
        if (missionProgress.ContainsKey(missionID))
        {
            return missionProgress[missionID];
        }
        else
        {
            Debug.LogError($"도전과제 ID '{missionID}'가 존재하지 않습니다.");
            return 0;
        }
    }

    // 게임 내에서 이벤트 발생할 때 이 메서드 호출
    public void UpdateProgress(MissionType type, int amount)
    {
        foreach (var mission in AllMissions)
        {
            if (mission.Type == type)
            {
                ProcessProgress(mission, amount);
            }
        }
    }

    public void UpdateKillEnemyByTypeProgress(EnemyType killedEnemyType, int amount)
    {
        // ChallengeManager가 관리하는 모든 도전과제 순회
        foreach (var mission in AllMissions)
        {
            // 도전과제 타입이 타입별 몬스터 처치가 맞는지 확인
            // 처치된 몬스터의 타입이 도전과제의 목표 타입과 일치하는지 확인
            if (mission.Type == MissionType.KillEnemyByType && mission.TargetEnemyType == killedEnemyType)
            {
                ProcessProgress(mission, amount);
            }
        }
    }

    private void ProcessProgress(MissionSO mission, int amount)
    {
        // 이미 달성한 과제는 무시
        if (GetProgress(mission.MissionID) >= mission.TargetValue)
        {
            return;
        }

        // 진행도 증가 및 이벤트 호출
        missionProgress[mission.MissionID] += amount;
        int newProgress = missionProgress[mission.MissionID];
        Debug.Log($"도전과제 '{mission.MissionName}' 진행도: {newProgress} / {mission.TargetValue}");

        OnChallengeProgressUpdated?.Invoke(mission.MissionID);

        // 완료 체크 및 이벤트 호출
        if (newProgress >= mission.TargetValue)
        {
            Debug.Log($"미션 완료: {mission.MissionName}");
            OnMissionCompleted?.Invoke(mission);
        }
    }
}
