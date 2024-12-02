using UnityEngine;
using UnityEngine.UI;

public class CharacterBadge : MonoBehaviour
{
	public RawImage GUIImage;

	public Text GUIName;

	public Text GUIType;

	[HideInInspector]
	public CharacterSaveData CharacterData;

	[HideInInspector]
	public int Index;

	[HideInInspector]
	public CharacterCreatorCanvas CharacterCreatorS;

	private void Start()
	{
	}

	public void Delete()
	{
		if ((bool)CharacterCreatorS)
		{
			CharacterCreatorS.RemoveCharacter(Index);
		}
	}

	public void PlayThisCharacter()
	{
		if ((bool)CharacterCreatorS && CharacterData.PlayerName != string.Empty)
		{
			CharacterCreatorS.SelectCharacter(CharacterData);
		}
	}
}
