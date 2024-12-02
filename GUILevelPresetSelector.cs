using UnityEngine;
using UnityEngine.UI;

public class GUILevelPresetSelector : MonoBehaviour
{
	public LevelPreset Level;

	public RawImage Icon;

	public Text Name;

	public Text Detail;

	private void Start()
	{
		if (Level != null)
		{
			if ((bool)Level.Icon)
			{
				Icon.texture = Level.Icon;
			}
			if ((bool)Name)
			{
				Name.text = Level.LevelName;
			}
			if ((bool)Detail)
			{
				Detail.text = Level.Detail;
			}
		}
	}

	public void Select()
	{
		MainMenuManager mainMenuManager = (MainMenuManager)Object.FindObjectOfType(typeof(MainMenuManager));
		if (mainMenuManager != null && Level != null)
		{
			mainMenuManager.LevelSelected(Level.SceneName);
			mainMenuManager.OpenPreviousPanel();
		}
	}
}
