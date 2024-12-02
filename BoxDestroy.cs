using UnityEngine;

public class BoxDestroy : MonoBehaviour
{
	private void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.name == "okno")
		{
			Object.Destroy(col.gameObject);
		}
	}
}
