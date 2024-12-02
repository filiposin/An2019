using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
	public List<AITarget> TargetObject;

	private float timeTemp;

	public float LoopInterval = 0.1f;

	private void Start()
	{
		TargetObject = new List<AITarget>();
		timeTemp = Time.time;
	}

	public void Searching(string[] tags)
	{
		for (int i = 0; i < tags.Length; i++)
		{
			if (!checking(tags[i]))
			{
				Debug.Log("Add new target");
				AITarget aITarget = new AITarget();
				aITarget.Tag = tags[i];
				aITarget.Targets = null;
				TargetObject.Add(aITarget);
			}
		}
	}

	private bool checking(string tag)
	{
		foreach (AITarget item in TargetObject)
		{
			if (item.Tag == tag)
			{
				return true;
			}
		}
		return false;
	}

	public GameObject[] GetTargets(string targettag)
	{
		foreach (AITarget item in TargetObject)
		{
			if (item.Tag == targettag)
			{
				return item.Targets;
			}
		}
		return null;
	}

	public void Loop()
	{
		foreach (AITarget item in TargetObject)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(item.Tag);
			if (array != null && array.Length > 0)
			{
				item.Targets = array;
			}
		}
	}

	private void Update()
	{
		if (Time.time >= timeTemp + LoopInterval)
		{
			Loop();
			timeTemp = Time.time;
		}
	}
}
