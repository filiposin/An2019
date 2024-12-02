using UnityEngine;

public class followCam : MonoBehaviour
{
	public bool followTarget;

	public Transform target;

	public float distance = 10f;

	public float height = 5f;

	public float heightDamping = 2f;

	public float rotationDamping = 3f;

	public float speed = 1f;

	private float tempSpeed;

	public Vector3 movement;

	private void Start()
	{
		tempSpeed = speed;
		if (!followTarget)
		{
			movement = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z);
		}
	}

	private void LateUpdate()
	{
		float axisRaw = Input.GetAxisRaw("Mouse ScrollWheel");
		if (height >= 5f && height <= 45f)
		{
			height += base.transform.position.y * (0f - axisRaw) * Time.deltaTime * 50f;
		}
		if (height < 5f)
		{
			height = 5f;
		}
		if (height > 45f)
		{
			height = 45f;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			followTarget = !followTarget;
		}
		if (!followTarget)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				speed = tempSpeed * 2.5f;
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				speed = tempSpeed;
			}
			Vector3 vector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
			base.transform.position += new Vector3(vector.x * speed * Time.deltaTime, 0f, vector.z * speed * Time.deltaTime);
			float b = 0f;
			float b2 = height;
			float y = base.transform.eulerAngles.y;
			float y2 = base.transform.position.y;
			y = Mathf.LerpAngle(y, b, rotationDamping * Time.deltaTime);
			y2 = Mathf.Lerp(y2, b2, heightDamping * Time.deltaTime);
			Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
			base.transform.position -= new Vector3(0f, quaternion.y * Vector3.forward.y * distance, 0f);
			movement += new Vector3(vector.x * speed * Time.deltaTime, 0f, vector.z * speed * Time.deltaTime);
			base.transform.position = new Vector3(movement.x, y2, movement.z);
		}
		if (followTarget && (bool)target)
		{
			float b = target.eulerAngles.y;
			float b2 = target.position.y + height;
			float y = base.transform.eulerAngles.y;
			float y2 = base.transform.position.y;
			y = Mathf.LerpAngle(y, b, rotationDamping * Time.deltaTime);
			y2 = Mathf.Lerp(y2, b2, heightDamping * Time.deltaTime);
			Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
			movement = new Vector3(target.position.x, y2, target.position.z - distance);
			base.transform.position = target.position;
			base.transform.position -= quaternion * Vector3.forward * distance;
			base.transform.position = new Vector3(target.position.x, y2, target.position.z - distance);
			base.transform.LookAt(target);
		}
	}
}
