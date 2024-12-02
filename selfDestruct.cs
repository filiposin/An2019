using System.Collections;
using UnityEngine;

public class selfDestruct : MonoBehaviour
{
	[Tooltip("how long this gameObject should exist in scene before being destroyed.")]
	public float destroyTime = 5f;

	private void Start()
	{
		StartCoroutine("destroyMe", destroyTime);
	}

	private IEnumerator destroyMe(float destroyTime)
	{
		yield return new WaitForSeconds(destroyTime);
		Object.Destroy(base.gameObject);
	}
}
