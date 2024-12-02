using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] Objectman;

    public float TimeSpawn = 3f;

    public int MaxObject = 10;

    public string PlayerTag = "Player";

    public bool PlayerEnter = true;

    private float timetemp;

    private int indexSpawn;

    private List<GameObject> spawnList = new List<GameObject>();

    public bool OnActive;

    public float myRadius = 0.5f;

    private int ObjectsNumber;

    private void Start()
    {
        indexSpawn = Random.Range(0, Objectman.Length);
        timetemp = Time.time;
    }

    private void Update()
    {
        OnActive = false;
        if (PlayerEnter)
        {
            GameObject[] array = GameObject.FindGameObjectsWithTag(PlayerTag);
            for (int i = 0; i < array.Length; i++)
            {
                if (Vector3.Distance(transform.position, array[i].transform.position) < transform.localScale.x)
                {
                    OnActive = true;
                }
            }
        }
        else
        {
            OnActive = true;
        }

        if (!OnActive)
        {
            return;
        }

        ObjectExistCheck();
        if (Objectman[indexSpawn] == null || ObjectsNumber >= MaxObject || !(Time.time > timetemp + TimeSpawn))
        {
            return;
        }

        timetemp = Time.time;
        GameObject gameObject = null;
        Vector3 position = DetectGround(transform.position + new Vector3(Random.Range(-(int)(transform.localScale.x / 2f), (int)(transform.localScale.x / 2f)), 0f, Random.Range(-(int)(transform.localScale.z / 2f), (int)(transform.localScale.z / 2f))));

        gameObject = Object.Instantiate(Objectman[indexSpawn], position, Quaternion.identity);

        if (gameObject != null)
        {
            spawnList.Add(gameObject);
        }
        indexSpawn = Random.Range(0, Objectman.Length);
    }

    private void ObjectExistCheck()
    {
        ObjectsNumber = 0;
        foreach (GameObject spawn in spawnList)
        {
            if (spawn != null)
            {
                ObjectsNumber++;
            }
        }
    }

    private void OnDrawGizmos()
    {
    }

    private Vector3 DetectGround(Vector3 position)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(position, -Vector3.up, out hitInfo, 1000f))
        {
            return hitInfo.point;
        }
        return position;
    }
}