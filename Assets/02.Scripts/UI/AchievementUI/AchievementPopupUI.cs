using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private TextMeshProUGUI progressTxt;

    public void Setup(MissionSO missionData)
    {
        // 유효한 데이터인지 확인
        if (missionData == null)
        {
            Debug.LogError("미션 데이터가 null입니다.");
            return;
        }

        descriptionTxt.text = missionData.Description;

        int currentProgress = AchievementManager.Instance.GetProgress(missionData.MissionID);
        int targetValue = missionData.TargetValue;

        progressTxt.text = $"{currentProgress} / {targetValue}";
    }
}
