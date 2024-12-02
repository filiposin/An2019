using UnityEngine;

public class TvZoneFixed : MonoBehaviour
{
	public GameObject blocker;

	public float sec;

	public float presec;

	public float maxsec;

	public GameObject prepointer;

	public Renderer pointer;

	public Color Active;

	public OpenDraw driver;

	private bool fixi;

	private void Start()
	{
	}

	public void OnTriggerStay(Collider other)
	{
		if (!(other.name == "NeighborBody(Clone)"))
		{
			return;
		}
		if (pointer.material.color == Active)
		{
			blocker.SetActive(false);
			sec = 0f;
			presec = 0f;
			Debug.Log("realtime");
			return;
		}
		presec += Time.deltaTime;
		if ((double)presec >= 2.5)
		{
			blocker.SetActive(true);
			sec += Time.deltaTime;
			if (driver != null)
			{
				Tvdraw();
			}
			if (sec >= maxsec)
			{
				blocker.SetActive(false);
				sec = 0f;
				presec = 0f;
			}
		}
		Debug.Log("triger");
	}

	private void FixedUpdate()
	{
		if (pointer == null && (bool)GameObject.Find("Cone"))
		{
			pointer = GameObject.Find("Cone").GetComponent<Renderer>();
		}
		if (pointer != null && pointer.material.color == Active)
		{
			Debug.Log("fixedup");
			blocker.SetActive(false);
		}
	}

	public void Tvdraw()
	{
		if (!fixi)
		{
			driver.Pick();
			fixi = true;
		}
	}
}
