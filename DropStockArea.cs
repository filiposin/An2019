using UnityEngine;
using UnityEngine.EventSystems;

public class DropStockArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	[HideInInspector]
	public CharacterInventory inventory;

	public GUIItemLoader loader;

	public bool IsMine;

	public string Type;

	private GUIItemCollector itemDrop;

	public ItemData Currency;

	public void Start()
	{
	}

	public void Trade(int number)
	{
		if (loader == null || itemDrop == null || !(itemDrop != null) || !(inventory != null) || itemDrop.Item == null)
		{
			return;
		}
		if (number <= 0)
		{
			number = 1;
		}
		if (!(inventory != itemDrop.currentInventory))
		{
			return;
		}
		// Removed TooltipTrade related code
	}

	public void OnDrop(PointerEventData data)
	{
		itemDrop = GetDropSprite(data);
		if (!(loader == null) && !(itemDrop == null))
		{
			inventory = loader.currentInventory;
			if (inventory != itemDrop.currentInventory)
			{
				// Removed TooltipTrade related code
			}
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{
	}

	public void OnPointerExit(PointerEventData data)
	{
	}

	private GUIItemCollector GetDropSprite(PointerEventData data)
	{
		GameObject pointerDrag = data.pointerDrag;
		if (pointerDrag == null)
		{
			return null;
		}
		if ((bool)pointerDrag.GetComponent<GUIItemCollector>())
		{
			return pointerDrag.GetComponent<GUIItemCollector>();
		}
		return null;
	}
}