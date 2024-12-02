using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	public bool SpawnOnStart = true;

	public float timeSpawn = 120f;

	public ItemData[] Item;

	public int ItemMax = 3;

	public Vector3 Offset = new Vector3(0f, 0.1f, 0f);

	private float timeTemp;

	private List<GameObject> itemList = new List<GameObject>();

	private bool full;

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
		if (ObjectsNumber >= ItemMax)
		{
			return;
		}
		if (Item.Length > 0)
		{
			ItemData itemData = Item[Random.Range(0, Item.Length)];
			GameObject gameObject = null;
			Vector3 position = DetectGround(base.transform.position + new Vector3(Random.Range(-(int)(base.transform.localScale.x / 2f), (int)(base.transform.localScale.x / 2f)), 0f, Random.Range((int)((0f - base.transform.localScale.z) / 2f), (int)(base.transform.localScale.z / 2f))));
			gameObject = Object.Instantiate(itemData.gameObject, position, Quaternion.identity);
			if ((bool)gameObject)
			{
				itemList.Add(gameObject);
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
		ObjectExistCheck();
		if (ObjectsNumber >= ItemMax)
		{
			full = true;
			return;
		}
		if (full)
		{
			timeTemp = Time.time;
			full = false;
		}
		if (Time.time > timeTemp + timeSpawn)
		{
			Spawn();
			timeTemp = Time.time;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(base.transform.position, 0.2f);
		Gizmos.DrawWireCube(base.transform.position, base.transform.localScale);
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
