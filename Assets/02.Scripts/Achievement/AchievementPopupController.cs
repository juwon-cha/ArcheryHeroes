using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopupController : MonoBehaviour
{
    [Header("팝업 설정")]
    [SerializeField] private GameObject achievementPopupPrefab; // 팝업 프리팹
    [SerializeField] private Transform popupParent; // 팝업이 생성될 부모
    [SerializeField] private float displayDuration = 3f; // 팝업이 화면에 머무는 시간
    [SerializeField] private SoundDataSO popupSFX; // 팝업 사운드 효과

    private void Start()
    {
        // AchievementManager의 미션 완료 이벤트 구독
        AchievementManager.OnMissionCompleted += ShowCompletionPopup;
    }

    private void OnDestroy()
    {
        AchievementManager.OnMissionCompleted -= ShowCompletionPopup;
    }

    private void ShowCompletionPopup(MissionSO completedMission)
    {
        // 팝업 생성 및 코루틴 시작
        GameObject popupInstance = Instantiate(achievementPopupPrefab, popupParent);
        
        StartCoroutine(PopupRoutine(popupInstance, completedMission));
    }

    private IEnumerator PopupRoutine(GameObject popupInstance, MissionSO missionData)
    {
        // 사운드 효과 재생
        AudioManager.Instance.PlaySFX(popupSFX);

        AchievementPopupUI popupUI = popupInstance.GetComponent<AchievementPopupUI>();
        if(popupUI != null)
        {
            popupUI.Setup(missionData);
        }

        Animator animator = popupInstance.GetComponent<Animator>();

        // 나타나는 애니메이션 실행
        animator.SetTrigger("PopupShow");

        // 설정된 시간만큼 대기
        yield return new WaitForSeconds(displayDuration);

        // 사라지는 애니메이션 실행
        animator.SetTrigger("PopupHide");

        // 애니메이션이 끝날 때까지 잠시 대기
        yield return new WaitForSeconds(1f);

        Destroy(popupInstance);
    }
}
