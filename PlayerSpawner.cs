using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Vector3 Offset = Vector3.up;

    private void Start()
    {
    }

    private void OnDrawGizmos()
    {
    }

    public Vector3 SpawnPoint()
    {
        return DetectGround(base.transform.position + new Vector3(Random.Range(-(int)(base.transform.localScale.x / 2f), (int)(base.transform.localScale.x / 2f)), 0f, Random.Range(-(int)(base.transform.localScale.z / 2f), (int)(base.transform.localScale.z / 2f))));
    }

    public GameObject Spawn(GameObject player)
    {
        Vector3 position = SpawnPoint();
        Debug.Log("Spawn " + player.name);
        return Object.Instantiate(player, position, Quaternion.identity);
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