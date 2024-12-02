using UnityEngine;

public class RespawnUpdate : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Respawn")
		{
			Object.Destroy(other.gameObject);
		}
	}
}
