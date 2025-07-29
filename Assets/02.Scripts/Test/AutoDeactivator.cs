using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivator : MonoBehaviour
{
    [SerializeField] private float delay = 1f;

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }


}
