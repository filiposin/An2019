using UnityEngine;

public class CameraSwing : MonoBehaviour
{
	public Vector3 Speed = new Vector3(0f, 0.01f, 0f);

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(Speed * Mathf.Sin(Time.time));
	}
}
