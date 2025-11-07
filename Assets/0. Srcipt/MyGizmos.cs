using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color myColor = Color.yellow;
    public float radius = 1;
    void OnDrawGizmos()
    {
        Gizmos.color = myColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
