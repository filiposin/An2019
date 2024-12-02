using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
[DefaultExecutionOrder(-102)]
public class NavMeshPrefabInstance : MonoBehaviour
{
	[SerializeField]
	private NavMeshData m_NavMesh;

	[SerializeField]
	private bool m_FollowTransform;

	private NavMeshDataInstance m_Instance;

	private static readonly List<NavMeshPrefabInstance> s_TrackedInstances = new List<NavMeshPrefabInstance>();

	private Vector3 m_Position;

	private Quaternion m_Rotation;

	public NavMeshData navMeshData
	{
		get
		{
			return m_NavMesh;
		}
		set
		{
			m_NavMesh = value;
		}
	}

	public bool followTransform
	{
		get
		{
			return m_FollowTransform;
		}
		set
		{
			SetFollowTransform(value);
		}
	}

	public static List<NavMeshPrefabInstance> trackedInstances
	{
		get
		{
			return s_TrackedInstances;
		}
	}

	private void OnEnable()
	{
		AddInstance();
		if (m_Instance.valid && m_FollowTransform)
		{
			AddTracking();
		}
	}

	private void OnDisable()
	{
		m_Instance.Remove();
		RemoveTracking();
	}

	public void UpdateInstance()
	{
		m_Instance.Remove();
		AddInstance();
	}

	private void AddInstance()
	{
		if ((bool)m_NavMesh)
		{
			m_Instance = NavMesh.AddNavMeshData(m_NavMesh, base.transform.position, base.transform.rotation);
		}
		m_Rotation = base.transform.rotation;
		m_Position = base.transform.position;
	}

	private void AddTracking()
	{
		if (s_TrackedInstances.Count == 0)
		{
			NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Combine(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(UpdateTrackedInstances));
		}
		s_TrackedInstances.Add(this);
	}

	private void RemoveTracking()
	{
		s_TrackedInstances.Remove(this);
		if (s_TrackedInstances.Count == 0)
		{
			NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Remove(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(UpdateTrackedInstances));
		}
	}

	private void SetFollowTransform(bool value)
	{
		if (m_FollowTransform != value)
		{
			m_FollowTransform = value;
			if (value)
			{
				AddTracking();
			}
			else
			{
				RemoveTracking();
			}
		}
	}

	private bool HasMoved()
	{
		return m_Position != base.transform.position || m_Rotation != base.transform.rotation;
	}

	private static void UpdateTrackedInstances()
	{
		foreach (NavMeshPrefabInstance s_TrackedInstance in s_TrackedInstances)
		{
			if (s_TrackedInstance.HasMoved())
			{
				s_TrackedInstance.UpdateInstance();
			}
		}
	}
}
