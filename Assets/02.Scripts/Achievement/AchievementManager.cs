using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : Singleton<AchievementManager>
{
    // Achievement 폴더 안의 모든 ChallengeSO를 담을 리스트
    public List<MissionSO> AllMissions;

    // 각 도전과제의 현재 진행도를 저장할 딕셔너리 (Key: 도전과제 ID, Value: 현재 달성 수치)
    private Dictionary<string, int> missionProgress = new Dictionary<string, int>();

    // 도전과제 진행도 업데이트 시 호출되는 이벤트
    public static event Action<string> OnChallengeProgressUpdated;

    void Awake()
    {
        // 게임 시작 시 모든 도전과제 로드 및 초기화
        LoadAllMissions();
        InitializeProgress();
    }

    private void LoadAllMissions()
    {
        // 간단하게 Resources 폴더에서 모든 ChallengeSO를 불러옴
        // 실무에서는 Addressable Asset System을 사용하는 것이 더 효율적
        AllMissions = new List<MissionSO>(Resources.LoadAll<MissionSO>("Missions"));
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
        // 특정 도전과제의 현재 진행도를 반환
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

    // 게임 내에서 이벤트가 발생할 때 이 함수를 호출
    public void UpdateProgress(MissionType type, int amount)
    {
        foreach (var mission in AllMissions)
        {
            if (mission.type == type)
            {
                missionProgress[mission.MissionID] += amount;
                Debug.Log($"도전과제 '{mission.Title}' 진행도: {missionProgress[mission.MissionID]} / {mission.targetValue}");

                // 진행도가 변경될 때마다 이벤트 구독자에게 알림
                OnChallengeProgressUpdated?.Invoke(mission.MissionID);
            }
        }
    }
}
