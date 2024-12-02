using UnityEngine;

public class UpdateInventory : MonoBehaviour
{
	public GUIPlayerItemLoader inventory;

	private void Start()
	{
		inventory = GetComponent<GUIPlayerItemLoader>();
		inventory.enabled = false;
		inventory.enabled = true;
	}
}
