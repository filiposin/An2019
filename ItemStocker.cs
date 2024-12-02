using UnityEngine;

public class ItemStocker : ObjectTrigger
{
    public string StockID = "mybox";

    public CharacterInventory inventory;

    private int updateTemp;

    private bool stockLoaded;

    private ObjectPlacing placing;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        inventory = GetComponent<CharacterInventory>();
        placing = GetComponent<ObjectPlacing>();
        if (placing != null)
        {
            StockID = placing.ItemUID;
        }
    }

    public void OpenStock()
    {
        LoadStock();
    }

    public override void OnExit()
    {
        UnitZ.Hud.CloseSecondInventory();
        base.OnExit();
    }

    public void SaveItem(ItemCollector item)
    {
        inventory.AddItemByCollector(item);
    }

    private void Update()
    {
        if (inventory != null)
        {
            if (updateTemp != inventory.UpdateCount && stockLoaded)
            {
                SaveStock();
                updateTemp = inventory.UpdateCount;
            }
            UpdateFunction();
        }
    }

    public override void Pickup(CharacterSystem character)
    {
        if (character != null && character.IsMine)
        {
            OpenStock();
            UnitZ.Hud.OpenSecondInventory(inventory, "Stock");
        }
        base.Pickup(character);
    }

    private void SaveStock()
    {
        if (inventory != null)
        {
            string itemDataText = inventory.GetItemDataText();
            PlayerPrefs.SetString(StockID, itemDataText);
        }
    }

    private void LoadStock()
    {
        if (inventory != null)
        {
            if (PlayerPrefs.HasKey(StockID))
            {
                inventory.SetItemsFromText(PlayerPrefs.GetString(StockID));
                stockLoaded = true;
            }
            else
            {
                stockLoaded = true;
                SaveStock();
            }
        }
    }
}
