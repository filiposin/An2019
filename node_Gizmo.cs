using UnityEngine;

public class node_Gizmo : MonoBehaviour
{
	public enum GizmoShape
	{
		Sphere = 0,
		Cube = 1
	}

	public GizmoShape gizmoShape;

	public Color gizmoColor = Color.red;

	public float gizmoSize = 1f;

	private void OnDrawGizmos()
	{
		Gizmos.color = gizmoColor;
		switch (gizmoShape)
		{
		case GizmoShape.Sphere:
			Gizmos.DrawSphere(base.transform.position, gizmoSize);
			break;
		case GizmoShape.Cube:
			Gizmos.DrawCube(base.transform.position, new Vector3(1f * gizmoSize, 1f * gizmoSize, 1f * gizmoSize));
			break;
		}
	}
}
