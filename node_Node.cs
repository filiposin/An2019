using UnityEngine;

public class node_Node : MonoBehaviour
{
	public Color myColor = Color.red;

	public float myRadius = 0.5f;

	public string nodeGroup;

	private void OnDrawGizmos()
	{
		Gizmos.color = myColor;
		Gizmos.DrawSphere(base.transform.position, myRadius);
	}
}
