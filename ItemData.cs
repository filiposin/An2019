using UnityEngine;

public class ItemData : MonoBehaviour
{
    public Sprite ImageSprite;
    public string ItemName;
    public string Description;
    public int Price;
    public bool Stack = true;
    public FPSItemEquipment ItemFPS;
    public ItemEquipment ItemEquip;
    public int Quantity = 1;

    [HideInInspector]
    public int NumTag = -1;

    public AudioClip SoundPickup;
    public string ItemID;

    public bool ignore;
    public bool ShowInfo;

    public virtual void Pickup(CharacterSystem character)
    {
        if (!(character != null) || !(character.inventory != null))
        {
            return;
        }
        if (ignore)
        {
            if (SoundPickup)
            {
                AudioSource.PlayClipAtPoint(SoundPickup, base.transform.position);
            }
            RemoveItem();
        }
        else if (character.inventory.AddItemTest(this, Quantity))
        {
            character.inventory.AddItemByItemData(this, Quantity, NumTag, -1);
            if (SoundPickup)
            {
                AudioSource.PlayClipAtPoint(SoundPickup, base.transform.position);
            }
            RemoveItem();
        }
    }

    public void RemoveItem()
    {
        Object.Destroy(base.gameObject);
    }

    private void Awake()
    {
        Object.DontDestroyOnLoad(base.gameObject.transform.root);
    }

    private void Start()
    {
        // Any multiplayer-specific code has been removed
    }

    public void FixedUpdate()
    {
        ShowInfo = false;
    }

    public void GetInfo()
    {
        ShowInfo = true;
    }

    private void OnGUI()
    {
        if (ShowInfo)
        {
            Vector3 vector = Camera.main.WorldToScreenPoint(base.gameObject.transform.position);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(vector.x, (float)Screen.height - vector.y, 200f, 60f), ItemName);
        }
    }
}