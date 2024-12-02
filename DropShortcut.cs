using UnityEngine;
using UnityEngine.EventSystems;

public class DropShortcut : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	public int ShortcutIndex;

	public KeyCode Key;

	private GUIItemCollector guiItem;

	public void Start()
	{
		guiItem = GetComponent<GUIItemCollector>();
	}

	public void OnDrop(PointerEventData data)
	{
		if (!(UnitZ.playerManager == null) && !(UnitZ.playerManager.playingCharacter == null) && !(guiItem == null) && data != null)
		{
			ItemCollector itemDrop = GetItemDrop(data);
			if (itemDrop != null)
			{
				UnitZ.playerManager.playingCharacter.inventory.SwarpShortcut(itemDrop, guiItem.Item);
				itemDrop.Shortcut = ShortcutIndex;
				guiItem.SetItemCollector(itemDrop);
			}
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{
	}

	public void OnPointerExit(PointerEventData data)
	{
	}

	public void UseItem()
	{
		if (guiItem != null && guiItem.Item != null && UnitZ.playerManager != null && UnitZ.playerManager.playingCharacter != null)
		{
			UnitZ.playerManager.playingCharacter.inventory.EquipItemByCollector(guiItem.Item);
		}
	}

	public void LateUpdate()
	{
		if (!(guiItem == null) && !(UnitZ.playerManager == null) && !(UnitZ.playerManager.playingCharacter == null) && !(UnitZ.playerManager.playingCharacter.inventory == null))
		{
			ItemCollector itemByShortCutIndex = UnitZ.playerManager.playingCharacter.inventory.GetItemByShortCutIndex(ShortcutIndex);
			guiItem.SetItemCollector(itemByShortCutIndex);
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown(Key))
		{
			UseItem();
		}
	}

	private ItemCollector GetItemDrop(PointerEventData data)
	{
		GameObject pointerDrag = data.pointerDrag;
		if (pointerDrag == null)
		{
			return null;
		}
		if ((bool)pointerDrag.GetComponent<DragItem>())
		{
			return pointerDrag.GetComponent<DragItem>().GUIItem.Item;
		}
		if ((bool)pointerDrag.GetComponent<GUIItemCollector>())
		{
			return pointerDrag.GetComponent<GUIItemCollector>().Item;
		}
		return null;
	}
}
