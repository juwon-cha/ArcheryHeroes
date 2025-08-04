using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface Surface2D;

    private void Start()
    {
        UpdateNavMesh();
    }

    private void OnEnable()
    {
        DungeonManager.Instance.AddStageChangedEvent(UpdateNavMesh);
    }

    private void OnDisable()
    {
        // DungeonManager.Instance.RemoveStageChangedEvent(UpdateNavMesh);
    }
    void Update()
    {
        // Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }

    void UpdateNavMesh(int _= 0)
    {
        Debug.Log("Updating NavMesh...");
        Surface2D.navMeshData = null; // Clear the existing NavMesh data
        Surface2D.BuildNavMeshAsync();
    }

}
