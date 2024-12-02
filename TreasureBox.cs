using UnityEngine;

public class TreasureBox : DamageManager
{
    public ItemData[] Item;

    public Vector3 Offset = new Vector3(0f, 0.1f, 0f);

    public void OnThisThingDead()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (Item.Length <= 0)
        {
            return;
        }
        ItemData itemData = Item[Random.Range(0, Item.Length)];
        Vector3 position = DetectGround(base.transform.position + Vector3.up);
        // Удалены проверки на Network.isServer и Network.isClient
        Object.Instantiate(itemData.gameObject, position, Quaternion.identity);
    }

    private Vector3 DetectGround(Vector3 position)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(position, -Vector3.up, out hitInfo, 1000f))
        {
            return hitInfo.point + Offset;
        }
        return position;
    }
}