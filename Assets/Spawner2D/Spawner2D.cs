using UnityEngine;

public class Spawner2D : MonoBehaviour
{
    public GameObject prefab;
    public float radius;
    public int count;

    [HideInInspector] public Transform root;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
