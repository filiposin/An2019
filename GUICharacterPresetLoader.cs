using UnityEngine;
using UnityEngine.UI;

public class GUICharacterPresetLoader : MonoBehaviour
{
	public RectTransform CharacterPresetPrefab;
	public Transform Canvas;
	public RectTransform View;
	public Text Description;
	public CharacterPreset[] CharacterPresets;

	private void Start()
	{
		Draw();
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

	public void Draw()
	{
		if (Canvas == null || CharacterPresetPrefab == null || View == null || CharacterPresets == null)
		{
			return;
		}
		ClearCanvas();
		float num = 0f;
		RectTransform component = Canvas.gameObject.GetComponent<RectTransform>();
		float num2 = (CharacterPresetPrefab.sizeDelta.x + 5f) * (float)CharacterPresets.Length;
		if (num2 < View.rect.width)
		{
			num = (View.rect.width - num2) / 2f;
		}
		for (int i = 0; i < CharacterPresets.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(CharacterPresetPrefab.gameObject, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(Canvas.transform);
			GUICharacterPreset component2 = gameObject.GetComponent<GUICharacterPreset>();
			if ((bool)component2)
			{
				component2.GUIImage.texture = CharacterPresets[i].Icon;
				component2.Name.text = CharacterPresets[i].Name;
				component2.Description = CharacterPresets[i].Description;
				component2.Index = i;
			}
			RectTransform component3 = gameObject.GetComponent<RectTransform>();
			if ((bool)component3)
			{
				component3.anchoredPosition = new Vector2(num + (CharacterPresetPrefab.sizeDelta.x + 5f) * (float)i + CharacterPresetPrefab.sizeDelta.x / 2f, 5f);
				component3.localScale = CharacterPresetPrefab.gameObject.transform.localScale;
			}
		}
		component.sizeDelta = new Vector2(num + num + (CharacterPresetPrefab.sizeDelta.x + 5f) * (float)CharacterPresets.Length, component.sizeDelta.y);
	}
}
