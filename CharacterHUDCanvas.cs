using UnityEngine;

public class CharacterHUDCanvas : PanelsManager
{
	public GUIIStockItemLoader SecondItemLoader;

	private CharacterLiving living;

	public GameObject Canvas;

	private void Start()
	{
	}

	private void Awake()
	{
		if (Pages.Length > 0)
		{
			ClosePanel(Pages[0]);
		}
	}

	private void InputController()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			MouseLock.MouseLocked = !TogglePanelByName("Inventory");
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			MouseLock.MouseLocked = !TogglePanelByName("InGameMenu");
		}
		if (IsPanelOpened("InGameMenu") || IsPanelOpened("Inventory") || IsPanelOpened("Craft") || IsPanelOpened("InventoryTrade"))
		{
			MouseLock.MouseLocked = false;
		}
		else
		{
			MouseLock.MouseLocked = true;
		}
	}

	private void Update()
	{
		if (UnitZ.playerManager == null || Canvas == null)
		{
			return;
		}
		if (UnitZ.playerManager.playingCharacter == null)
		{
			living = null;
			Canvas.gameObject.SetActive(false);
			return;
		}
		Canvas.gameObject.SetActive(true);
		InputController();
		if (living == null)
		{
			living = UnitZ.playerManager.playingCharacter.GetComponent<CharacterLiving>();
		}
	}

	public void OpenSecondInventory(CharacterInventory inventory, string type)
	{
		if (IsPanelOpened("InventoryTrade"))
		{
			ClosePanelByName("InventoryTrade");
			return;
		}
		SecondItemLoader.OpenInventory(inventory, type);
		OpenPanelByName("InventoryTrade");
	}

	public void CloseSecondInventory()
	{
		ClosePanelByName("InventoryTrade");
	}
}