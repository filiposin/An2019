using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public int level;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void startGame()
    {
        Application.LoadLevel(level);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}

public class Mainmenu : MonoBehaviour
{
    public Texture2D LogoGame;

    [HideInInspector]
    public int PageState;

    public string SceneStart = "sandbox";

    [HideInInspector]
    public GameManager gameManage;

    [HideInInspector]
    public CharacterCreator characterCreate;

    public GUISkin skin;

    private float delta;

    private int pageTemp;

    private void Start()
    {
        delta = 1f;
        Application.targetFrameRate = 140;
        gameManage = (GameManager)Object.FindObjectOfType(typeof(GameManager));
        characterCreate = (CharacterCreator)Object.FindObjectOfType(typeof(CharacterCreator));
        StyleManager styleManager = (StyleManager)Object.FindObjectOfType(typeof(StyleManager));
        if (PlayerPrefs.GetString("StartScene") != string.Empty)
        {
            SceneStart = PlayerPrefs.GetString("StartScene");
        }
        if (!skin && (bool)styleManager)
        {
            skin = styleManager.GetSkin(0);
        }
    }

    private void OnGUI()
    {
        Screen.lockCursor = false;
        if ((bool)skin)
        {
            GUI.skin = skin;
        }
        GUI.skin.button.fontSize = 17;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;

        if (SceneStart != "zombieland")
        {
            if (GUI.Button(new Rect(10f, 10f, 150f, 30f), "Zombie Land"))
            {
                SceneStart = "zombieland";
            }
        }
        else
        {
            GUI.Box(new Rect(10f, 10f, 150f, 30f), "Zombie Land");
        }

        if (SceneStart != "sandbox")
        {
            if (GUI.Button(new Rect(170f, 10f, 150f, 30f), "Sandbox"))
            {
                SceneStart = "sandbox";
            }
        }
        else
        {
            GUI.Box(new Rect(170f, 10f, 150f, 30f), "Sandbox");
        }

        if (SceneStart != "training")
        {
            if (GUI.Button(new Rect(330f, 10f, 150f, 30f), "Death Match"))
            {
                SceneStart = "training";
            }
        }
        else
        {
            GUI.Box(new Rect(330f, 10f, 150f, 30f), "Death Match");
        }

        if (PageState == 0)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 130, (float)(Screen.height / 2 + 20) - 100f * delta, 260f, 50f), "Single Player"))
            {
                if ((bool)gameManage)
                {
                    gameManage.OfflineMode = true;
                    characterCreate.OpenCharacter();
                }
                PageState = 1;
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 130, (float)(Screen.height / 2 + 80) - 100f * delta, 260f, 50f), "Exit"))
            {
                Application.Quit();
            }

            GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)LogoGame.width * 0.5f / 2f, (float)(Screen.height / 2 - 200) - 300f * delta, (float)LogoGame.width * 0.5f, (float)LogoGame.height * 0.5f), LogoGame);
        }
        else if (PageState == 1)
        {
            if (GUI.Button(new Rect(50f - 300f * delta, 50f, 160f, 50f), "Back"))
            {
                PageState = 0;
            }
            characterCreate.DrawCharacterCreator();
            if (characterCreate.State == 0 && GUI.Button(new Rect(Screen.width / 2 - 130, (float)(Screen.height / 2 + 80) - 100f * delta, 260f, 50f), "Enter world") && (bool)gameManage)
            {
                characterCreate.SetCharacter();
                gameManage.StartSinglePlayerGame(SceneStart);
            }
        }

        GUI.skin.label.fontSize = 14;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.normal.textColor = Color.white;
        GUI.Label(new Rect(0f, Screen.height - 50, Screen.width, 30f), "UnitZ beta | www.hardworkerstudio.com");
    }

    private void Update()
    {
        delta += (0f - delta) / 10f;
        if (pageTemp != PageState)
        {
            delta = 1f;
            pageTemp = PageState;
        }
    }
}