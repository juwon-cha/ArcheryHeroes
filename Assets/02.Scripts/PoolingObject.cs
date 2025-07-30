using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    private void OnDisable()
    {
        ObjectPoolingManager.Instance.Return(gameObject);
    }
}
