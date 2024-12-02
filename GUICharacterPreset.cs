using UnityEngine;
using UnityEngine.UI;

public class GUICharacterPreset : MonoBehaviour
{
	public RawImage GUIImage;

	public Text Name;

	public string Description;

	public int Index;

	private void Start()
	{
	}

	public void Select()
	{
		MainMenuManager mainMenuManager = (MainMenuManager)Object.FindObjectOfType(typeof(MainMenuManager));
		if ((bool)mainMenuManager)
		{
			mainMenuManager.OpenPanelByName("NewCharacter");
		}
		CharacterCreatorCanvas characterCreatorCanvas = (CharacterCreatorCanvas)Object.FindObjectOfType(typeof(CharacterCreatorCanvas));
		if ((bool)characterCreatorCanvas)
		{
			characterCreatorCanvas.SelectCreateCharacter(Index);
		}
	}

	public void Hover()
	{
		GUICharacterPresetLoader gUICharacterPresetLoader = (GUICharacterPresetLoader)Object.FindObjectOfType(typeof(GUICharacterPresetLoader));
		if ((bool)gUICharacterPresetLoader)
		{
			gUICharacterPresetLoader.Description.text = Description;
		}
	}

	private void Update()
	{
	}
}
