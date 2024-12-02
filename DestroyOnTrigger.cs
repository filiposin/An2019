using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
	public string m_Tag = "Player";

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == m_Tag)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
