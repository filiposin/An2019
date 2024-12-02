using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreatorCanvas : MonoBehaviour
{
	public RectTransform CharacterBadgePrefab;

	public RectTransform Canvas;

	[HideInInspector]
	public CharacterSaveData[] Characters;

	private Vector2 scrollPosition;

	private int indexCharacter;

	public void Setup()
	{
		ClearCanvas();
		indexCharacter = PlayerPrefs.GetInt("INDEX_CRE_CHAR");
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

	private void Start()
	{
		Setup();
	}

	public IEnumerator LoadCharacters()
	{
		if (!UnitZ.playerSave)
		{
			yield break;
		}
		Characters = UnitZ.playerSave.LoadAllCharacters();
		while (Characters == null)
		{
			yield return new WaitForEndOfFrame();
		}
		if (Characters.Length > 0)
		{
			if (indexCharacter >= Characters.Length)
			{
				indexCharacter = Characters.Length - 1;
			}
			if (indexCharacter < 0)
			{
				indexCharacter = 0;
			}
			if ((bool)UnitZ.characterManager)
			{
				UnitZ.characterManager.SetupCharacter(Characters[indexCharacter]);
			}
		}
		DrawCharactersToCanvas();
	}

	public void DrawCharactersToCanvas()
	{
		if (Canvas == null || CharacterBadgePrefab == null || Characters == null)
		{
			return;
		}
		ClearCanvas();
		for (int i = 0; i < Characters.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(CharacterBadgePrefab.gameObject, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(Canvas.transform);
			CharacterBadge component = gameObject.GetComponent<CharacterBadge>();
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			if ((bool)component2)
			{
				component2.anchoredPosition = new Vector2(5f, 0f - (CharacterBadgePrefab.sizeDelta.y + 5f) * (float)i);
				component2.localScale = CharacterBadgePrefab.gameObject.transform.localScale;
			}
			if ((bool)component)
			{
				component.Index = i;
				component.CharacterData = Characters[i];
				if ((bool)UnitZ.characterManager && UnitZ.characterManager.CharacterPresets.Length > 0 && Characters[i].CharacterIndex < UnitZ.characterManager.CharacterPresets.Length)
				{
					component.GUIImage.texture = UnitZ.characterManager.CharacterPresets[Characters[i].CharacterIndex].Icon;
				}
				component.GUIName.text = Characters[i].PlayerName;
				component.CharacterCreatorS = this;
				component.name = Characters[i].PlayerName;
			}
		}
		Canvas.sizeDelta = new Vector2(Canvas.sizeDelta.x, (CharacterBadgePrefab.sizeDelta.y + 5f) * (float)Characters.Length);
	}

	public void CreateCharacter(Text textName)
	{
		if ((bool)UnitZ.characterManager && (bool)textName && UnitZ.characterManager.CreateCharacter(textName.text))
		{
			Setup();
			StartCoroutine(LoadCharacters());
			MainMenuManager mainMenuManager = (MainMenuManager)Object.FindObjectOfType(typeof(MainMenuManager));
			if ((bool)mainMenuManager)
			{
				mainMenuManager.OpenPanelByNameNoPreviousSave("LoadCharacter");
			}
		}
	}

	public void SelectCharacter(CharacterSaveData character)
	{
		if ((bool)UnitZ.characterManager)
		{
			UnitZ.characterManager.SetupCharacter(character);
		}
		MainMenuManager mainMenuManager = (MainMenuManager)Object.FindObjectOfType(typeof(MainMenuManager));
		if ((bool)mainMenuManager)
		{
			mainMenuManager.OpenPanelByName("EnterWorld");
		}
	}

	public void SetCharacter()
	{
		if ((bool)UnitZ.characterManager)
		{
			UnitZ.characterManager.SetCharacter();
		}
	}

	public void SelectCreateCharacter(int index)
	{
		if ((bool)UnitZ.characterManager)
		{
			UnitZ.characterManager.SelectCreateCharacter(index);
		}
	}

	public void RemoveCharacter(int index)
	{
		if ((bool)UnitZ.characterManager)
		{
			UnitZ.characterManager.RemoveCharacter(Characters[index]);
			StartCoroutine(LoadCharacters());
		}
	}
}
