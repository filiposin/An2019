using UnityEngine;

public class SkinnedMeshMovements : MonoBehaviour
{
	public Transform[] waypoints;

	public float speed = 3f;

	private Rigidbody rigidBody;

	private int index;

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		NewDestination();
	}

	private void Update()
	{
		Vector3 vector = speed * Vector3.Normalize(waypoints[index].position - base.transform.position);
		vector.y = 0f;
		base.transform.rotation = Quaternion.LookRotation(vector);
		rigidBody.velocity = vector;
		if (vector.magnitude < 1f)
		{
			NewDestination();
		}
	}

	private void NewDestination()
	{
		index = Random.Range(0, waypoints.Length);
		rigidBody.velocity = Vector3.Normalize(waypoints[index].position - base.transform.position);
	}
}
