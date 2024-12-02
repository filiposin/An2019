public class GUIPlayerItemLoader : GUIItemLoader
{
	public DropStockArea dropArea;

	private void Start()
	{
		dropArea = GetComponent<DropStockArea>();
	}

	private void Update()
	{
		if (!(UnitZ.playerManager == null) && !(UnitZ.playerManager.playingCharacter == null))
		{
			currentInventory = UnitZ.playerManager.playingCharacter.inventory;
			UpdateFunction();
		}
	}
}
