using UnityEngine;

[RequireComponent(typeof(FPSController))]
public class FPSInputController : MonoBehaviour
{
	public FPSController FPSmotor;

	public CharacterDriver Driver;

	private void Start()
	{
		FPSmotor = GetComponent<FPSController>();
		Driver = GetComponent<CharacterDriver>();
		Application.targetFrameRate = 120;
	}

	private void Awake()
	{
		MouseLock.MouseLocked = true;
	}

	private void Update()
	{
		if (FPSmotor == null || FPSmotor.character == null)
		{
			return;
		}
		FPSItemEquipment fPSItemEquipment = null;
		if (!FPSmotor.character.IsMine)
		{
			return;
		}
		if (((bool)Driver && Driver.DrivingSeat == null) || Driver == null)
		{
			FPSmotor.Move(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
			FPSmotor.Jump(Input.GetButton("Jump"));
		}
		else if ((bool)Driver)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				Driver.OutVehicle();
			}
			Driver.Drive(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), Input.GetButton("Jump"));
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			FPSmotor.Boost(1.1f);
		}
		if (MouseLock.MouseLocked)
		{
			FPSmotor.Aim(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
			if (FPSmotor.character.inventory != null && FPSmotor.character.inventory.FPSEquipment != null)
			{
				fPSItemEquipment = FPSmotor.character.inventory.FPSEquipment;
			}
			if (fPSItemEquipment != null)
			{
				if (Input.GetButton("Fire1"))
				{
					fPSItemEquipment.Trigger();
				}
				else
				{
					fPSItemEquipment.OnTriggerRelease();
				}
				if (Input.GetButtonDown("Fire2"))
				{
					fPSItemEquipment.Trigger2();
				}
				else
				{
					fPSItemEquipment.OnTrigger2Release();
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.F) && FPSmotor.character.inventory != null)
		{
			FPSmotor.character.Interactive(FPSmotor.FPSCamera.transform.position, FPSmotor.FPSCamera.transform.forward);
		}
		if (Input.GetKeyDown(KeyCode.R) && FPSmotor.character.inventory != null && FPSmotor.character.inventory.FPSEquipment != null)
		{
			FPSmotor.character.inventory.FPSEquipment.Reload();
		}
		FPSmotor.character.Checking(FPSmotor.FPSCamera.transform.position, FPSmotor.FPSCamera.transform.forward);
	}
}
