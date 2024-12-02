using UnityEngine;

public class CharactersBadgesRefresher : MonoBehaviour
{
	public CharacterCreatorCanvas characterCreator;

	private void Start()
	{
	}

	private void OnEnable()
	{
		if ((bool)characterCreator)
		{
			StartCoroutine(characterCreator.LoadCharacters());
		}
	}
}
