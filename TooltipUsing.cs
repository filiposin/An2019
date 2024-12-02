using UnityEngine;

public class TooltipUsing : TooltipInstance
{
	private static TooltipInstance tooltip;

	private bool interactive;

	private bool onpress;

	public static TooltipInstance Instance
	{
		get
		{
			if (tooltip == null)
			{
				tooltip = Object.FindObjectOfType<TooltipUsing>();
			}
			return tooltip;
		}
	}

	private void Start()
	{
		tooltip = this;
		interactive = false;
		HideTooltip();
	}

	private void LateUpdate()
	{
		if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
		{
			onpress = true;
		}
		else if (onpress || MouseLock.MouseLocked)
		{
			if (!interactive)
			{
				HideTooltip();
			}
			interactive = false;
			onpress = false;
		}
	}

	public void Use()
	{
		interactive = true;
		if (UnitZ.playerManager != null && UnitZ.playerManager.playingCharacter != null && Item != null)
		{
			UnitZ.playerManager.playingCharacter.inventory.EquipItemByCollector(Item);
			HideTooltip();
		}
	}

	public void Unequip()
	{
		interactive = true;
		if (UnitZ.playerManager != null && UnitZ.playerManager.playingCharacter != null && Item != null)
		{
			UnitZ.playerManager.playingCharacter.inventory.RemoveEquipItemByCollector(Item);
		}
		HideTooltip();
	}

	public void Delete()
	{
		interactive = true;
		if (UnitZ.playerManager != null && UnitZ.playerManager.playingCharacter != null && Item != null)
		{
			UnitZ.playerManager.playingCharacter.inventory.DropItemByCollector(Item, Item.Num);
		}
		HideTooltip();
	}

	public override void ShowTooltip(ItemCollector itemCol, Vector3 pos)
	{
		onpress = false;
		base.ShowTooltip(itemCol, pos);
	}

	public override void ShowTooltip(ItemCollector itemCol, Vector3 pos, string type)
	{
		onpress = false;
		base.ShowTooltip(itemCol, pos, type);
	}
}
