using UnityEngine;

public class GameMenu : MonoBehaviour
{
	public GUISkin skin;

	public Texture2D BG;

	public bool Toggle;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Toggle = !Toggle;
			MouseLock.MouseLocked = !Toggle;
		}
	}

	private void OnGUI()
	{
		if (!Toggle)
		{
			return;
		}
		if ((bool)skin)
		{
			GUI.skin = skin;
		}
		if ((bool)BG)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), BG);
		}
		if (GUI.Button(new Rect(Screen.width / 2 - 130, Screen.height / 2 - 80, 260f, 50f), "Resume"))
		{
			Toggle = false;
			MouseLock.MouseLocked = true;
		}
		if (GUI.Button(new Rect(Screen.width / 2 - 130, Screen.height / 2 - 20, 260f, 50f), "Disconnect"))
		{
			if ((bool)UnitZ.gameManager)
			{
				UnitZ.gameManager.QuitGame();
			}
			MouseLock.MouseLocked = false;
		}
		GUI.skin.label.fontSize = 50;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.label.normal.textColor = Color.white;
		GUI.Label(new Rect(0f, Screen.height / 2 - 180, Screen.width, 30f), "Main menu");
	}
}
