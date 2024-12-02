using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(PlayerView))]
[RequireComponent(typeof(FPSInputController))]
[RequireComponent(typeof(CharacterInventory))]
[AddComponentMenu("Character/FPS Input Controller")]
public class FPSController : MonoBehaviour
{
	[HideInInspector]
	public CharacterSystem character;

	[HideInInspector]
	public CharacterMotor motor;

	[HideInInspector]
	public Vector3 inputDirection;

	private Vector2 mouseDirection;

	public Transform FPSViewPart;

	public Camera FPSCamera;

	[HideInInspector]
	public float sensitivityXMult = 1f;

	[HideInInspector]
	public float sensitivityYMult = 1f;

	public float sensitivityX = 15f;

	public float sensitivityY = 15f;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float minimumY = -60f;

	public float maximumY = 60f;

	public float delayMouse = 0.5f;

	private float rotationX;

	private float rotationY;

	private float rotationXtemp;

	private float rotationYtemp;

	private Quaternion originalRotation;

	private Vector2 kickPower;

	private float fovTemp = 40f;

	private float fovTarget;

	[HideInInspector]
	public bool zooming;

	private float climbDirection;

	private void Start()
	{
		character = base.gameObject.GetComponent<CharacterSystem>();
		PlayerView component = base.gameObject.GetComponent<PlayerView>();
		if ((bool)component && FPSCamera == null)
		{
			FPSCamera = component.FPScamera.MainCamera;
			FPSViewPart = component.FPScamera.transform;
		}
		motor = GetComponent<CharacterMotor>();
		if ((bool)GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
		originalRotation = base.transform.localRotation;
		if ((bool)FPSCamera)
		{
			fovTemp = FPSCamera.fieldOfView;
			fovTarget = fovTemp;
		}
	}

	public void Zoom(float zoom)
	{
		fovTarget = zoom;
		zooming = !zooming;
	}

	public void Kick(Vector2 power)
	{
		kickPower = power;
	}

	public void HideGun(bool visible)
	{
		if ((bool)FPSCamera && (bool)FPSCamera.GetComponent<Camera>())
		{
			FPSCamera.GetComponent<Camera>().enabled = visible;
		}
	}

	public void Boost(float mult)
	{
		motor.boostMults = mult;
	}

	public void Climb(float speed)
	{
		motor.Climb(speed);
	}

	public void Move(Vector3 directionVector)
	{
		if (!(character == null))
		{
			inputDirection = directionVector;
			if (directionVector != Vector3.zero)
			{
				float magnitude = directionVector.magnitude;
				directionVector /= magnitude;
				magnitude = Mathf.Min(1f, magnitude);
				magnitude *= magnitude;
				directionVector *= magnitude;
			}
			Quaternion rotation = base.transform.rotation;
			if ((bool)FPSViewPart)
			{
				rotation = FPSViewPart.transform.rotation;
			}
			Vector3 eulerAngles = rotation.eulerAngles;
			eulerAngles.x = 0f;
			eulerAngles.z = 0f;
			rotation.eulerAngles = eulerAngles;
			character.MoveTo(rotation * directionVector);
		}
	}

	public void Jump(bool jump)
	{
		motor.inputJump = jump;
	}

	public void Aim(Vector2 direction)
	{
		mouseDirection = direction;
	}

	private void Update()
	{
		if (MouseLock.MouseLocked && !(character == null) && character.IsMine)
		{
			if (!zooming)
			{
				sensitivityXMult = 1f;
				sensitivityYMult = sensitivityXMult;
				fovTarget = fovTemp;
			}
			else
			{
				sensitivityXMult = fovTarget / fovTemp;
				sensitivityYMult = sensitivityXMult;
			}
			if ((bool)FPSCamera)
			{
				FPSCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(FPSCamera.GetComponent<Camera>().fieldOfView, fovTarget, 0.5f);
			}
			motor.boostMults += (1f - motor.boostMults) * Time.deltaTime;
			kickPower.y += (0f - kickPower.y) / 20f;
			kickPower.x += (0f - kickPower.x) / 20f;
			rotationXtemp += mouseDirection.x * sensitivityX * sensitivityXMult;
			rotationYtemp += mouseDirection.y * sensitivityY * sensitivityYMult;
			rotationX = Mathf.Lerp(rotationX, rotationXtemp, delayMouse);
			rotationY = Mathf.Lerp(rotationY, rotationYtemp, delayMouse);
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
			Quaternion quaternion = Quaternion.AngleAxis(rotationX + kickPower.x, Vector3.up);
			Quaternion quaternion2 = Quaternion.AngleAxis(rotationY + kickPower.y, Vector3.left);
			if ((bool)FPSViewPart)
			{
				FPSViewPart.transform.localRotation = originalRotation * Quaternion.AngleAxis(0f, Vector3.up) * quaternion2;
				base.transform.localRotation = originalRotation * quaternion * Quaternion.AngleAxis(0f, Vector3.left);
			}
			else
			{
				base.transform.localRotation = originalRotation * quaternion * quaternion2;
			}
		}
	}

	public void Stun(float val)
	{
		kickPower.y = val;
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
