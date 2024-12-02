using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
	public GUISkin skin;

	public int PlayerIndex;

	public CharacterSystem[] CharacterBase;

	[HideInInspector]
	public GameManager gameManage;

	[HideInInspector]
	public PlayerSave Save;

	[HideInInspector]
	public PlayerManager playerManage;

	[HideInInspector]
	public int State;

	public Texture2D[] CharactersIcon;

	private Vector2 scrollPosition;

	private CharacterSaveData[] Characters;

	private CharacterSaveData currentCharacter;

	private CharacterSaveData characterCreate;

	private int indexCharacter;

	private int indexDelete;

	private float delta;

	private void Start()
	{
		indexDelete = -1;
		Save = (PlayerSave)Object.FindObjectOfType(typeof(PlayerSave));
		gameManage = (GameManager)Object.FindObjectOfType(typeof(GameManager));
		playerManage = (PlayerManager)Object.FindObjectOfType(typeof(PlayerManager));
		StyleManager styleManager = (StyleManager)Object.FindObjectOfType(typeof(StyleManager));
		if (!skin && (bool)styleManager)
		{
			skin = styleManager.GetSkin(0);
		}
		Characters = Save.LoadAllCharacters();
		indexCharacter = PlayerPrefs.GetInt("INDEX_CRE_CHAR");
	}

	private void Update()
	{
		delta += (0f - delta) / 10f;
	}

	public void OpenCharacter()
	{
		State = 0;
		delta = 1f;
		indexDelete = -1;
		if (!Save)
		{
			return;
		}
		Characters = Save.LoadAllCharacters();
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
			currentCharacter = Characters[indexCharacter];
			PlayerIndex = currentCharacter.CharacterIndex;
			SetCharacter();
		}
	}

	public void SetCharacter()
	{
		indexDelete = -1;
		PlayerIndex = currentCharacter.CharacterIndex;
		if (CharacterBase.Length > 0)
		{
			if (PlayerIndex >= CharacterBase.Length)
			{
				PlayerIndex = CharacterBase.Length - 1;
			}
			if (PlayerIndex < 0)
			{
				PlayerIndex = 0;
			}
			if ((bool)gameManage)
			{
				gameManage.UserName = currentCharacter.PlayerName;
			}
		}
	}

	public void DrawCharacterCreator()
	{
		switch (State)
		{
		case 0:
			GUI.skin.label.fontSize = 25;
			GUI.skin.label.alignment = TextAnchor.UpperCenter;
			GUI.Label(new Rect(Screen.width / 2 - 130, (float)(Screen.height / 2 - 50) + -100f * delta, 260f, 50f), gameManage.UserName);
			if (GUI.Button(new Rect((float)(Screen.width - 210) + 300f * delta, 50f, 160f, 50f), "Create"))
			{
				State = 1;
				delta = 1f;
			}
			if (GUI.Button(new Rect((float)(Screen.width - 210) + 300f * delta, 105f, 160f, 50f), "Characters"))
			{
				State = 3;
				delta = 1f;
				Characters = Save.LoadAllCharacters();
			}
			if (Characters.Length <= 0)
			{
				State = 1;
			}
			break;
		case 1:
			if (Characters.Length > 0 && GUI.Button(new Rect((float)(Screen.width - 210) + 300f * delta, 50f, 160f, 50f), "Cancel"))
			{
				State = 0;
				delta = 1f;
			}
			GUI.BeginGroup(new Rect(Screen.width / 2 - 520, (float)(Screen.height / 2 - 125) + -100f * delta, 1040f, 300f), string.Empty);
			GUI.skin.label.alignment = TextAnchor.UpperCenter;
			GUI.skin.label.fontSize = 20;
			if (GUI.Button(new Rect(0f, 0f, 200f, 300f), CharactersIcon[0]))
			{
				characterCreate = default(CharacterSaveData);
				characterCreate.CharacterIndex = 0;
				State = 2;
			}
			GUI.Label(new Rect(0f, 20f, 200f, 300f), "Policeman");
			if (GUI.Button(new Rect(210f, 0f, 200f, 300f), CharactersIcon[1]))
			{
				characterCreate = default(CharacterSaveData);
				characterCreate.CharacterIndex = 1;
				State = 2;
			}
			GUI.Label(new Rect(210f, 20f, 200f, 300f), "Farmer");
			if (GUI.Button(new Rect(420f, 0f, 200f, 300f), CharactersIcon[2]))
			{
				characterCreate = default(CharacterSaveData);
				characterCreate.CharacterIndex = 2;
				State = 2;
			}
			GUI.Label(new Rect(420f, 20f, 200f, 300f), "Salaryman");
			if (GUI.Button(new Rect(630f, 0f, 200f, 300f), CharactersIcon[3]))
			{
				characterCreate = default(CharacterSaveData);
				characterCreate.CharacterIndex = 3;
				State = 2;
			}
			GUI.Label(new Rect(630f, 20f, 200f, 300f), "Nu New");
			if (GUI.Button(new Rect(840f, 0f, 200f, 300f), CharactersIcon[4]))
			{
				characterCreate = default(CharacterSaveData);
				characterCreate.CharacterIndex = 4;
				State = 2;
			}
			GUI.Label(new Rect(840f, 20f, 200f, 300f), "Doctor");
			GUI.EndGroup();
			break;
		case 2:
			if (GUI.Button(new Rect((float)(Screen.width - 210) + 300f * delta, 50f, 160f, 50f), "Cancel"))
			{
				State = 0;
				delta = 1f;
			}
			gameManage.UserName = GUI.TextField(new Rect(Screen.width / 2 - 130, (float)(Screen.height / 2 - 50) + -100f * delta, 260f, 50f), gameManage.UserName);
			if (GUI.Button(new Rect(Screen.width / 2 - 130, (float)(Screen.height / 2 + 80) + -100f * delta, 260f, 50f), "Create"))
			{
				characterCreate.PlayerName = gameManage.UserName;
				if (Save.CreateCharacter(characterCreate).IsSuccess)
				{
					OpenCharacter();
					State = 3;
				}
			}
			break;
		case 3:
		{
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUI.skin.label.fontSize = 25;
			if (GUI.Button(new Rect((float)(Screen.width - 210) + 300f * delta, 50f, 160f, 50f), "Close"))
			{
				State = 0;
				delta = 1f;
			}
			scrollPosition = GUI.BeginScrollView(new Rect(Screen.width / 2 - 210, 50f + 300f * delta, 420f, Screen.height - 200), scrollPosition, new Rect(0f, 0f, 400f, 160 * Characters.Length));
			for (int i = 0; i < Characters.Length; i++)
			{
				if (GUI.Button(new Rect(0f, i * 160, 340f, 150f), string.Empty))
				{
					currentCharacter = Characters[i];
					indexCharacter = i;
					PlayerPrefs.SetInt("INDEX_CRE_CHAR", indexCharacter);
					SetCharacter();
					State = 0;
				}
				GUI.DrawTexture(new Rect(0f, i * 160, 100f, 150f), CharactersIcon[Characters[i].CharacterIndex]);
				GUI.Label(new Rect(120f, i * 160, 370f, 150f), Characters[i].PlayerName);
				if (indexDelete == i)
				{
					if (GUI.Button(new Rect(350f, i * 160, 50f, 50f), "No"))
					{
						indexDelete = -1;
					}
					if (GUI.Button(new Rect(350f, i * 160 + 60, 50f, 50f), "Yes"))
					{
						Save.RemoveCharacter(Characters[i]);
						OpenCharacter();
						State = 3;
					}
				}
				else if (GUI.Button(new Rect(350f, i * 160, 50f, 50f), "X"))
				{
					indexDelete = i;
				}
			}
			GUI.EndScrollView();
			break;
		}
		}
	}
}
