using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description; // 도전과제 설명 텍스트
    [SerializeField] private TextMeshProUGUI progressTxt; // 진행도 텍스트
    [SerializeField] private GameObject clearedIcon; // 도전과제 완료 아이콘

    private MissionSO missionData; // 도전과제 데이터

    private void OnEnable()
    {
        AchievementManager.OnChallengeProgressUpdated += HandleProgressUpdate;
    }

    private void OnDisable()
    {
        AchievementManager.OnChallengeProgressUpdated -= HandleProgressUpdate;
    }

    public void Setup(MissionSO mission)
    {
        missionData = mission;

        description.text = mission.Description;

        UpdateProgressUI();
    }

    private void HandleProgressUpdate(string missionID)
    {
        // 이벤트로 전달된 ID가 이 UI의 도전과제 ID와 일치하는지 확인
        if (missionData != null && missionData.MissionID == missionID)
        {
            // 일치하면 UI 업데이트
            UpdateProgressUI();
        }
    }

    public void UpdateProgressUI()
    {
        if (missionData == null)
        {
            Debug.LogError("Mission data is not set.");
            return;
        }

        // AchievementManager에서 현재 진행도를 가져옴
        int currentProgress = AchievementManager.Instance.GetProgress(missionData.MissionID);
        int targetValue = missionData.targetValue;

        progressTxt.text = $"{currentProgress} / {targetValue}";

        // 도전과제 달성 여부 확인
        if (currentProgress >= targetValue)
        {
            progressTxt.SetActive(false);
            clearedIcon.SetActive(true);
        }
        else
        {
            clearedIcon.SetActive(false);
            progressTxt.SetActive(true);
        }
    }
}
