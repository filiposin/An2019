using UnityEngine;
using UnityEngine.UI;

public class TooltipDetails : TooltipInstance
{
	public Text Header;

	public Text Content;

	private static TooltipDetails tooltip;

	public static TooltipDetails Instance
	{
		get
		{
			if (tooltip == null)
			{
				tooltip = Object.FindObjectOfType<TooltipDetails>();
			}
			return tooltip;
		}
	}

	private void Start()
	{
		tooltip = this;
		HideTooltip();
	}

	public override void ShowTooltip(ItemCollector itemCol, Vector3 pos)
	{
		if (itemCol != null && !(itemCol.Item == null) && !MouseLock.MouseLocked)
		{
			if ((bool)Header)
			{
				Header.text = itemCol.Item.ItemName;
			}
			if ((bool)Content)
			{
				Content.text = itemCol.Item.Description;
			}
			if (TooltipUsing.Instance.gameObject.activeSelf)
			{
				hover = false;
			}
			base.ShowTooltip(itemCol, pos);
		}
	}
}
