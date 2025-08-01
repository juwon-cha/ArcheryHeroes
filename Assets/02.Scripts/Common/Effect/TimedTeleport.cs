using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Effect/TimedTeleport")]
public class TimedTeleport : EffectSO
{
    [SerializeField] private float teleportTime = 3.0f;
    [SerializeField] private GameObject portalEffectPrefab;
    private GameObject portalEffect;

    public override void Initialize() { }

    public override void Execute(EffectContext effectContext = null)
    {
        GameObject caster = effectContext.Caster;
        CoroutineRunner.Instance.StartCoroutine(TeleportAfterDelay(caster));
    }

    private IEnumerator TeleportAfterDelay(GameObject target)
    {
        portalEffect = ObjectPoolingManager.Instance.Get(portalEffectPrefab, target.GetPosition(), Quaternion.identity);
        yield return new WaitForSeconds(teleportTime);
        target.transform.position = portalEffect.GetPosition();
        portalEffect.SetActive(false);
    }
}
