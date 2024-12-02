using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(-102)]
public class LocalNavMeshBuilder : MonoBehaviour
{
	public Transform m_Tracked;

	public Vector3 m_Size = new Vector3(80f, 20f, 80f);

	private NavMeshData m_NavMesh;

	private AsyncOperation m_Operation;

	private NavMeshDataInstance m_Instance;

	private List<NavMeshBuildSource> m_Sources = new List<NavMeshBuildSource>();

	private IEnumerator Start()
	{
		while (true)
		{
			UpdateNavMesh(true);
			yield return m_Operation;
		}
	}

	private void OnEnable()
	{
		m_NavMesh = new NavMeshData();
		m_Instance = NavMesh.AddNavMeshData(m_NavMesh);
		if (m_Tracked == null)
		{
			m_Tracked = base.transform;
		}
		UpdateNavMesh();
	}

	private void OnDisable()
	{
		m_Instance.Remove();
	}

	private void UpdateNavMesh(bool asyncUpdate = false)
	{
		NavMeshSourceTag.Collect(ref m_Sources);
		NavMeshBuildSettings settingsByID = NavMesh.GetSettingsByID(0);
		Bounds localBounds = QuantizedBounds();
		if (asyncUpdate)
		{
			m_Operation = NavMeshBuilder.UpdateNavMeshDataAsync(m_NavMesh, settingsByID, m_Sources, localBounds);
		}
		else
		{
			NavMeshBuilder.UpdateNavMeshData(m_NavMesh, settingsByID, m_Sources, localBounds);
		}
	}

	private static Vector3 Quantize(Vector3 v, Vector3 quant)
	{
		float x = quant.x * Mathf.Floor(v.x / quant.x);
		float y = quant.y * Mathf.Floor(v.y / quant.y);
		float z = quant.z * Mathf.Floor(v.z / quant.z);
		return new Vector3(x, y, z);
	}

	private Bounds QuantizedBounds()
	{
		Vector3 v = ((!m_Tracked) ? base.transform.position : m_Tracked.position);
		return new Bounds(Quantize(v, 0.1f * m_Size), m_Size);
	}

	private void OnDrawGizmosSelected()
	{
		if ((bool)m_NavMesh)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(m_NavMesh.sourceBounds.center, m_NavMesh.sourceBounds.size);
		}
		Gizmos.color = Color.yellow;
		Bounds bounds = QuantizedBounds();
		Gizmos.DrawWireCube(bounds.center, bounds.size);
		Gizmos.color = Color.green;
		Vector3 center = ((!m_Tracked) ? base.transform.position : m_Tracked.position);
		Gizmos.DrawWireCube(center, m_Size);
	}
}
