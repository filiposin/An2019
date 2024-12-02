using UnityEngine;

public class AiDoorKill : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Door")
		{
			Object.Destroy(other.gameObject);
		}
	}
}
