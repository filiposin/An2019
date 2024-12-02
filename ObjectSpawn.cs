using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectSpawn : MonoBehaviour
{
    public string ItemID = string.Empty;
    public string ItemUID = string.Empty;
    public GameObject Item;

    private void Start()
    {
        if (Item)
        {
            // Создаем объект без учета мультиплеера
            GameObject gameObject = UnityEngine.Object.Instantiate(Item, transform.position, transform.rotation);
            gameObject.SendMessage("SetItemID", ItemID, SendMessageOptions.DontRequireReceiver);
            gameObject.SendMessage("SetItemUID", ItemUID, SendMessageOptions.DontRequireReceiver);
        }
        UnityEngine.Object.Destroy(gameObject);
    }

    public void SetItemID(string id)
    {
        ItemID = id;
    }

    public void SetItemUID(string uid)
    {
        ItemUID = uid;
    }

    public void GenItemUID()
    {
        ItemUID = GetUniqueID();
    }

    private void Update()
    {
    }

    public string GetUniqueID()
    {
        System.Random random = new System.Random();
        DateTime dateTime = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
        double totalSeconds = (DateTime.UtcNow - dateTime).TotalSeconds;
        return string.Format("{0:X}", Convert.ToInt32(totalSeconds)) + "-" + string.Format("{0:X}", Convert.ToInt32(Time.time * 1000000f)) + "-" + string.Format("{0:X}", random.Next(1000000000));
    }
}