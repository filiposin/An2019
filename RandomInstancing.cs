using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class RandomInstancing : MonoBehaviour
{
	public GameObject m_Prefab;

	public int m_PoolSize = 250;

	public int m_InstancesPerTile = 10;

	public bool m_RandomPosition = true;

	public bool m_RandomOrientation = true;

	public float m_Height;

	public int m_BaseHash = 347652783;

	public float m_Size = 100f;

	private List<Transform> m_Instances = new List<Transform>();

	private int m_Seed;

	private int m_Used;

	private int m_LocX;

	private int m_LocZ;

	private void Awake()
	{
		for (int i = 0; i < m_PoolSize; i++)
		{
			GameObject gameObject = Object.Instantiate(m_Prefab, Vector3.zero, Quaternion.identity);
			m_Instances.Add(gameObject.transform);
		}
	}

	private void OnEnable()
	{
		m_LocX = -1;
		m_LocZ = -1;
		UpdateInstances();
	}

	private void OnDestroy()
	{
		for (int i = 0; i < m_Instances.Count; i++)
		{
			if ((bool)m_Instances[i])
			{
				Object.DestroyObject(m_Instances[i].gameObject);
			}
		}
		m_Instances.Clear();
	}

	private void Update()
	{
		UpdateInstances();
	}

	private void UpdateInstances()
	{
		int num = (int)Mathf.Floor(base.transform.position.x / m_Size);
		int num2 = (int)Mathf.Floor(base.transform.position.z / m_Size);
		if (num == m_LocX && num2 == m_LocZ)
		{
			return;
		}
		m_LocX = num;
		m_LocZ = num2;
		m_Used = 0;
		for (int i = num - 2; i <= num + 2; i++)
		{
			for (int j = num2 - 2; j <= num2 + 2; j++)
			{
				if (m_Used >= m_PoolSize - 1)
				{
					return;
				}
				UpdateTileInstances(i, j);
			}
		}
	}

	private void UpdateTileInstances(int i, int j)
	{
		m_Seed = Hash2(i, j) ^ m_BaseHash;
		for (int k = 0; k < m_InstancesPerTile; k++)
		{
			float num = 0f;
			float num2 = 0f;
			if (m_RandomPosition)
			{
				num = Random();
				num2 = Random();
			}
			Vector3 position = new Vector3(((float)i + num) * m_Size, m_Height, ((float)j + num2) * m_Size);
			if (m_RandomOrientation)
			{
				float angle = 360f * Random();
				m_Instances[m_Used].rotation = Quaternion.AngleAxis(angle, Vector3.up);
			}
			m_Instances[m_Used].position = position;
			m_Used++;
		}
	}

	private static int Hash2(int i, int j)
	{
		return (i * 73856093) ^ (j * 19349663);
	}

	private float Random()
	{
		m_Seed ^= 123459876;
		int num = m_Seed / 127773;
		m_Seed = 16807 * (m_Seed - num * 127773) - 2836 * num;
		if (m_Seed < 0)
		{
			m_Seed += int.MaxValue;
		}
		float result = (float)m_Seed * 1f / 2.1474836E+09f;
		m_Seed ^= 123459876;
		return result;
	}
}
