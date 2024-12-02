using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    public List<PlayerSaveData> LoadedData = new List<PlayerSaveData>();

    [HideInInspector]
    public CharacterSystem MainCharacter;

    private void Start()
    {
        if (!(UnitZ.gameManager == null) && UnitZ.gameManager.UserID == string.Empty)
        {
            Debug.Log("UID is not assigned");
            UnitZ.gameManager.UserID = PlayerPrefs.GetString("UID");
            if (UnitZ.gameManager.UserID == string.Empty)
            {
                UnitZ.gameManager.UserID = GetUniqueID();
                PlayerPrefs.SetString("UID", UnitZ.gameManager.UserID);
                Debug.Log("UID is generated " + UnitZ.gameManager.UserID);
            }
            else
            {
                Debug.Log("UID is " + UnitZ.gameManager.UserID);
            }
        }
    }

    public void DeleteSave()
    {
        if (!(UnitZ.gameManager == null))
        {
            DeleteSave(UnitZ.gameManager.UserID, UnitZ.gameManager.CharacterKey, UnitZ.gameManager.UserName);
        }
    }

    public void DeleteSave(string userID, string characterKey, string userName)
    {
        if (!(userID == string.Empty))
        {
            PlayersRegister(userID);
            PlayerSaveData playersave = default(PlayerSaveData);
            playersave.UID = userID;
            playersave.PlayerName = userName;
            playersave.CharacterKey = characterKey;
            playersave.ItemData = string.Empty;
            playersave.EquipData = string.Empty;
            playersave.FPSItemIndex = 0;
            playersave.Position = string.Empty;
            playersave.LevelName = string.Empty;
            playersave.Food = 0;
            playersave.Water = 0;
            playersave.Health = 0;
            WriteData(playersave);
        }
    }

    public void SavePlayer(CharacterSystem character)
    {
        if (!(UnitZ.gameManager == null) && !(character == null))
        {
            PlayersRegister(UnitZ.gameManager.UserID);
            string userID = UnitZ.gameManager.UserID;
            PlayerSaveData playersave = default(PlayerSaveData);
            playersave.UID = userID;
            playersave.PlayerName = UnitZ.gameManager.UserName;
            playersave.CharacterKey = character.CharacterKey;
            playersave.ItemData = character.inventory.GetItemDataText();
            playersave.EquipData = character.inventory.GenStickerTextData();
            playersave.FPSItemIndex = character.inventory.GetCollectorFPSindex();
            playersave.Position = character.transform.position.x + "," + character.transform.position.y + "," + character.transform.position.z;
            playersave.LevelName = Application.loadedLevelName;
            playersave.Health = character.HP;
            CharacterLiving component = character.GetComponent<CharacterLiving>();
            if ((bool)component)
            {
                playersave.Food = component.Hungry;
                playersave.Water = component.Water;
            }
            WriteData(playersave);
        }
    }

    public void LoadPlayer(CharacterSystem character)
    {
        if (!(UnitZ.gameManager == null) && !(character == null))
        {
            MainCharacter = character;
            string text = UnitZ.gameManager.UserID + "_" + Application.loadedLevelName + "_" + character.CharacterKey + "_" + UnitZ.gameManager.UserName;
            PlayerSaveData playerSaveData = default(PlayerSaveData);
            playerSaveData = ReadData(text);
            ApplyPlayerData(playerSaveData, character);
        }
    }

    public void InstantiateCharacter(CharacterSystem character, string hasKey)
    {
        Debug.Log("Instantiate Character save : " + hasKey);
        if (!(character == null))
        {
            PlayerSaveData playerSaveData = default(PlayerSaveData);
            playerSaveData = ReadData(hasKey);
            ApplyPlayerData(playerSaveData, character);
        }
    }

    public void WriteData(PlayerSaveData playersave)
    {
        string text = playersave.UID + "_" + Application.loadedLevelName + "_" + playersave.CharacterKey + "_" + playersave.PlayerName;
        PlayerPrefs.SetString("PLAYER_" + text, playersave.UID);
        PlayerPrefs.SetString("NAME_" + text, playersave.PlayerName);
        PlayerPrefs.SetString("CHARACTERKEY_" + text, playersave.CharacterKey);
        PlayerPrefs.SetString("ITEM_" + text, playersave.ItemData);
        PlayerPrefs.SetString("EQUIP_" + text, playersave.EquipData);
        PlayerPrefs.SetInt("FPSINDEX" + text, playersave.FPSItemIndex);
        PlayerPrefs.SetString("POS" + text, playersave.Position);
        PlayerPrefs.SetString("LEVELNAME" + text, playersave.LevelName);
        PlayerPrefs.SetInt(" FOOD" + text, playersave.Food);
        PlayerPrefs.SetInt("WATER" + text, playersave.Water);
        PlayerPrefs.SetInt("HEALTH" + text, playersave.Health);
    }

    public PlayerSaveData ReadData(string hasKey)
    {
        PlayerSaveData result = default(PlayerSaveData);
        result.UID = PlayerPrefs.GetString("PLAYER_" + hasKey);
        result.PlayerName = PlayerPrefs.GetString("NAME_" + hasKey);
        result.CharacterKey = PlayerPrefs.GetString("CHARACTERKEY_" + hasKey);
        result.ItemData = PlayerPrefs.GetString("ITEM_" + hasKey);
        result.EquipData = PlayerPrefs.GetString("EQUIP_" + hasKey);
        result.FPSItemIndex = PlayerPrefs.GetInt("FPSINDEX" + hasKey);
        result.Position = PlayerPrefs.GetString("POS" + hasKey);
        result.LevelName = PlayerPrefs.GetString("LEVELNAME" + hasKey);
        result.Food = PlayerPrefs.GetInt("FOOD" + hasKey);
        result.Water = PlayerPrefs.GetInt("WATER" + hasKey);
        result.Health = PlayerPrefs.GetInt("HEALTH" + hasKey);
        return result;
    }

    public string PlayerSaveDataText(PlayerSaveData playersave)
    {
        return playersave.UID + "^" + playersave.PlayerName + "^" + playersave.ItemData + "^" + playersave.EquipData + "^" + playersave.FPSItemIndex + "^" + playersave.Position + "^" + playersave.LevelName + "^" + playersave.Food + "^" + playersave.Water + "^" + playersave.Health + "^" + playersave.CharacterKey;
    }

    public PlayerSaveData GetSaveDataFromText(string dataText)
    {
        string[] array = dataText.Split("^"[0]);
        PlayerSaveData result = default(PlayerSaveData);
        result.UID = array[0];
        result.PlayerName = array[1];
        result.ItemData = array[2];
        result.EquipData = array[3];
        int.TryParse(array[4], out result.FPSItemIndex);
        result.Position = array[5];
        result.LevelName = array[6];
        int.TryParse(array[7], out result.Food);
        int.TryParse(array[8], out result.Water);
        int.TryParse(array[9], out result.Health);
        result.CharacterKey = array[10];
        return result;
    }

    private void ApplyPlayerData(PlayerSaveData playersave)
    {
        ApplyPlayerData(playersave, MainCharacter);
    }

    private void ApplyPlayerData(PlayerSaveData playersave, CharacterSystem character)
    {
        if (!character || !character.inventory)
        {
            return;
        }
        character.inventory.SetupStarterItem();
        character.inventory.SetItemsFromText(playersave.ItemData);
        if (character.inventory.Items.Count > playersave.FPSItemIndex)
        {
            character.inventory.EquipItemByCollector(character.inventory.Items[playersave.FPSItemIndex]);
        }
        string[] array = playersave.Position.Split(","[0]);
        if (array.Length > 2)
        {
            Vector3 zero = Vector3.zero;
            float.TryParse(array[0], out zero.x);
            float.TryParse(array[1], out zero.y);
            float.TryParse(array[2], out zero.z);
            character.transform.position = zero;
        }
        character.HP = playersave.Health;
        CharacterLiving component = character.GetComponent<CharacterLiving>();
        if ((bool)component)
        {
            component.Hungry = playersave.Food;
            component.Water = playersave.Water;
        }
        if (playersave.Food <= 0 && playersave.Water <= 0 && playersave.Health <= 0)
        {
            character.HP = character.HPmax;
            if ((bool)component)
            {
                component.Hungry = component.HungryMax;
                component.Water = component.WaterMax;
            }
        }
    }

    public void ReceiveDataAndApply(string datatext, CharacterSystem character)
    {
        PlayerSaveData playerSaveData = default(PlayerSaveData);
        playerSaveData = GetSaveDataFromText(datatext);
        ApplyPlayerData(playerSaveData, character);
    }

    public bool PlayersRegister(string uid)
    {
        string @string = PlayerPrefs.GetString("PLAYERS");
        string[] array = @string.Split(","[0]);
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != string.Empty && uid == array[i])
            {
                return false;
            }
        }
        @string = @string + uid + ",";
        PlayerPrefs.SetString("PLAYERS", @string);
        return true;
    }

    private void Update()
    {
    }

    public string GetUniqueID()
    {
        System.Random random = new System.Random();
        DateTime dateTime = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
        double totalSeconds = (DateTime.UtcNow - dateTime).TotalSeconds;
        return string.Format("{0:X}", Convert.ToInt32(totalSeconds)) + "-" + string.Format("{0:X}", Convert.ToInt32(Time.time * 1000000f)) + "-" + string.Format("{0:X}", random.Next(1000000000));
    }

    public bool SaveCharacter(CharacterSaveData character)
    {
        string characterKey = character.CharacterKey;
        if (!PlayerPrefs.HasKey("CHARACTER_" + characterKey))
        {
            PlayerPrefs.SetString("CHARACTER_" + characterKey, character.PlayerName);
            PlayerPrefs.SetInt("CharacterIndex_" + characterKey, character.CharacterIndex);
            return true;
        }
        return false;
    }

    public CreateResult CreateCharacter(CharacterSaveData character)
    {
        CreateResult result = default(CreateResult);
        string @string = PlayerPrefs.GetString("CHARACTERS");
        character.CharacterKey = GetUniqueID();
        result.CharacterKey = character.CharacterKey;
        result.IsSuccess = false;
        if (SaveCharacter(character))
        {
            @string = @string + "," + character.CharacterKey;
            PlayerPrefs.SetString("CHARACTERS", @string);
            result.IsSuccess = true;
        }
        return result;
    }

    public void RemoveCharacter(CharacterSaveData character)
    {
        string[] array = PlayerPrefs.GetString("CHARACTERS").Split(","[0]);
        string text = string.Empty;
        string characterKey = character.CharacterKey;
        if (array.Length <= 0)
        {
            return;
        }
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != string.Empty && array[i] != characterKey)
            {
                text = text + "," + array[i];
            }
        }
        PlayerPrefs.SetString("CHARACTERS", text);
        PlayerPrefs.DeleteKey("CHARACTER_" + characterKey);
        PlayerPrefs.DeleteKey("CharacterIndex_" + characterKey);
        if ((bool)UnitZ.gameManager)
        {
            DeleteSave(UnitZ.gameManager.UserID, characterKey, character.PlayerName);
        }
    }

    public CharacterSaveData LoadCharacter(string key)
    {
        CharacterSaveData result = default(CharacterSaveData);
        if (PlayerPrefs.HasKey("CHARACTER_" + key))
        {
            result.PlayerName = PlayerPrefs.GetString("CHARACTER_" + key);
            result.CharacterIndex = PlayerPrefs.GetInt("CharacterIndex_" + key);
            result.CharacterKey = key;
        }
        return result;
    }

    public CharacterSaveData[] LoadAllCharacters()
    {
        string[] array = PlayerPrefs.GetString("CHARACTERS").Split(","[0]);
        List<CharacterSaveData> list = new List<CharacterSaveData>();
        if (array.Length > 0)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != string.Empty)
                {
                    CharacterSaveData item = LoadCharacter(array[i]);
                    if (item.PlayerName != string.Empty)
                    {
                        list.Add(item);
                    }
                }
            }
            return list.ToArray();
        }
        return null;
    }
}