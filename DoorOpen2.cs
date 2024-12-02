using UnityEngine;

public class DoorOpen2 : MonoBehaviour
{
	private void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.name == "DDoor2")
		{
			Object.Destroy(col.gameObject);
		}
	}
}
