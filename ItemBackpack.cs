using System.Collections.Generic;
using UnityEngine;

public class ItemBackpack : ItemData
{
    public List<ItemCollector> Items = new List<ItemCollector>();

    private void Start()
    {
        // Формируем строку данных предметов для логирования.
        string itemsData = GetItemsData();
        Debug.Log("Copy " + itemsData);
    }

    // Метод для создания строки с данными о предметах.
    private string GetItemsData()
    {
        string itemIDs = string.Empty;
        string itemNumbers = string.Empty;
        string itemTags = string.Empty;

        foreach (ItemCollector item in Items)
        {
            if (item.Item != null)
            {
                itemIDs += item.Item.ItemID + ",";
                itemNumbers += item.Num + ",";
                itemTags += item.NumTag + ",";
            }
        }

        return $"{itemIDs}|{itemNumbers}|{itemTags}";
    }

    public void AddItem(ItemCollector item)
    {
        if (item.Active)
        {
            Items.Add(item);
        }
    }

    public override void Pickup(CharacterSystem character)
    {
        if (character == null || character.inventory == null || Items == null)
            return;

        // Передача предметов в инвентарь персонажа.
        foreach (ItemCollector item in Items)
        {
            if (item.Item != null)
            {
                Debug.Log($"Pick up {item.Item} Num tag {item.NumTag}");
                character.inventory.AddItemByItemData(item.Item, item.Num, item.NumTag, -1);
            }
        }

        RemoveItem();
    }
}
