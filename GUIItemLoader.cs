using System.Collections.Generic;
using UnityEngine;

public class GUIItemLoader : MonoBehaviour
{
	public RectTransform Canvas;

	public RectTransform Item;

	public int Size = 5;

	public float Spacing = 3f;

	private int updateTmp;

	private int numRaw;

	private int numItem;

	public string Type;

	[HideInInspector]
	public CharacterInventory currentInventory;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		UpdateGUIInventory();
	}

	private void AddItemToRaw(ItemCollector item)
	{
		GameObject gameObject = Object.Instantiate(Item.gameObject, Vector3.zero, Quaternion.identity);
		GUIItemCollector component = gameObject.GetComponent<GUIItemCollector>();
		if ((bool)component)
		{
			component.Item = item;
			component.currentInventory = currentInventory;
			component.Type = Type;
			if ((bool)item.Item.ImageSprite)
			{
				component.Icon.sprite = item.Item.ImageSprite;
			}
			component.Num.text = item.Num.ToString();
		}
		gameObject.transform.SetParent(Canvas.gameObject.transform);
		RectTransform component2 = gameObject.GetComponent<RectTransform>();
		component2.anchoredPosition = new Vector2((component2.sizeDelta.x + Spacing) * (float)numItem + Spacing, 0f - ((component2.sizeDelta.y + Spacing) * (float)numRaw + Spacing));
		component2.localScale = Item.gameObject.transform.localScale;
		numItem++;
		if (numItem >= Size)
		{
			numItem = 0;
			numRaw++;
		}
		Canvas.sizeDelta = new Vector2(Canvas.sizeDelta.x, (Item.sizeDelta.y + Spacing) * (float)(numRaw + 1));
	}

	public void UpdateGUIInventory()
	{
		if (currentInventory == null)
		{
			return;
		}
		Clear();
		List<ItemCollector> items = currentInventory.Items;
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].Num > 0 && items[i].Item != null && items[i].Index > -1)
			{
				AddItemToRaw(items[i]);
			}
		}
	}

	private void Clear()
	{
		if (Canvas == null)
		{
			return;
		}
		numItem = 0;
		numRaw = 0;
		foreach (Transform item in Canvas.transform)
		{
			Object.Destroy(item.gameObject);
		}
	}

	public void UpdateFunction()
	{
		if (!(currentInventory == null) && currentInventory.UpdateCount != updateTmp)
		{
			updateTmp = currentInventory.UpdateCount;
			UpdateGUIInventory();
		}
	}

	private void Update()
	{
		UpdateFunction();
	}
}
