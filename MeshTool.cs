using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshTool : MonoBehaviour
{
	public enum ExtrudeMethod
	{
		Vertical = 0,
		MeshNormal = 1
	}

	public List<MeshFilter> m_Filters = new List<MeshFilter>();

	public float m_Radius = 1.5f;

	public float m_Power = 2f;

	public ExtrudeMethod m_Method;

	private RaycastHit m_HitInfo = default(RaycastHit);

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
		{
			Debug.DrawRay(m_HitInfo.point, m_HitInfo.normal, Color.red);
			Vector3 vector = ((m_Method != 0) ? m_HitInfo.normal : Vector3.up);
			if (Input.GetMouseButton(0) || (Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift)))
			{
				ModifyMesh(m_Power * vector, m_HitInfo.point);
			}
			if (Input.GetMouseButton(1) || (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift)))
			{
				ModifyMesh((0f - m_Power) * vector, m_HitInfo.point);
			}
		}
	}

	private void ModifyMesh(Vector3 displacement, Vector3 center)
	{
		foreach (MeshFilter filter in m_Filters)
		{
			Mesh mesh = filter.mesh;
			Vector3[] vertices = mesh.vertices;
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 pos = filter.transform.TransformPoint(vertices[i]);
				vertices[i] += displacement * Gaussian(pos, center, m_Radius);
			}
			mesh.vertices = vertices;
			mesh.RecalculateBounds();
			MeshCollider component = filter.GetComponent<MeshCollider>();
			if (component != null)
			{
				Mesh mesh2 = new Mesh();
				mesh2.vertices = mesh.vertices;
				mesh2.triangles = mesh.triangles;
				component.sharedMesh = mesh2;
			}
		}
	}

	private static float Gaussian(Vector3 pos, Vector3 mean, float dev)
	{
		float num = pos.x - mean.x;
		float num2 = pos.y - mean.y;
		float num3 = pos.z - mean.z;
		float num4 = 1f / ((float)Math.PI * 2f * dev * dev);
		return num4 * Mathf.Pow((float)Math.E, (0f - (num * num + num2 * num2 + num3 * num3)) / (2f * dev * dev));
	}
}
