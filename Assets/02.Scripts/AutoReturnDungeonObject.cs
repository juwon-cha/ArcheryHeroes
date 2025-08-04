using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReturnDungeonObject : MonoBehaviour
{
    private void OnEnable()
    {
        DungeonManager.Instance.AddStageChangedEvent(Return);
    }

    void OnDisable()
    {
        DungeonManager.Instance.RemoveStageChangedEvent(Return);
    }

    void Return(int _)
    {
        ObjectPoolingManager.Instance.Return(gameObject);
    }

}
