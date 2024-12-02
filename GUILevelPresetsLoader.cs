using UnityEngine;

public class GUILevelPresetsLoader : MonoBehaviour
{
	public RectTransform LevelPrefab;
	public Transform Canvas;
	public LevelPreset[] LevelPresets;

	private void Start()
	{
		DrawGameLobby();
	}

	private void Update()
	{
	}

	private void ClearCanvas()
	{
		if (Canvas == null)
		{
			return;
		}
		foreach (Transform item in Canvas.transform)
		{
			Object.Destroy(item.gameObject);
		}
	}

	public void DrawGameLobby()
	{
		if (Canvas == null || LevelPrefab == null || LevelPresets == null)
		{
			return;
		}
		ClearCanvas();
		for (int i = 0; i < LevelPresets.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(LevelPrefab.gameObject, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(Canvas.transform);
			GUILevelPresetSelector component = gameObject.GetComponent<GUILevelPresetSelector>();
			if ((bool)component)
			{
				component.Level = LevelPresets[i];
			}
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			if ((bool)component2)
			{
				component2.anchoredPosition = new Vector2(5f, 0f - (LevelPrefab.sizeDelta.y + 5f) * (float)i);
				component2.localScale = LevelPrefab.gameObject.transform.localScale;
			}
		}
		RectTransform component3 = Canvas.gameObject.GetComponent<RectTransform>();
		component3.sizeDelta = new Vector2(component3.sizeDelta.x, (LevelPrefab.sizeDelta.y + 5f) * (float)LevelPresets.Length);
	}
}
