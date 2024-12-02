using System.Collections;
using UnityEngine;

public class quickDisable : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine("disable");
	}

	private IEnumerator disable()
	{
		yield return new WaitForSeconds(0.5f);
		GetComponent<Renderer>().enabled = false;
	}
}
