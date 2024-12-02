using UnityEngine;

public class CooldownEnter : MonoBehaviour
{
	public float maxsec;

	private float sec;

	private Collider col;

	private void Start()
	{
		col = GetComponent<Collider>();
		col.enabled = false;
	}

	private void FixedUpdate()
	{
		sec += Time.deltaTime;
		if (sec > maxsec)
		{
			col.enabled = true;
		}
	}
}
