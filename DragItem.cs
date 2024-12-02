using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	public bool dragOnSurfaces = true;

	public GUIItemCollector GUIItem;

	private GameObject m_DraggingIcon;

	private RectTransform m_DraggingPlane;

	private void Start()
	{
		if (GUIItem == null)
		{
			GUIItem = GetComponent<GUIItemCollector>();
		}
		if (GUIItem != null && GUIItem.Item != null)
		{
			GUIItem.Icon.sprite = GUIItem.Item.Item.ImageSprite;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Canvas canvas = FindInParents<Canvas>(base.gameObject);
		if (!(canvas == null))
		{
			m_DraggingIcon = new GameObject("icon");
			m_DraggingIcon.transform.SetParent(canvas.transform, false);
			m_DraggingIcon.transform.SetAsLastSibling();
			Image image = m_DraggingIcon.AddComponent<Image>();
			CanvasGroup canvasGroup = m_DraggingIcon.AddComponent<CanvasGroup>();
			canvasGroup.blocksRaycasts = false;
			image.color = GetComponent<Image>().color;
			image.sprite = GetComponent<Image>().sprite;
			image.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
			if (dragOnSurfaces)
			{
				m_DraggingPlane = base.transform as RectTransform;
			}
			else
			{
				m_DraggingPlane = canvas.transform as RectTransform;
			}
			SetDraggedPosition(eventData);
		}
	}

	public void OnDrag(PointerEventData data)
	{
		if (m_DraggingIcon != null)
		{
			SetDraggedPosition(data);
		}
	}

	private void SetDraggedPosition(PointerEventData data)
	{
		base.gameObject.SendMessage("Draging", SendMessageOptions.DontRequireReceiver);
		if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
		{
			m_DraggingPlane = data.pointerEnter.transform as RectTransform;
		}
		RectTransform component = m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 worldPoint;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out worldPoint))
		{
			component.position = worldPoint;
			component.rotation = m_DraggingPlane.rotation;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (m_DraggingIcon != null)
		{
			Object.Destroy(m_DraggingIcon);
		}
	}

	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return (T)null;
		}
		T component = go.GetComponent<T>();
		if (component != null)
		{
			return component;
		}
		Transform parent = go.transform.parent;
		while (parent != null && component == null)
		{
			component = parent.gameObject.GetComponent<T>();
			parent = parent.parent;
		}
		return component;
	}
}
