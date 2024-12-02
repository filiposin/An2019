using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipInstance : MonoBehaviour
{
	protected PanelsManager panelsManager;

	protected ItemCollector Item;

	public bool hover;

	private void Awake()
	{
		panelsManager = GetComponent<PanelsManager>();
	}

	private void Start()
	{
		panelsManager = GetComponent<PanelsManager>();
		HideTooltip();
	}

	public virtual IEnumerator OnHover(PointerEventData eventData, ItemCollector itemCol)
	{
		while (hover && !MouseLock.MouseLocked)
		{
			ShowTooltip(itemCol, new Vector3(eventData.position.x, eventData.position.y - 18f, 0f));
			yield return new WaitForEndOfFrame();
		}
		HideTooltip();
	}

	private void Update()
	{
		if (MouseLock.MouseLocked)
		{
			base.gameObject.SetActive(false);
		}
	}

	public virtual void ShowTooltip(ItemCollector itemCol, Vector3 pos)
	{
		if (itemCol != null && !MouseLock.MouseLocked)
		{
			Item = itemCol;
			base.transform.position = pos;
			base.gameObject.SetActive(true);
		}
	}

	public virtual void ShowTooltip(ItemCollector itemCol, Vector3 pos, string type)
	{
		if (itemCol != null)
		{
			if ((bool)panelsManager)
			{
				panelsManager.DisableAllPanels();
				panelsManager.OpenPanelByName(type);
			}
			Item = itemCol;
			base.transform.position = pos;
			base.gameObject.SetActive(true);
		}
	}

	public void HideTooltip()
	{
		base.gameObject.SetActive(false);
	}
}
