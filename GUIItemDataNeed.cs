using UnityEngine;

public class GUIItemDataNeed : GUIItemData
{
	public Color Ready = Color.green;

	public Color NotReady = Color.red;

	public CharacterInventory Inventory;

	public int Need;

	private void Update()
	{
		if (!Item)
		{
			return;
		}
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
		if (!Num)
		{
			return;
		}
		Num.enabled = true;
		if ((bool)Inventory)
		{
			if (Inventory.CheckItem(Item, Need))
			{
				Num.text = "X " + Need + " Ready";
				Num.color = Ready;
			}
			else
			{
				Num.text = "X " + Need;
				Num.color = NotReady;
			}
		}
	}
}
