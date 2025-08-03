using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementListUI : MonoBehaviour
{
    [SerializeField] private Button closeButton; // 닫기 버튼

    public GameObject missionUIPrefab;  // 도전 과제 UI 프리팹
    public Transform contentParent;     // 생성된 슬롯들이 위치할 부모 객체 (Scroll View의 Content)

    private void Awake()
    {
        closeButton.onClick.AddListener(OnClose);
    }

    private void Start()
    {
        // 도전과제 리스트 UI 새로고침
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        var allMissions = AchievementManager.Instance.AllMissions;

        // 리스트에 있는 모든 도전과제에 대해 UI 생성
        CreateMissionSlots(allMissions);
    }

    private void CreateMissionSlots(List<MissionSO> missions)
    {
        foreach (var mission in missions)
        {
            // contentParent의 자식으로 하여 인스턴스화
            GameObject missionInstance = Instantiate(missionUIPrefab, contentParent);

            MissionUI missionUI = missionInstance.GetComponent<MissionUI>();

            if (missionUI != null)
            {
                missionUI.Setup(mission);
            }
            else
            {
                Debug.LogError("MissionUI 컴포넌트가 미션 UI 프리팹에 없습니다.");
            }
        }
    }

    public void OnClose()
    {
        //UIManager.Instance.HideUI(UIType.AchievementList);
    }
}
