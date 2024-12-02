using UnityEngine;

public class Crosshair : MonoBehaviour
{
	private FPSController fpsController;

	public Texture2D CrosshairImg;

	public Texture2D CrosshairZoomImg;

	public Texture2D CrosshairHit;

	public float HitDuration = 0.2f;

	private float timeTemp;

	private void Start()
	{
		if ((bool)base.transform.root)
		{
			fpsController = base.transform.root.GetComponent<FPSController>();
		}
		else
		{
			fpsController = base.transform.GetComponent<FPSController>();
		}
	}

	public void Hit()
	{
		timeTemp = Time.time;
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		if ((bool)fpsController)
		{
			if (fpsController.zooming)
			{
				if ((bool)CrosshairZoomImg)
				{
					GUI.DrawTexture(new Rect(Screen.width / 2 - CrosshairZoomImg.width / 2, Screen.height / 2 - CrosshairZoomImg.height / 2, CrosshairZoomImg.width, CrosshairZoomImg.height), CrosshairZoomImg);
				}
			}
			else if ((bool)CrosshairImg)
			{
				GUI.DrawTexture(new Rect(Screen.width / 2 - CrosshairImg.width / 2, Screen.height / 2 - CrosshairImg.height / 2, CrosshairImg.width, CrosshairImg.height), CrosshairImg);
			}
		}
		else if ((bool)CrosshairImg)
		{
			GUI.DrawTexture(new Rect(Screen.width / 2 - CrosshairImg.width / 2, Screen.height / 2 - CrosshairImg.height / 2, CrosshairImg.width, CrosshairImg.height), CrosshairImg);
		}
		if (Time.time < timeTemp + HitDuration && (bool)CrosshairHit)
		{
			GUI.DrawTexture(new Rect(Screen.width / 2 - CrosshairHit.width / 2, Screen.height / 2 - CrosshairHit.height / 2, CrosshairHit.width, CrosshairHit.height), CrosshairHit);
		}
	}
}
