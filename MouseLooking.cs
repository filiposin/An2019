using UnityEngine;

public class MouseLooking : MonoBehaviour
{
	public float sensitivityX = 15f;

	public float sensitivityY = 15f;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float minimumY = -60f;

	public float maximumY = 60f;

	public float delayMouse = 3f;

	public float noiseX = 0.1f;

	public float noiseY = 0.1f;

	public bool Noise;

	private float rotationX;

	private float rotationY;

	private float rotationXtemp;

	private float rotationYtemp;

	private Quaternion originalRotation;

	private float noisedeltaX;

	private float noisedeltaY;

	private float stunY;

	private float breathHolderValtarget = 1f;

	private float breathHolderVal = 1f;

	private void Start()
	{
		if ((bool)GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
		originalRotation = base.transform.localRotation;
	}

	private void Update()
	{
		sensitivityY = sensitivityX;
		stunY += (0f - stunY) / 20f;
		if (Noise)
		{
			noisedeltaX += (Mathf.Cos(Time.time) * (float)Random.Range(-10, 10) / 5f * noiseX - noisedeltaX) / 100f;
			noisedeltaY += (Mathf.Sin(Time.time) * (float)Random.Range(-10, 10) / 5f * noiseY - noisedeltaY) / 100f;
		}
		else
		{
			noisedeltaX = 0f;
			noisedeltaY = 0f;
		}
		rotationXtemp += Input.GetAxis("Mouse X") * sensitivityX + noisedeltaX * breathHolderVal;
		rotationYtemp += Input.GetAxis("Mouse Y") * sensitivityY + noisedeltaY * breathHolderVal;
		rotationX += (rotationXtemp - rotationX) / delayMouse;
		rotationY += (rotationYtemp - rotationY) / delayMouse;
		if (rotationX >= 360f)
		{
			rotationX = 0f;
			rotationXtemp = 0f;
		}
		if (rotationX <= -360f)
		{
			rotationX = 0f;
			rotationXtemp = 0f;
		}
		rotationX = ClampAngle(rotationX, minimumX, maximumX);
		rotationY = ClampAngle(rotationY, minimumY, maximumY);
		rotationYtemp = ClampAngle(rotationYtemp, minimumY, maximumY);
		Quaternion quaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
		Quaternion quaternion2 = Quaternion.AngleAxis(rotationY + stunY, Vector3.left);
		base.transform.localRotation = originalRotation * quaternion * quaternion2;
		breathHolderVal += (breathHolderValtarget - breathHolderVal) / 10f;
	}

	public void Holdbreath(float val)
	{
		breathHolderValtarget = val;
	}

	public void Stun(float val)
	{
		stunY = val;
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
