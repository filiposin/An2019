using UnityEngine;

public class spawnHeightOffset : MonoBehaviour
{
	[Tooltip("used to adjust the spawning height of objects.")]
	public float offset = -1f;

	private void Start()
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + offset, base.transform.position.z);
	}
}
