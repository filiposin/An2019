using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public bool DrawGUI = true;

	public GUISkin skin;

	public bool Toggle;

	private GUIKillBadgeManager guiBadgeManager;

	private Vector2 scrollPosition;

	private int playerCount;

	private void Start()
	{
		StyleManager styleManager = (StyleManager)Object.FindObjectOfType(typeof(StyleManager));
		guiBadgeManager = (GUIKillBadgeManager)Object.FindObjectOfType(typeof(GUIKillBadgeManager));
		if (!skin && (bool)styleManager)
		{
			skin = styleManager.GetSkin(0);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab) && (bool)UnitZ.gameManager && UnitZ.gameManager.IsPlaying)
		{
			Toggle = !Toggle;
		}
	}

	public void UpdatePlayerScore(string id, int score, int dead)
	{
		foreach (PlayerData player in UnitZ.playersManager.PlayerList)
		{
			if (id == player.ID)
			{
				player.Score += score;
				player.Dead += dead;
				break;
			}
		}
	}

	public void AddKillText(string killer, string victim, string killtype)
	{
		if (guiBadgeManager != null && (bool)UnitZ.playersManager)
		{
			PlayerData playerData = UnitZ.playersManager.GetPlayerData(killer);
			PlayerData playerData2 = UnitZ.playersManager.GetPlayerData(victim);
			string killer2 = "N/A";
			string victim2 = "N/A";
			if (playerData != null)
			{
				killer2 = playerData.Name;
			}
			if (playerData2 != null)
			{
				victim2 = playerData2.Name;
			}
			guiBadgeManager.PushKillText(killer2, victim2, killtype);
		}
	}

	private void OnGUI()
	{
		if (!Toggle || !DrawGUI)
		{
			return;
		}
		if ((bool)skin)
		{
			GUI.skin = skin;
		}
		Vector2 vector = new Vector2(450f, 350f);
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.skin.label.fontSize = 35;
		GUI.Box(new Rect((float)(Screen.width / 2) - vector.x / 2f - 10f, (float)(Screen.height / 2) - vector.y / 2f - 10f, vector.x + 20f, vector.y + 20f), string.Empty);
		GUI.BeginGroup(new Rect((float)(Screen.width / 2) - vector.x / 2f, (float)(Screen.height / 2) - vector.y / 2f, vector.x, vector.y), string.Empty);
		GUI.skin.label.fontSize = 19;
		GUI.Label(new Rect(20f, 0f, 200f, 30f), "Player Name");
		GUI.Label(new Rect(250f, 0f, 100f, 30f), "Kills");
		GUI.Label(new Rect(360f, 0f, 100f, 30f), "Death");
		scrollPosition = GUI.BeginScrollView(new Rect(0f, 0f, vector.x, vector.y), scrollPosition, new Rect(0f, 0f, vector.x - 20f, playerCount * 35));
		playerCount = 1;
		foreach (string team in UnitZ.playersManager.TeamList)
		{
			if (team != string.Empty)
			{
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
				GUI.Label(new Rect(20f, playerCount * 35, 200f, 30f), "TEAM " + team);
				playerCount++;
			}
			foreach (PlayerData player in UnitZ.playersManager.PlayerList)
			{
				if (player.IsConnected && team == player.Team)
				{
					GUI.skin.label.alignment = TextAnchor.MiddleLeft;
					GUI.Label(new Rect(20f, playerCount * 35, 200f, 30f), "<color=lime>" + player.Name.ToString() + "</color>");
					GUI.skin.label.alignment = TextAnchor.MiddleCenter;
					GUI.Label(new Rect(250f, playerCount * 35, 50f, 30f), player.Score.ToString());
					GUI.Label(new Rect(360f, playerCount * 35, 30f, 30f), player.Dead.ToString());
					playerCount++;
				}
			}
		}
		GUI.EndScrollView();
		GUI.EndGroup();
	}
}
