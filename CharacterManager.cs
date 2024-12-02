using UnityEngine;

public class CharacterManager : MonoBehaviour
{
	public int CharacterIndex;

	public CharacterPreset[] CharacterPresets;

	private CharacterSaveData characterCreate;

	public CharacterSaveData SelectedCharacter;

	private void Start()
	{
	}

	public bool CreateCharacter(string characterName)
	{
		if ((bool)UnitZ.gameManager)
		{
			CreateResult createResult = default(CreateResult);
			if (characterName != string.Empty)
			{
				createResult = SaveNewCharacter(characterName);
				if (createResult.IsSuccess)
				{
					UnitZ.gameManager.UserName = characterName;
					UnitZ.gameManager.CharacterKey = createResult.CharacterKey;
					return true;
				}
			}
		}
		return false;
	}

	public void SetCharacter()
	{
		CharacterIndex = SelectedCharacter.CharacterIndex;
		if (CharacterPresets.Length > 0)
		{
			if (CharacterIndex >= CharacterPresets.Length)
			{
				CharacterIndex = CharacterPresets.Length - 1;
			}
			if (CharacterIndex < 0)
			{
				CharacterIndex = 0;
			}
			if ((bool)UnitZ.gameManager)
			{
				UnitZ.gameManager.UserName = SelectedCharacter.PlayerName;
				UnitZ.gameManager.CharacterKey = SelectedCharacter.CharacterKey;
			}
		}
	}

	public void SetupCharacter(CharacterSaveData character)
	{
		SelectedCharacter = character;
		CharacterIndex = SelectedCharacter.CharacterIndex;
		if (CharacterPresets.Length > 0)
		{
			if (CharacterIndex >= CharacterPresets.Length)
			{
				CharacterIndex = CharacterPresets.Length - 1;
			}
			if (CharacterIndex < 0)
			{
				CharacterIndex = 0;
			}
			if ((bool)UnitZ.gameManager)
			{
				UnitZ.gameManager.UserName = SelectedCharacter.PlayerName;
				UnitZ.gameManager.CharacterKey = SelectedCharacter.CharacterKey;
			}
		}
	}

	public void SelectCreateCharacter(int index)
	{
		characterCreate = default(CharacterSaveData);
		characterCreate.CharacterIndex = index;
	}

	public CreateResult SaveNewCharacter(string characterName)
	{
		CreateResult result = default(CreateResult);
		result.IsSuccess = false;
		if (characterName != string.Empty && (bool)UnitZ.gameManager && (bool)UnitZ.playerSave)
		{
			characterCreate.PlayerName = characterName;
			return UnitZ.playerSave.CreateCharacter(characterCreate);
		}
		return result;
	}

	public void RemoveCharacter(CharacterSaveData character)
	{
		if ((bool)UnitZ.playerSave)
		{
			UnitZ.playerSave.RemoveCharacter(character);
		}
	}
}
