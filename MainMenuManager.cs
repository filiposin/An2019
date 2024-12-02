using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : PanelsManager
{
    public string SceneStart = "zombieland";

    public CharacterCreatorCanvas characterCreator;

    public Text CharacterName;

    public GameObject Preloader;

    public GameObject loader;

    public GameObject loading;

    private void Start()
    {
        Application.targetFrameRate = 140;
        characterCreator = (CharacterCreatorCanvas)Object.FindObjectOfType(typeof(CharacterCreatorCanvas));
        if (PlayerPrefs.GetString("StartScene") != string.Empty)
        {
            SceneStart = PlayerPrefs.GetString("StartScene");
        }
    }

    private void Update()
    {
        if (CharacterName && UnitZ.gameManager)
        {
            CharacterName.text = UnitZ.gameManager.UserName;
        }
        if (UnitZ.gameClient && UnitZ.gameClient.isConnecting && Preloader)
        {
            Preloader.SetActive(UnitZ.gameClient.isConnecting);
        }
    }

    public void LevelSelected(string name)
    {
        SceneStart = name;
        PlayerPrefs.SetString("StartScene", SceneStart);
    }

    public void ConnectIP()
    {
        OpenPanelByName("LoadCharacter");
    }

    public void HostGame()
    {
        if (UnitZ.gameManager)
        {
            UnitZ.gameManager.CreateGame(SceneStart); // Удален второй аргумент
        }
    }

    public void UseMasterServer(bool masterserver)
    {
        if (UnitZ.gameServer)
        {
            UnitZ.gameServer.LanOnly = !masterserver;
        }
    }

    public void StartSinglePlayer()
    {
        if (UnitZ.gameManager)
        {
            loader.SetActive(true);
            loading.SetActive(true);
            UnitZ.gameManager.CreateGame(SceneStart); // Удален второй аргумент
            OpenPanelByName("LoadCharacter");
        }
    }

    public void StartNetworkGame()
    {
        if (UnitZ.gameManager)
        {
            UnitZ.gameManager.CreateGame(SceneStart); // Удален второй аргумент
            OpenPanelByName("LoadCharacter");
        }
    }

    public void EnterWorld()
    {
        if (UnitZ.gameManager)
        {
            if (characterCreator)
            {
                characterCreator.SetCharacter();
            }
            UnitZ.gameManager.StartGame(SceneStart);
        }
        OpenPanelByName("Connecting");
    }

    public void ConnectingDeny()
    {
        UnitZ.gameManager.ConnectingDeny();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}