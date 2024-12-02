using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string UserName = string.Empty;
    public string Team = string.Empty;
    public string UserID = string.Empty;
    public string CharacterKey = string.Empty;
    public string PlayerID = string.Empty;

    [HideInInspector]
    public bool OfflineMode;

    [HideInInspector]
    public string PlayingLevel;

    [HideInInspector]
    public string CurrentLevel;

    [HideInInspector]
    public bool IsPlaying;

    [HideInInspector]
    public int lastLevelPrefix;

    private void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
    }

    public void ConnectingDeny()
    {

    }

    private void Start()
    {
        PlayerPrefs.SetString("landingpage", Application.loadedLevelName);
        UserName = PlayerPrefs.GetString("user_name");
    }

    private void Update()
    {
        CurrentLevel = Application.loadedLevelName;
    }

    public void CreateGame(string startlevel)
    {
        IsPlaying = false;
        PlayingLevel = startlevel;
        RestartGame();
    }

    public void StartGame(string level)
    {
        StartSinglePlayerGame(level);
    }

    public void StartSinglePlayerGame(string level)
    {
        StartLoadLevel(level, lastLevelPrefix);
        IsPlaying = true;
        PlayerPrefs.SetString("user_name", UserName);
    }

    public void RestartGame()
    {
        // Здесь можно добавить логику для перезапуска игры без мультиплеера
    }

    public void QuitGame()
    {
        ClearNetworkGameObject();
        if (Application.loadedLevelName != PlayerPrefs.GetString("landingpage"))
        {
            Application.LoadLevel(PlayerPrefs.GetString("landingpage"));
            Object.Destroy(gameObject);
        }
    }

    public void ClearNetworkGameObject()
    {
        Debug.Log("Clear all objects");
        Object[] objects = Object.FindObjectsOfType(typeof(GameObject));
        for (int i = 0; i < objects.Length; i++)
        {
            GameObject obj = (GameObject)objects[i];
            if (obj != gameObject)
            {
                Object.Destroy(obj);
            }
        }
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 14;
        GUI.skin.label.normal.textColor = Color.white;
        GUI.skin.label.alignment = TextAnchor.UpperLeft;

        GUI.Label(new Rect(0f, 0f, 800f, 30f), "Playing: " + IsPlaying + " | Team: " + Team);
    }

    private void OnLevelWasLoaded()
    {
        Debug.Log(Application.loadedLevelName + " was loaded");
        if (IsPlaying && Application.loadedLevelName == PlayingLevel)
        {
            PlayerPrefs.SetString("StartScene", PlayingLevel);
        }
    }

    public void StartLoadLevel(string level, int levelPrefix)
    {
        PlayingLevel = level;
        Debug.Log("Load level " + level);
        Application.LoadLevel(PlayingLevel);
    }
}