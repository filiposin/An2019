using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GUISkin skin;

    [HideInInspector]
    public CharacterSystem playingCharacter;

    public float SaveInterval = 5f;

    public bool SavePlayer = true;

    [HideInInspector]
    public bool ManualSpawn;

    [HideInInspector]
    public bool DisabledSpawn;

    private float timeTemp;

    private bool savePlayerTemp;

    public GameObject BlackIns;

    private void Start()
    {
        savePlayerTemp = SavePlayer;
        if (!skin && (bool)UnitZ.styleManager)
        {
            skin = UnitZ.styleManager.GetSkin(0);
        }
    }

    public void Reset()
    {
        ManualSpawn = false;
        DisabledSpawn = false;
        SavePlayer = savePlayerTemp;
    }

    private void Update()
    {
        if (!(UnitZ.gameManager == null) && UnitZ.gameManager.IsPlaying)
        {
            OnPlaying();
        }
    }

    private void findPlayerCharacter()
    {
        CharacterSystem[] array = (CharacterSystem[])Object.FindObjectsOfType(typeof(CharacterSystem));
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].IsMine)
            {
                playingCharacter = array[i];
                break;
            }
        }
    }

    public void OnPlaying()
    {
        if ((bool)playingCharacter)
        {
            if ((bool)UnitZ.playerSave && Time.time >= timeTemp + SaveInterval)
            {
                timeTemp = Time.time;
                if (SavePlayer)
                {
                    UnitZ.playerSave.SavePlayer(playingCharacter);
                }
            }
        }
        else
        {
            findPlayerCharacter();
        }
    }

    public void RequestSpawnPlayer()
    {
        InstantiatePlayerObject(UnitZ.gameManager.PlayerID, UnitZ.gameManager.UserID, UnitZ.gameManager.UserName, UnitZ.gameManager.CharacterKey, UnitZ.characterManager.CharacterIndex, UnitZ.gameManager.Team, -1);
    }

    private bool InstantiatePlayerObject(string playerID, string userID, string userName, string characterKey, int index, string team, int spawner)
    {
        if (UnitZ.characterManager == null || UnitZ.characterManager.CharacterPresets.Length <= index || index < 0)
        {
            return false;
        }
        CharacterSystem characterPrefab = UnitZ.characterManager.CharacterPresets[index].CharacterPrefab;
        PlayerSpawner[] array = (PlayerSpawner[])Object.FindObjectsOfType(typeof(PlayerSpawner));
        if (spawner < 0 || spawner >= array.Length)
        {
            spawner = Random.Range(0, array.Length);
        }
        if (characterPrefab && array.Length > 0)
        {
            CharacterSystem component = array[spawner].Spawn(characterPrefab.gameObject).GetComponent<CharacterSystem>();

            // Удален вызов ReceivePlayerInfo
            // Вместо этого, вы можете установить информацию о игроке другим способом, если это необходимо

            if (UnitZ.playerSave != null && component != null)
            {
                string hasKey = userID + "_" + Application.loadedLevelName + "_" + characterKey + "_" + userName;
                UnitZ.playerSave.InstantiateCharacter(component, hasKey);
            }
            if (playerID == UnitZ.gameManager.PlayerID)
            {
                playingCharacter = component;
            }
            Debug.Log("Instantiate character : " + component.CharacterKey);
            MouseLock.MouseLocked = true;
            return true;
        }
        return false;
    }

    private void OnGUI()
    {
        if ((bool)skin)
        {
            GUI.skin = skin;
        }
        if (!DisabledSpawn && (bool)UnitZ.gameManager && UnitZ.gameManager.IsPlaying && playingCharacter == null)
        {
            BlackIns.SetActive(false);
            RequestSpawnPlayer();
            BlackIns.SetActive(true);
        }
    }
}