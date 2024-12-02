using System.Collections.Generic;
using UnityEngine;

public class node_NodeManager : MonoBehaviour
{
	public bool nodesAreChildren;

	public List<Transform> nodes;

	private void Awake()
	{
		if (!nodesAreChildren)
		{
			return;
		}
		foreach (Transform item in base.transform)
		{
			nodes.Add(item.transform);
		}
	}
}
