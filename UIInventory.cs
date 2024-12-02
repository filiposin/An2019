using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [HideInInspector]
    public CharacterInventory inventory;

    public GUISkin Skin;

    private int page;

    private Vector2 scrollPosition;

    public Vector2 size = new Vector2(450f, 300f);

    private void Start()
    {
        page = 0;
        inventory = GetComponent<CharacterInventory>();
        StyleManager styleManager = (StyleManager)Object.FindObjectOfType(typeof(StyleManager));
        if (!Skin && styleManager)
        {
            Skin = styleManager.GetSkin(0);
        }
    }

    private void Update()
    {
    }

    private void OnGUI()
    {
        size = new Vector2(450f, 350f);
        if (Skin)
        {
            GUI.skin = Skin;
        }

        // Удалены проверки на сетевые условия
        if (inventory == null || !inventory.Toggle)
        {
            return;
        }

        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.skin.label.fontSize = 35;
        if (GUI.Button(new Rect((Screen.width / 2) - size.x / 2f - 10f, (Screen.height / 2) - size.y / 2f - 50f, 100f, 35f), "Items"))
        {
            page = 0;
        }
        GUI.Box(new Rect((Screen.width / 2) - size.x / 2f - 10f, (Screen.height / 2) - size.y / 2f - 10f, size.x + 20f, size.y + 20f), string.Empty);
        GUI.BeginGroup(new Rect((Screen.width / 2) - size.x / 2f, (Screen.height / 2) - size.y / 2f, size.x, size.y), string.Empty);
        GUI.skin.label.fontSize = 19;
        int num = 0;

        switch (page)
        {
            case 0:
                {
                    List<ItemCollector> items = inventory.Items;
                    for (int k = 0; k < items.Count; k++)
                    {
                        if (items[k].Index != -1 && items[k].Num > 0)
                        {
                            num++;
                        }
                    }
                    scrollPosition = GUI.BeginScrollView(new Rect(0f, 0f, size.x, size.y), scrollPosition, new Rect(0f, 0f, size.x - 20f, num * 35));
                    for (int l = 0; l < items.Count; l++)
                    {
                        if (items[l].Item != null && items[l].Index != -1 && items[l].Num > 0)
                        {
                            GUI.Label(new Rect(20f, l * 35, 200f, 30f), items[l].Item.ItemName + " <color=lime>x " + items[l].Num + "</color>");
                            if (GUI.Button(new Rect(340f, l * 35, 50f, 30f), "Use"))
                            {
                                inventory.EquipItemByCollector(items[l]);
                            }
                            if (GUI.Button(new Rect(400f, l * 35, 30f, 30f), "X"))
                            {
                                Debug.Log(string.Concat(items[l], "  ", items[l].Item));
                                inventory.DropItemByCollector(items[l], items[l].Item.Quantity);
                            }
                        }
                    }
                    GUI.EndScrollView();
                    break;
                }
        }
        GUI.EndGroup();
    }
}