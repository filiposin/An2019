public class GUIIStockItemLoader : GUIItemLoader
{
	public DropStockArea dropArea;

	private void Start()
	{
		dropArea = GetComponent<DropStockArea>();
	}

	public void OpenInventory(CharacterInventory inventory, string type)
	{
		currentInventory = inventory;
		Type = type;
		UpdateGUIInventory();
	}

	private void Update()
	{
		if (!(currentInventory == null))
		{
			UpdateFunction();
		}
	}
}
