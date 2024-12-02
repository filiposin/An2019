using System.Collections.Generic;
using UnityEngine;

public class GUIKillBadgeManager : MonoBehaviour
{
	public GUIKillBadge killBadge;

	public float LifeTime = 5f;

	private List<GUIKillBadge> badgeList = new List<GUIKillBadge>();

	private void Start()
	{
		badgeList = new List<GUIKillBadge>();
	}

	private void testText()
	{
		PushKillText("Sun", "Palm", "Sniper");
	}

	public void PushKillText(string killer, string victim, string killtype)
	{
		PushText(killer + " <color='#FF9900'>" + killtype + "</color> " + victim);
	}

	public void PushText(string text)
	{
		GameObject gameObject = Object.Instantiate(killBadge.gameObject, Vector3.zero, Quaternion.identity);
		gameObject.gameObject.transform.SetParent(base.transform);
		GUIKillBadge component = gameObject.GetComponent<GUIKillBadge>();
		component.KillText.text = text;
		component.timeTemp = Time.time;
		badgeList.Add(component);
	}

	private void Update()
	{
		RectTransform component = killBadge.GetComponent<RectTransform>();
		for (int i = 0; i < badgeList.Count; i++)
		{
			if (badgeList[i] != null)
			{
				RectTransform component2 = badgeList[i].gameObject.GetComponent<RectTransform>();
				if ((bool)component2)
				{
					component2.anchoredPosition = new Vector2((0f - component.sizeDelta.x) / 2f, 0f - (component.sizeDelta.y * (float)i + component.sizeDelta.y / 2f));
					component2.localScale = killBadge.gameObject.transform.localScale;
				}
				if (Time.time > badgeList[i].timeTemp + LifeTime)
				{
					Object.Destroy(badgeList[i].gameObject);
					badgeList.RemoveAt(i);
				}
			}
		}
	}
}
