using System.Collections;
using UnityEngine;

public class Objectiv : MonoBehaviour
{
	public GameObject block;

	private void Start()
	{
		StartCoroutine(Inst());
	}

	private void Update()
	{
	}

	private IEnumerator Inst()
	{
		yield return new WaitForSeconds(2f);
		GameObject ob = Object.Instantiate(block, base.gameObject.transform.position, Quaternion.identity);
		Object.Destroy(ob);
		Debug.Log("ASD");
		Repeat();
	}

	private void Repeat()
	{
		StartCoroutine(Inst());
	}
}
