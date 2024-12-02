using UnityEngine;

public class Rotation : MonoBehaviour
{
	public Vector3 Axis = Vector3.up;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(Axis * Time.deltaTime);
	}
}
