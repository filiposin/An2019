using LeTai.Asset.TranslucentImage;
using UnityEngine;

public class BlurMaster : MonoBehaviour
{
	public TranslucentImageSource Camera;

	public GameObject inventory;

	public GameObject fps;

	private void Start()
	{
	}

	private void Update()
	{
		if (Camera == null)
		{
			Camera = Object.FindObjectOfType<TranslucentImageSource>();
		}
		if (!(inventory == null))
		{
			return;
		}
		if ((bool)GameObject.Find("Inventory"))
		{
			Camera.enabled = true;
			fps.SetActive(false);
		}
		else if ((bool)GameObject.Find("Settings"))
		{
			if (Camera != null)
			{
				Camera.enabled = true;
				fps.SetActive(false);
			}
		}
		else if (Camera != null)
		{
			Camera.enabled = false;
			fps.SetActive(true);
		}
	}
}
