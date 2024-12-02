using UnityEngine;

public class Bang : MonoBehaviour
{
	private void Start()
	{
		Object.Destroy(base.gameObject, 1f);
	}
}
