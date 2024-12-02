using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, IEventSystemHandler
{
	public ItemCollector Item;

	public string Type = "inventory";

	private Vector3 pointerPosition;

	public void Start()
	{
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		pointerPosition = new Vector3(eventData.position.x, eventData.position.y - 18f, 0f);
		Select(pointerPosition);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GUIItemCollector component = GetComponent<GUIItemCollector>();
		if ((bool)component)
		{
			Item = component.Item;
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
	}

	public void OnPointerExit(PointerEventData eventData)
	{
	}

	public void OnDeselect(BaseEventData eventData)
	{
	}

	private void Select(Vector3 position)
	{
		GUIItemCollector component = GetComponent<GUIItemCollector>();
		if ((bool)component)
		{
			Item = component.Item;
		}
		if (Item != null)
		{
			TooltipUsing.Instance.ShowTooltip(Item, position, Type);
		}
		TooltipDetails.Instance.HideTooltip();
	}
}
