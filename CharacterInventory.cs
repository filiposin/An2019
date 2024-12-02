using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [HideInInspector]
    public CharacterSystem character;

    public FPSItemEquipment DefaultHand;

    public ItemSticker[] itemStickers;

    public List<ItemCollector> Items = new List<ItemCollector>();

    public ItemSticker FPSItemView;

    [HideInInspector]
    public FPSItemEquipment FPSEquipment;

    [HideInInspector]
    public ItemEquipment TDEquipment;

    [HideInInspector]
    public string StickerTextData;

    [HideInInspector]
    public bool Toggle;

    public ItemDataPackage[] StarterItems;

    [HideInInspector]
    public int UpdateCount;

    public bool Limited;

    public int LimitedSlot = 30;

    [HideInInspector]
    public bool IsReady;

    [HideInInspector]
    public ItemCollector collectorAttachedTemp;

    private void Awake()
    {
        character = base.gameObject.GetComponent<CharacterSystem>();
        if (base.transform.GetComponentsInChildren(typeof(ItemSticker)).Length > 0)
        {
            Component[] componentsInChildren = base.transform.GetComponentsInChildren(typeof(ItemSticker));
            itemStickers = new ItemSticker[componentsInChildren.Length];
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                itemStickers[i] = componentsInChildren[i].GetComponent<ItemSticker>();
                itemStickers[i].ItemIndex = -1;
            }
        }
    }

    public void SetupStarterItem()
    {
        int num = 0;
        for (int i = 0; i < StarterItems.Length; i++)
        {
            if (StarterItems[i].item != null)
            {
                AddItemByItemData(StarterItems[i].item, StarterItems[i].Num, -1, num);
                num++;
            }
        }
    }

    public string GenStickerTextData()
    {
        string text = string.Empty;
        for (int i = 0; i < itemStickers.Length; i++)
        {
            if (itemStickers[i].transform.childCount <= 0)
            {
                itemStickers[i].ItemIndex = -1;
            }
            text = text + itemStickers[i].ItemIndex + ",";
        }
        return text;
    }

    public string GetItemDataText()
    {
        string empty = string.Empty;
        string text = string.Empty;
        string text2 = string.Empty;
        string text3 = string.Empty;
        string text4 = string.Empty;
        foreach (ItemCollector item in Items)
        {
            if (item.Item != null)
            {
                text = text + item.Item.ItemID + ",";
                text2 = text2 + item.Num + ",";
                text3 = text3 + item.NumTag + ",";
                text4 = text4 + item.Shortcut + ",";
            }
        }
        return text + "|" + text2 + "|" + text3 + "|" + text4;
    }

    public void SetItemsFromText(string itemdatatext)
    {
        ItemManager itemManager = (ItemManager)UnityEngine.Object.FindObjectOfType(typeof(ItemManager));
        if (!itemManager)
        {
            return;
        }
        string[] array = itemdatatext.Split("|"[0]);
        if (array.Length < 4)
        {
            return;
        }
        RemoveAllItem();
        string[] array2 = array[0].Split(","[0]);
        string[] array3 = array[1].Split(","[0]);
        string[] array4 = array[2].Split(","[0]);
        string[] array5 = array[3].Split(","[0]);
        for (int i = 0; i < array2.Length; i++)
        {
            if (array2[i] != string.Empty)
            {
                ItemCollector itemCollector = new ItemCollector();
                itemCollector.Item = itemManager.GetItemDataByID(array2[i]);
                if (itemCollector.Item != null)
                {
                    int.TryParse(array3[i], out itemCollector.Num);
                    int.TryParse(array4[i], out itemCollector.NumTag);
                    int.TryParse(array5[i], out itemCollector.Shortcut);
                    itemCollector.Active = true;
                    AddItemByItemDataNoLimit(itemCollector.Item, itemCollector.Num, itemCollector.NumTag, itemCollector.Shortcut);
                }
            }
        }
    }

    public bool AddItemByIndex(int index, int num, int numtag)
    {
        UpdateCount++;
        if (UnitZ.itemManager != null)
        {
            ItemData item = UnitZ.itemManager.GetItem(index);
            if (item != null && num > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Index == index && item.Stack)
                    {
                        Items[i].Num += num;
                        return true;
                    }
                }
                int num2 = 0;
                for (int j = 0; j < Items.Count; j++)
                {
                    if (Items[j] != null && Items[j].Item != null)
                    {
                        num2++;
                    }
                }
                if (Limited && num2 >= LimitedSlot)
                {
                    return false;
                }
                if (item.Stack)
                {
                    ItemCollector itemCollector = new ItemCollector();
                    itemCollector.Index = index;
                    itemCollector.Item = item;
                    itemCollector.NumTag = numtag;
                    itemCollector.Num += num;
                    Items.Add(itemCollector);
                }
                else
                {
                    for (int k = 0; k < num; k++)
                    {
                        ItemCollector itemCollector2 = new ItemCollector();
                        itemCollector2.Index = index;
                        itemCollector2.Item = item;
                        itemCollector2.NumTag = numtag;
                        itemCollector2.Num = 1;
                        Items.Add(itemCollector2);
                    }
                }
                return true;
            }
        }
        return false;
    }

    public bool AddItemByItemDataNoLimit(ItemData item, int num, int numtag, int shortcut)
    {
        UpdateCount++;
        if (UnitZ.itemManager != null && item != null && num > 0)
        {
            ItemData itemData = UnitZ.itemManager.CloneItemData(item);
            if (itemData != null)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Item != null && Items[i].Item.ItemID == itemData.ItemID && itemData.Stack)
                    {
                        Items[i].Num += num;
                        return true;
                    }
                }
                if (itemData.Stack)
                {
                    ItemCollector itemCollector = new ItemCollector();
                    itemCollector.Index = UnitZ.itemManager.GetIndexByID(itemData.ItemID);
                    itemCollector.Item = itemData;
                    itemCollector.NumTag = numtag;
                    itemCollector.Shortcut = shortcut;
                    itemCollector.Num += num;
                    Items.Add(itemCollector);
                }
                else
                {
                    for (int j = 0; j < num; j++)
                    {
                        ItemCollector itemCollector2 = new ItemCollector();
                        itemCollector2.Index = UnitZ.itemManager.GetIndexByID(itemData.ItemID);
                        itemCollector2.Item = itemData;
                        itemCollector2.NumTag = numtag;
                        itemCollector2.Shortcut = shortcut;
                        itemCollector2.Num = 1;
                        Items.Add(itemCollector2);
                    }
                }
                return true;
            }
        }
        return false;
    }

    public bool AddItemByItemData(ItemData item, int num, int numtag, int shortcut)
    {
        UpdateCount++;
        if (UnitZ.itemManager != null && item != null && num > 0)
        {
            ItemData itemData = UnitZ.itemManager.CloneItemData(item);
            if (itemData != null)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Item != null && Items[i].Item.ItemID == itemData.ItemID && itemData.Stack)
                    {
                        Items[i].Num += num;
                        return true;
                    }
                }
                int num2 = 0;
                for (int j = 0; j < Items.Count; j++)
                {
                    if (Items[j] != null && Items[j].Item != null)
                    {
                        num2++;
                    }
                }
                if (Limited && num2 >= LimitedSlot)
                {
                    return false;
                }
                if (itemData.Stack)
                {
                    ItemCollector itemCollector = new ItemCollector();
                    itemCollector.Index = UnitZ.itemManager.GetIndexByID(itemData.ItemID);
                    itemCollector.Item = itemData;
                    itemCollector.NumTag = numtag;
                    itemCollector.Shortcut = shortcut;
                    itemCollector.Num += num;
                    Items.Add(itemCollector);
                }
                else
                {
                    for (int k = 0; k < num; k++)
                    {
                        ItemCollector itemCollector2 = new ItemCollector();
                        itemCollector2.Index = UnitZ.itemManager.GetIndexByID(itemData.ItemID);
                        itemCollector2.Item = itemData;
                        itemCollector2.NumTag = numtag;
                        itemCollector2.Shortcut = shortcut;
                        itemCollector2.Num = 1;
                        Items.Add(itemCollector2);
                    }
                }
                return true;
            }
        }
        return false;
    }

    public bool AddItemByCollector(ItemCollector item)
    {
        UpdateCount++;
        if (UnitZ.itemManager != null && item.Item != null && item.Num > 0)
        {
            ItemData itemData = UnitZ.itemManager.CloneItemData(item.Item);
            if (itemData != null)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Item != null && Items[i].Item.ItemID == itemData.ItemID && itemData.Stack)
                    {
                        Items[i].Num += item.Num;
                        return true;
                    }
                }
                int num = 0;
                for (int j = 0; j < Items.Count; j++)
                {
                    if (Items[j] != null && Items[j].Item != null)
                    {
                        num++;
                    }
                }
                if (Limited && num >= LimitedSlot)
                {
                    return false;
                }
                if (itemData.Stack)
                {
                    ItemCollector itemCollector = new ItemCollector();
                    itemCollector.Index = UnitZ.itemManager.GetIndexByID(itemData.ItemID);
                    itemCollector.Item = itemData;
                    itemCollector.NumTag = item.NumTag;
                    itemCollector.Shortcut = item.Shortcut;
                    itemCollector.Num += item.Num;
                    Items.Add(itemCollector);
                }
                else
                {
                    for (int k = 0; k < item.Num; k++)
                    {
                        ItemCollector itemCollector2 = new ItemCollector();
                        itemCollector2.Index = UnitZ.itemManager.GetIndexByID(itemData.ItemID);
                        itemCollector2.Item = itemData;
                        itemCollector2.NumTag = item.NumTag;
                        itemCollector2.Shortcut = item.Shortcut;
                        itemCollector2.Num = 1;
                        Items.Add(itemCollector2);
                    }
                }
                return true;
            }
        }
        return false;
    }

    public bool AddItemTest(ItemCollector item, int num)
    {
        if (UnitZ.itemManager != null && item.Item != null && num > 0)
        {
            ItemData itemData = UnitZ.itemManager.CloneItemData(item.Item);
            if (itemData != null)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Item != null && Items[i].Item.ItemID == itemData.ItemID && itemData.Stack)
                    {
                        return true;
                    }
                }
                int num2 = 0;
                for (int j = 0; j < Items.Count; j++)
                {
                    if (Items[j] != null && Items[j].Item != null)
                    {
                        num2++;
                    }
                }
                if (Limited && num2 >= LimitedSlot)
                {
                    return false;
                }
                return true;
            }
        }
        return false;
    }

    public bool AddItemTest(ItemData itemdata, int num)
    {
        if (UnitZ.itemManager != null && itemdata != null && num > 0)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Item != null && Items[i].Item.ItemID == itemdata.ItemID && itemdata.Stack)
                {
                    return true;
                }
            }
            int num2 = 0;
            for (int j = 0; j < Items.Count; j++)
            {
                if (Items[j] != null && Items[j].Item != null)
                {
                    num2++;
                }
            }
            if (Limited && num2 >= LimitedSlot)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public bool AddItemByCollector(ItemCollector item, int num, int shortcut)
    {
        UpdateCount++;
        if (UnitZ.itemManager != null && item.Item != null && num > 0)
        {
            ItemData itemData = UnitZ.itemManager.CloneItemData(item.Item);
            if (itemData != null)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Item != null && Items[i].Item.ItemID == itemData.ItemID && itemData.Stack)
                    {
                        Items[i].Num += num;
                        return true;
                    }
                }
                int num2 = 0;
                for (int j = 0; j < Items.Count; j++)
                {
                    if (Items[j] != null && Items[j].Item != null)
                    {
                        num2++;
                    }
                }
                if (Limited && num2 >= LimitedSlot)
                {
                    return false;
                }
                if (itemData.Stack)
                {
                    ItemCollector itemCollector = new ItemCollector();
                    itemCollector.Index = UnitZ.itemManager.GetIndexByID(itemData.ItemID);
                    itemCollector.Item = itemData;
                    itemCollector.NumTag = item.NumTag;
                    itemCollector.Shortcut = shortcut;
                    itemCollector.Num += num;
                    Items.Add(itemCollector);
                }
                else
                {
                    for (int k = 0; k < num; k++)
                    {
                        ItemCollector itemCollector2 = new ItemCollector();
                        itemCollector2.Index = UnitZ.itemManager.GetIndexByID(itemData.ItemID);
                        itemCollector2.Item = itemData;
                        itemCollector2.NumTag = item.NumTag;
                        itemCollector2.Shortcut = shortcut;
                        itemCollector2.Num = 1;
                        Items.Add(itemCollector2);
                    }
                }
                return true;
            }
        }
        return false;
    }

    public bool AddItemByCollector(ItemCollector item, int num)
    {
        UpdateCount++;
        if (UnitZ.itemManager != null && item.Item != null && num > 0)
        {
            ItemData itemData = UnitZ.itemManager.CloneItemData(item.Item);
            if (itemData != null)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Item != null && Items[i].Item.ItemID == itemData.ItemID && itemData.Stack)
                    {
                        Items[i].Num += num;
                        return true;
                    }
                }
                int num2 = 0;
                for (int j = 0; j < Items.Count; j++)
                {
                    if (Items[j] != null && Items[j].Item != null)
                    {
                        num2++;
                    }
                }
                if (Limited && num2 >= LimitedSlot)
                {
                    return false;
                }
                if (itemData.Stack)
                {
                    ItemCollector itemCollector = new ItemCollector();
                    itemCollector.Index = UnitZ.itemManager.GetIndexByID(itemData.ItemID);
                    itemCollector.Item = itemData;
                    itemCollector.NumTag = item.NumTag;
                    itemCollector.Shortcut = item.Shortcut;
                    itemCollector.Num += num;
                    Items.Add(itemCollector);
                }
                else
                {
                    for (int k = 0; k < num; k++)
                    {
                        ItemCollector itemCollector2 = new ItemCollector();
                        itemCollector2.Index = UnitZ.itemManager.GetIndexByID(itemData.ItemID);
                        itemCollector2.Item = itemData;
                        itemCollector2.NumTag = item.NumTag;
                        itemCollector2.Shortcut = item.Shortcut;
                        itemCollector2.Num = 1;
                        Items.Add(itemCollector2);
                    }
                }
                return true;
            }
        }
        return false;
    }

    public bool RemoveItemByCollectorIndex(int index, int num)
    {
        UpdateCount++;
        if (UnitZ.itemManager != null && num > 0)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] == null || i != index)
                {
                    continue;
                }
                if (Items[i].Num <= 0)
                {
                    Debug.Log(Items[i].Item.ItemName + " Is no more");
                    return false;
                }
                if (Items[i].Num < num)
                {
                    if (Items[i].Num > 0)
                    {
                        Items[i].Num -= Items[i].Num;
                    }
                }
                else
                {
                    Items[i].Num -= num;
                }
                if (Items[i].Num <= 0)
                {
                    RemoveEquipItemByIndex(Items[i].Index);
                    Items.RemoveAt(index);
                }
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(ItemData itemdata, int num)
    {
        UpdateCount++;
        if (UnitZ.itemManager != null && itemdata != null && num > 0)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] == null || !(Items[i].Item.ItemID == itemdata.ItemID))
                {
                    continue;
                }
                if (Items[i].Num <= 0)
                {
                    Debug.Log(Items[i].Item.ItemName + " Is no more");
                    return false;
                }
                if (Items[i].Num < num)
                {
                    if (Items[i].Num > 0)
                    {
                        Items[i].Num -= Items[i].Num;
                    }
                }
                else
                {
                    Items[i].Num -= num;
                }
                if (Items[i].Num <= 0)
                {
                    RemoveEquipItemByIndex(Items[i].Index);
                    Items.RemoveAt(i);
                }
                return true;
            }
        }
        return false;
    }

    public void RemoveEquipItemByCollector(ItemCollector item)
    {
        UpdateCount++;
        if (!(UnitZ.itemManager != null))
        {
            return;
        }
        for (int i = 0; i < itemStickers.Length; i++)
        {
            if (itemStickers[i].ItemIndex == item.Index)
            {
                RemoveEquipItem(itemStickers[i]);
                break;
            }
        }
    }

    public bool RemoveItemByCollector(ItemCollector itemcollector, int num)
    {
        UpdateCount++;
        ItemData item = itemcollector.Item;
        if (UnitZ.itemManager != null && item != null && num > 0)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] == null || Items[i] != itemcollector)
                {
                    continue;
                }
                if (Items[i].Num <= 0)
                {
                    Debug.Log(Items[i].Item.ItemName + " Is no more");
                    return false;
                }
                if (Items[i].Num < num)
                {
                    if (Items[i].Num > 0)
                    {
                        Items[i].Num -= Items[i].Num;
                    }
                }
                else
                {
                    Items[i].Num -= num;
                }
                if (Items[i].Num <= 0)
                {
                    if (collectorAttachedTemp == itemcollector)
                    {
                        RemoveEquipItemByIndex(Items[i].Index);
                        Debug.Log(string.Concat("Remove ", collectorAttachedTemp, " Num ", Items[i].Num, "/", itemcollector.Num));
                    }
                    Items.RemoveAt(i);
                }
                return true;
            }
        }
        return false;
    }

    public bool RemoveItemByIndex(int index, int num)
    {
        UpdateCount++;
        if (UnitZ.itemManager != null)
        {
            ItemData item = UnitZ.itemManager.GetItem(index);
            if (item != null && num > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i] == null || Items[i].Index != index)
                    {
                        continue;
                    }
                    if (Items[i].Num <= 0)
                    {
                        return false;
                    }
                    if (Items[i].Num < num)
                    {
                        if (Items[i].Num > 0)
                        {
                            Items[i].Num -= Items[i].Num;
                        }
                    }
                    else
                    {
                        Items[i].Num -= num;
                    }
                    if (Items[i].Num <= 0)
                    {
                        RemoveEquipItemByIndex(Items[i].Index);
                        Items.RemoveAt(i);
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public void RemoveAllItem()
    {
        UpdateCount++;
        if (UnitZ.itemManager != null)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] != null)
                {
                    Items[i].Num = 0;
                    RemoveEquipItemByIndex(Items[i].Index);
                    Items.RemoveAt(i);
                }
            }
        }
        Items.Clear();
    }

    public int GetItemNum(ItemData itemdata)
    {
        if (UnitZ.itemManager != null && itemdata != null)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Item.ItemID == itemdata.ItemID)
                {
                    return Items[i].Num;
                }
            }
        }
        return 0;
    }

    public int GetItemNumByIndex(int index)
    {
        if (UnitZ.itemManager != null)
        {
            ItemData item = UnitZ.itemManager.GetItem(index);
            if (item != null)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Index == index)
                    {
                        return Items[i].Num;
                    }
                }
            }
        }
        return 0;
    }

    public bool CheckItem(ItemData itemdata, int num)
    {
        if (UnitZ.itemManager != null && itemdata != null && num > 0)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Item.ItemID == itemdata.ItemID && Items[i].Num >= num)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckItemByIndex(int index, int num)
    {
        if (UnitZ.itemManager != null)
        {
            ItemData item = UnitZ.itemManager.GetItem(index);
            if (item != null && num > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Index == index && Items[i].Num >= num)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void DropItem(ItemData itemdata, int num, int numtag)
    {
        UpdateCount++;
        Debug.Log(itemdata);
        if (RemoveItem(itemdata, num))
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(itemdata.gameObject, base.transform.position, itemdata.gameObject.transform.rotation);
            ItemData component = gameObject.GetComponent<ItemData>();
            component.NumTag = numtag;
            component.Quantity = num;
        }
    }

    public void DropItemByCollector(ItemCollector item, int num)
    {
        UpdateCount++;
        if (RemoveItemByCollector(item, num))
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(item.Item.gameObject, base.transform.position, item.Item.gameObject.transform.rotation);
            ItemData component = gameObject.GetComponent<ItemData>();
            component.NumTag = item.NumTag;
            component.Quantity = num;
        }
    }

    public void EquipItemByItemIndex(int index)
    {
        UpdateCount++;
        if (!(UnitZ.itemManager != null))
        {
            return;
        }
        ItemData item = UnitZ.itemManager.GetItem(index);
        if (item != null)
        {
            EquipItem(item.ItemEquip, index);
            if ((bool)item.ItemFPS)
            {
                AttachFPSItemView(item.ItemFPS);
            }
        }
    }

    public void EquipItemByCollector(ItemCollector item)
    {
        UpdateCount++;
        if (item.Index == -1 || item.Num <= 0 || !(UnitZ.itemManager != null))
        {
            return;
        }
        ItemData item2 = UnitZ.itemManager.GetItem(item.Index);
        if (item2 != null)
        {
            EquipItem(item2.ItemEquip, item.Index);
            if ((bool)item2.ItemFPS)
            {
                AttachFPSItemViewAndCollector(item2.ItemFPS, item);
            }
        }
    }

    public void RemoveEquipItemByIndex(int index)
    {
        UpdateCount++;
        if (!(UnitZ.itemManager != null))
        {
            return;
        }
        for (int i = 0; i < itemStickers.Length; i++)
        {
            if (itemStickers[i].ItemIndex == index)
            {
                RemoveEquipItem(itemStickers[i]);
                break;
            }
        }
    }

    public void RemoveStickerItem(ItemSticker sticker)
    {
        UpdateCount++;
        if (sticker != null)
        {
            sticker.ItemIndex = -1;
            Component[] componentsInChildren = sticker.transform.GetComponentsInChildren(typeof(ItemEquipment));
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
            }
        }
    }

    public void RemoveEquipItem(ItemSticker sticker)
    {
        UpdateCount++;
        if (sticker != null)
        {
            sticker.ItemIndex = -1;
            Component[] componentsInChildren = sticker.transform.GetComponentsInChildren(typeof(ItemEquipment));
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
            }
            Component[] componentsInChildren2 = FPSItemView.transform.GetComponentsInChildren(typeof(FPSItemEquipment));
            for (int j = 0; j < componentsInChildren2.Length; j++)
            {
                UnityEngine.Object.Destroy(componentsInChildren2[j].gameObject);
            }
        }
    }

    public void AttachItem(ItemSticker sticker, ItemEquipment item)
    {
        UpdateCount++;
        if (sticker != null && item != null)
        {
            RemoveEquipItem(sticker);
            Quaternion rotation = sticker.transform.rotation;
            rotation.eulerAngles += sticker.RotationOffset;
            GameObject gameObject = UnityEngine.Object.Instantiate(item.gameObject, sticker.transform.position, rotation);
            gameObject.transform.parent = sticker.gameObject.transform;
            if (sticker.equipType == EquipType.Weapon)
            {
                TDEquipment = gameObject.GetComponent<ItemEquipment>();
            }
        }
    }

    public int GetCollectorFPSindex()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] != null && Items[i] == collectorAttachedTemp)
            {
                return i;
            }
        }
        return 0;
    }

    public void SaveDataToItemCollector(FPSWeaponEquipment fpsprefab, ItemCollector item)
    {
        if (fpsprefab != null)
        {
            fpsprefab.SetCollectorSlot(item);
        }
    }

    public void AttachFPSItemView(FPSItemEquipment item)
    {
        UpdateCount++;
        if (item != null && FPSItemView != null)
        {
            Quaternion rotation = FPSItemView.transform.rotation;
            rotation.eulerAngles += FPSItemView.RotationOffset;
            RemoveEquipItem(FPSItemView);
            GameObject gameObject = UnityEngine.Object.Instantiate(item.gameObject, FPSItemView.transform.position, rotation);
            gameObject.transform.parent = FPSItemView.gameObject.transform;
            FPSEquipment = gameObject.GetComponent<FPSItemEquipment>();
        }
    }

    public void AttachFPSItemViewAndCollector(FPSItemEquipment item, ItemCollector itemcollector)
    {
        UpdateCount++;
        if (item != null && FPSItemView != null)
        {
            Quaternion rotation = FPSItemView.transform.rotation;
            rotation.eulerAngles += FPSItemView.RotationOffset;
            RemoveEquipItem(FPSItemView);
            GameObject gameObject = UnityEngine.Object.Instantiate(item.gameObject, FPSItemView.transform.position, rotation);
            gameObject.transform.parent = FPSItemView.gameObject.transform;
            FPSEquipment = gameObject.GetComponent<FPSItemEquipment>();
            collectorAttachedTemp = itemcollector;
            SaveDataToItemCollector(FPSEquipment.GetComponent<FPSWeaponEquipment>(), itemcollector);
        }
    }

    public void EquipItem(ItemEquipment item, int index)
    {
        UpdateCount++;
        for (int i = 0; i < itemStickers.Length; i++)
        {
            if (itemStickers[i] != null && item != null && itemStickers[i].equipType == item.itemType && itemStickers[i].Primary)
            {
                AttachItem(itemStickers[i], item);
                itemStickers[i].ItemIndex = index;
                return;
            }
        }
        for (int j = 0; j < itemStickers.Length; j++)
        {
            if (itemStickers[j] != null && item != null && itemStickers[j].equipType == item.itemType)
            {
                AttachItem(itemStickers[j], item);
                itemStickers[j].ItemIndex = index;
                break;
            }
        }
    }

    public void SwarpShortcut(ItemCollector item1, ItemCollector item2)
    {
        ItemCollector itemCollector = new ItemCollector();
        CopyShortcut(itemCollector, item1);
        CopyShortcut(item1, item2);
        CopyShortcut(item2, itemCollector);
        UpdateCount++;
    }

    public void CopyShortcut(ItemCollector item, ItemCollector source)
    {
        if (item != null && source != null)
        {
            item.Shortcut = source.Shortcut;
        }
    }

    public void PutCollector(ItemCollector item, int invindex)
    {
        putCollector(item.Item.ItemID, invindex, item.Num, item.NumTag);
        UpdateCount++;
    }

    private void putCollector(string itemID, int invindex, int num, int numTag)
    {
        throw new NotImplementedException();
    }

    public void SwarpCollector(ItemCollector item1, ItemCollector item2)
    {
        ItemCollector itemCollector = new ItemCollector();
        CopyCollector(itemCollector, item1);
        CopyCollector(item1, item2);
        CopyCollector(item2, itemCollector);
        UpdateCount++;
    }

    public void CopyCollector(ItemCollector item, ItemCollector source)
    {
        item.Active = source.Active;
        item.Index = source.Index;
        item.Item = source.Item;
        item.Num = source.Num;
        item.NumTag = source.NumTag;
        item.Shortcut = source.Shortcut;
    }

    public ItemCollector GetItemByShortCutIndex(int index)
    {
        if (UnitZ.itemManager != null)
        {
            foreach (ItemCollector item in Items)
            {
                if (item != null && item.Shortcut == index)
                {
                    return item;
                }
            }
        }
        return null;
    }

    public void DeleteShortcut(ItemCollector itemCollector, int shortcut)
    {
        if (!(UnitZ.itemManager != null))
        {
            return;
        }
        foreach (ItemCollector item in Items)
        {
            if (item != null)
            {
                if (itemCollector != item && item.Shortcut == shortcut)
                {
                    item.Shortcut = -1;
                }
                if (itemCollector == item)
                {
                    item.Shortcut = shortcut;
                }
            }
        }
    }

    private void Update()
    {
        int num = 0;
        foreach (ItemCollector item in Items)
        {
            item.InventoryIndex = num;
            num++;
        }
        IsReady = true;
    }

    private void LateUpdate()
    {
        FreeHandsChecker();
    }

    private void FreeHandsChecker()
    {
        if ((bool)FPSItemView && FPSItemView.gameObject.transform.childCount <= 0)
        {
            MeleeMode();
        }
    }

    public void MeleeMode()
    {
        if (!(DefaultHand == null))
        {
            AttachFPSItemView(DefaultHand);
        }
    }

    public void EquipmentOnAction()
    {
        if ((bool)TDEquipment)
        {
            TDEquipment.Action();
        }
    }
}