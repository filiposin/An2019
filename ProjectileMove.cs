using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
	public float Speed = 100f;

	private void Start()
	{
		Object.Destroy(base.gameObject, 2f);
	}

	private void FixedUpdate()
	{
		base.transform.position += base.transform.forward * Speed * Time.fixedDeltaTime;
	}
}
