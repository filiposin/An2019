using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(-200)]
public class NavMeshSourceTag : MonoBehaviour
{
	public static List<MeshFilter> m_Meshes = new List<MeshFilter>();

	public static List<Terrain> m_Terrains = new List<Terrain>();

	private void OnEnable()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		if (component != null)
		{
			m_Meshes.Add(component);
		}
		Terrain component2 = GetComponent<Terrain>();
		if (component2 != null)
		{
			m_Terrains.Add(component2);
		}
	}

	private void OnDisable()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		if (component != null)
		{
			m_Meshes.Remove(component);
		}
		Terrain component2 = GetComponent<Terrain>();
		if (component2 != null)
		{
			m_Terrains.Remove(component2);
		}
	}

	public static void Collect(ref List<NavMeshBuildSource> sources)
	{
		sources.Clear();
		for (int i = 0; i < m_Meshes.Count; i++)
		{
			MeshFilter meshFilter = m_Meshes[i];
			if (!(meshFilter == null))
			{
				Mesh sharedMesh = meshFilter.sharedMesh;
				if (!(sharedMesh == null))
				{
					NavMeshBuildSource item = default(NavMeshBuildSource);
					item.shape = NavMeshBuildSourceShape.Mesh;
					item.sourceObject = sharedMesh;
					item.transform = meshFilter.transform.localToWorldMatrix;
					item.area = 0;
					sources.Add(item);
				}
			}
		}
		for (int j = 0; j < m_Terrains.Count; j++)
		{
			Terrain terrain = m_Terrains[j];
			if (!(terrain == null))
			{
				NavMeshBuildSource item2 = default(NavMeshBuildSource);
				item2.shape = NavMeshBuildSourceShape.Terrain;
				item2.sourceObject = terrain.terrainData;
				item2.transform = Matrix4x4.TRS(terrain.transform.position, Quaternion.identity, Vector3.one);
				item2.area = 0;
				sources.Add(item2);
			}
		}
	}
}
