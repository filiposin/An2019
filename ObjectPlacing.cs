using UnityEngine;

public class ObjectPlacing : MonoBehaviour
{
    public string ItemID = string.Empty;
    public string ItemData = string.Empty;
    public string ItemUID = string.Empty;

    private void Awake()
    {
        Object.DontDestroyOnLoad(base.gameObject);
    }

    public void SetItemID(string id)
    {
        ItemID = id;
    }

    public void SetItemData(string data)
    {
        ItemData = data;
    }

    public void SetItemUID(string uid)
    {
        ItemUID = uid;
    }

    private void Start()
    {
        // Any multiplayer-specific code has been removed
    }
}
