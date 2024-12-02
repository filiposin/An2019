using UnityEngine;

public class PlacingArea : MonoBehaviour
{
	public bool Snap;

	public string[] KeyPair = new string[1] { string.Empty };

	public bool KeyPairChecker(string[] keys)
	{
		if (keys.Length <= 0 && KeyPair.Length <= 0)
		{
			return true;
		}
		for (int i = 0; i < KeyPair.Length; i++)
		{
			for (int j = 0; j < keys.Length; j++)
			{
				if (keys[j] == KeyPair[i])
				{
					return true;
				}
			}
		}
		return false;
	}

	private void Start()
	{
		if (KeyPair.Length <= 0)
		{
			KeyPair = new string[1] { string.Empty };
		}
	}

	private void Update()
	{
	}
}
