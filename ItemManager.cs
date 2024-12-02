using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemData[] ItemsList;

    public string Suffix = "UZ";

    private void Awake()
    {
        for (int i = 0; i < ItemsList.Length; i++)
        {
            ItemsList[i].ItemID = Suffix + i;
            if (!ItemsList[i].ItemFPS)
            {
                continue;
            }
            FPSItemEquipment component = ItemsList[i].ItemFPS.GetComponent<FPSItemEquipment>();
            if ((bool)component)
            {
                component.ItemID = ItemsList[i].ItemID;
            }
            FPSItemPlacing component2 = ItemsList[i].ItemFPS.GetComponent<FPSItemPlacing>();
            if (!component2 || !(component2.Item != null))
            {
                continue;
            }
            ObjectSpawn component3 = component2.Item.GetComponent<ObjectSpawn>();
            if (!component3)
            {
                continue;
            }
            component3.ItemID = ItemsList[i].ItemID;
            if ((bool)component3.Item)
            {
                ObjectPlacing component4 = component3.Item.GetComponent<ObjectPlacing>();
                if ((bool)component4)
                {
                    component4.ItemID = ItemsList[i].ItemID;
                }
            }
        }
    }

    public ItemData GetItem(int index)
    {
        if (index < ItemsList.Length && index >= 0)
        {
            return ItemsList[index];
        }
        return null;
    }

    public int GetIndexByID(string itemid)
    {
        for (int i = 0; i < ItemsList.Length; i++)
        {
            if (itemid == ItemsList[i].ItemID)
            {
                return i;
            }
        }
        return -1;
    }

    public int GetIndexByName(string itemname)
    {
        for (int i = 0; i < ItemsList.Length; i++)
        {
            if (itemname == ItemsList[i].ItemName)
            {
                return i;
            }
        }
        return -1;
    }

    public ItemData CloneItemData(ItemData item)
    {
        for (int i = 0; i < ItemsList.Length; i++)
        {
            if (item.ItemID == ItemsList[i].ItemID)
            {
                return ItemsList[i];
            }
        }
        return null;
    }

    public int GetItemIndexByItemData(ItemData item)
    {
        for (int i = 0; i < ItemsList.Length; i++)
        {
            if (item.ItemID == ItemsList[i].ItemID)
            {
                return i;
            }
        }
        return -1;
    }

    public ItemData CloneItemDataByIndex(string itemID)
    {
        for (int i = 0; i < ItemsList.Length; i++)
        {
            if (ItemsList[i].ItemID == itemID)
            {
                return ItemsList[i];
            }
        }
        return null;
    }

    public ItemData GetItemDataByID(string itemid)
    {
        for (int i = 0; i < ItemsList.Length; i++)
        {
            if (itemid == ItemsList[i].ItemID)
            {
                return ItemsList[i];
            }
        }
        return null;
    }

    public ItemData GetItemDataByName(string itemname)
    {
        for (int i = 0; i < ItemsList.Length; i++)
        {
            if (itemname == ItemsList[i].ItemName)
            {
                return ItemsList[i];
            }
        }
        return null;
    }

    public void PlacingObject(string itemid, Vector3 position, Vector3 normal)
    {
        placingObject(itemid, position, normal);
    }

    private void placingObject(string itemid, Vector3 position, Vector3 normal)
    {
        ItemData itemDataByID = GetItemDataByID(itemid);
        if ((bool)itemDataByID.ItemFPS)
        {
            FPSItemPlacing component = itemDataByID.ItemFPS.GetComponent<FPSItemPlacing>();
            if ((bool)component && (bool)component.Item)
            {
                GameObject gameObject = Object.Instantiate(component.Item, normal, component.Item.gameObject.transform.rotation);
                gameObject.transform.forward = normal;
            }
        }
    }

    public void DirectPlacingObject(string itemid, string itemuid, Vector3 position, Quaternion rotation)
    {
        ItemData itemDataByID = GetItemDataByID(itemid);
        if (!itemDataByID.ItemFPS)
        {
            return;
        }
        FPSItemPlacing component = itemDataByID.ItemFPS.GetComponent<FPSItemPlacing>();
        if ((bool)component && (bool)component.Item)
        {
            GameObject gameObject = Object.Instantiate(component.Item, position, rotation);
            gameObject.SendMessage("SetItemID", itemid, SendMessageOptions.DontRequireReceiver);
            gameObject.SendMessage("SetItemUID", itemuid, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void Update()
    {
    }
}
