using UnityEngine;

public class attacNe : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			Object.Destroy(other.gameObject);
		}
	}
}
