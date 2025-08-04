using UnityEngine;

public class EffectContext
{
    public Vector3 Position;
    public GameObject Caster;    // 효과를 사용하는 주체
    public GameObject Target;    // 타겟 오브젝트 (필요 시)

    public EffectContext(Vector3 position, GameObject caster, GameObject target = null)
    {
        Position = position;
        Caster = caster;
        Target = target;
    }
}
