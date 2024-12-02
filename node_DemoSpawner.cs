using UnityEngine;
using UnityEngine.AI;

public class node_DemoSpawner : MonoBehaviour
{
	[Tooltip("Drag your level pieces prefabs here")]
	public GameObject[] pieces;

	[Tooltip("Drag the transforms where you want to spawn the level pieces here")]
	public Transform[] spawnLocations;

	[Tooltip("if true, pieces will have a random y rotation by 90 degrees")]
	public bool randomYRotation;

	[Tooltip("Drag the agent prefab you want to spawn to this slot")]
	public GameObject agent;

	[Tooltip("Transform were the agent will be spawned")]
	public Transform agentSpawnLocation;

	[Tooltip("Drag the navmesh surface object here")]
	public NavMeshSurface myNavMeshSurface;

	private void Start()
	{
		Transform[] array = spawnLocations;
		foreach (Transform transform in array)
		{
			GameObject gameObject = Object.Instantiate(pieces[Random.Range(0, pieces.Length)], transform.transform.position, transform.transform.rotation);
			if (randomYRotation)
			{
				int num = Random.Range(0, 3);
				if (num == 0)
				{
					gameObject.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, 0f, base.transform.eulerAngles.z);
				}
				if (num == 1)
				{
					gameObject.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, 90f, base.transform.eulerAngles.z);
				}
				if (num == 2)
				{
					gameObject.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, 180f, base.transform.eulerAngles.z);
				}
				if (num == 3)
				{
					gameObject.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, 270f, base.transform.eulerAngles.z);
				}
			}
		}
		myNavMeshSurface.BuildNavMesh();
		GameObject gameObject2 = Object.Instantiate(agent, agentSpawnLocation.position, agentSpawnLocation.rotation);
		GameObject.FindWithTag("MainCamera").GetComponent<followCam>().target = gameObject2.transform;
	}
}
