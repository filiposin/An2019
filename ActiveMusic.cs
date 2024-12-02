using UnityEngine;

public class ActiveMusic : MonoBehaviour
{
	public Renderer Marker;

	public Color Active;

	public GameObject ob;

	private bool i;

	public float Repeat;

	public GameObject pros;

	private void Start()
	{
		Marker = GetComponent<Renderer>();
	}

	private void Update()
	{
		if (!i && Marker.material.color == Active)
		{
			ActiveMark();
		}
	}

	private void Update2()
	{
		i = false;
		Object.Destroy(pros);
	}

	private void ActiveMark()
	{
		pros = Object.Instantiate(ob, base.transform.position, Quaternion.identity);
		Debug.Log("CubeRed");
		i = true;
		Invoke("Update2", Repeat);
	}
}
