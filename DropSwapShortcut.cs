using UnityEngine;
using UnityEngine.EventSystems;

public class DropSwapShortcut : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	private GUIItemCollector guiItem;

	public void Start()
	{
		guiItem = GetComponent<GUIItemCollector>();
	}

	public void OnDrop(PointerEventData data)
	{
		if ((bool)UnitZ.playerManager && (bool)UnitZ.playerManager.playingCharacter && GetDropSprite(data) != null)
		{
			UnitZ.playerManager.playingCharacter.inventory.SwarpShortcut(guiItem.Item, GetDropSprite(data).Item);
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
		GUIItemCollector component = pointerDrag.GetComponent<GUIItemCollector>();
		if (component == null)
		{
			return null;
		}
		return component;
	}
}
