using UnityEngine;

public class StyleManager : MonoBehaviour
{
	public GUISkin[] Skins;

	private void Start()
	{
	}

	public GUISkin GetSkin(int i)
	{
		if (i < 0)
		{
			i = 0;
		}
		if (i < Skins.Length)
		{
			return Skins[i];
		}
		return null;
	}

	private void Update()
	{
	}
}
