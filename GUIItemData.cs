using UnityEngine;
using UnityEngine.UI;

public class GUIItemData : MonoBehaviour
{
	public Image Icon;

	public Text Name;

	public Text Num;

	public ItemData Item;

	private void Start()
	{
		if ((bool)Icon)
		{
			Icon.enabled = false;
		}
		if ((bool)Name)
		{
			Name.enabled = false;
		}
		if ((bool)Num)
		{
			Num.enabled = false;
		}
	}

	private void Update()
	{
		if ((bool)Item)
		{
			if ((bool)Icon)
			{
				Icon.enabled = true;
				Icon.sprite = Item.ImageSprite;
			}
			if ((bool)Name)
			{
				Name.enabled = true;
				Name.text = Item.ItemName;
			}
			if ((bool)Num)
			{
				Num.enabled = true;
			}
		}
	}
}
