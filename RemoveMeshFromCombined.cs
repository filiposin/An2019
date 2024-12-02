using System.Collections.Generic;
using LunarCatsStudio.SuperCombiner;
using UnityEngine;

public class RemoveMeshFromCombined : MonoBehaviour
{
	public List<int> instanceID = new List<int>();

	public CombinedMeshModification meshModifier;

	private void Start()
	{
		foreach (int item in instanceID)
		{
			meshModifier.RemoveFromCombined(item);
		}
	}
}
