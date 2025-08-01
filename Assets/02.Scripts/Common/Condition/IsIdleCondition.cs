using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsIdleCondition : ConditionSO
{
    private PlayerController playerController;

    public override void Initialize()
    {
        playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
    }

    public override bool IsSatisfied()
    {
        bool isIdle = playerController.MovementDirection == Vector2.zero;
        return isIdle;
    }

}
