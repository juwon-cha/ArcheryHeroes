using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IsDeadCondition", menuName = "Data/Condition/IsDeadCondition")]
public class IsDeadCondition : ConditionSO
{
    public override void Initialize() { }

    public override bool IsSatisfied()
    {
        return true;
    }
}
