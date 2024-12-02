using UnityEngine;
using UnityEngine.UI;

public class GUIItemCollector : MonoBehaviour
{
	public ItemCollector Item;

	public Image Icon;

	public Text Num;

	public bool UpdateEnabled;

	[HideInInspector]
	public CharacterInventory currentInventory;

	public string Type = string.Empty;

	private void Start()
	{
	}

	public void SetItemCollector(ItemCollector item)
	{
		Item = item;
	}

	private void FixedUpdate()
	{
		if (UpdateEnabled == (bool)Num)
		{
			return;
		}
		if (Item != null && Num != null && Icon != Num)
		{
			Icon.enabled = true;
			Num.enabled = true;
			Num.text = Item.Num.ToString();
			Icon.sprite = Item.Item.ImageSprite;
		}
		if (Item == null || (Item != null && Item.Num <= 0))
		{
			Icon.enabled = false;
			if (Num != null)
			{
				Num.enabled = false;
			}
		}
	}
}
