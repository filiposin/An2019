using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    public bool SpawnOnStart = true;
    public float timeSpawn = 120f;
    public GameObject[] Obj;
    public int ObjMax = 3;
    public Vector3 Offset = new Vector3(0f, 0.1f, 0f);
    public bool PlaceOnGround = true;

    private float timeTemp;
    private List<GameObject> itemList = new List<GameObject>();
    private int ObjectsNumber;

    private void Start()
    {
        if (SpawnOnStart)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        ObjectExistCheck();
        if (ObjectsNumber >= ObjMax)
        {
            return;
        }
        if (Obj.Length > 0)
        {
            GameObject gameObject = Obj[Random.Range(0, Obj.Length)];
            Vector3 position = DetectGround(transform.position + new Vector3(Random.Range(-(int)(transform.localScale.x / 2f), (int)(transform.localScale.x / 2f)), 0f, Random.Range(-(int)(transform.localScale.z / 2f), (int)(transform.localScale.z / 2f))));
            GameObject gameObject2 = Object.Instantiate(gameObject, position, Quaternion.identity);
            if (gameObject2 != null)
            {
                itemList.Add(gameObject2);
            }
        }
        timeTemp = Time.time;
    }

    private void ObjectExistCheck()
    {
        ObjectsNumber = 0;
        foreach (GameObject item in itemList)
        {
            if (item != null)
            {
                ObjectsNumber++;
            }
        }
    }

    private void Update()
    {
        if (Time.time > timeTemp + timeSpawn)
        {
            Spawn();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    private Vector3 DetectGround(Vector3 position)
    {
        RaycastHit hitInfo;
        if (PlaceOnGround && Physics.Raycast(position, -Vector3.up, out hitInfo, 1000f))
        {
            return hitInfo.point + Offset;
        }
        return position;
    }
}