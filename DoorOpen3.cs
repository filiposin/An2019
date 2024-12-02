using UnityEngine;

public class DoorOpen3 : MonoBehaviour
{
	private void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.name == "DDoor3")
		{
			Object.Destroy(col.gameObject);
		}
	}
}
