using CnControls;
using UnityEngine;

public class UnitZCNController : MonoBehaviour
{
	public FPSController FPSmotor;

	public CharacterDriver Driver;

	private void Start()
	{
		FPSmotor = GetComponent<FPSController>();
		Driver = GetComponent<CharacterDriver>();
		Application.targetFrameRate = 60;
	}

	private void Awake()
	{
		MouseLock.MouseLocked = true;
	}

	private void Update()
	{
		if (UnitZ.playerManager != null && UnitZ.playerManager.playingCharacter != null)
		{
			if (FPSmotor == null)
			{
				FPSmotor = UnitZ.playerManager.playingCharacter.GetComponent<FPSController>();
			}
			if (Driver == null)
			{
				Driver = UnitZ.playerManager.playingCharacter.GetComponent<CharacterDriver>();
			}
			if ((bool)UnitZ.playerManager.playingCharacter.GetComponent<FPSInputController>())
			{
				UnitZ.playerManager.playingCharacter.GetComponent<FPSInputController>().enabled = false;
			}
		}
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
			FPSmotor.Move(new Vector3(CnInputManager.GetAxis("Horizontal"), 0f, CnInputManager.GetAxis("Vertical")));
			FPSmotor.Jump(CnInputManager.GetButtonDown("Jump"));
		}
		else if ((bool)Driver)
		{
			if (CnInputManager.GetButtonDown("Fire3"))
			{
				Driver.OutVehicle();
			}
			Driver.Drive(new Vector2(CnInputManager.GetAxis("Horizontal"), CnInputManager.GetAxis("Vertical")), CnInputManager.GetButtonDown("Jump"));
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			FPSmotor.Boost(1.4f);
		}
		if (MouseLock.MouseLocked)
		{
			Vector2 direction = new Vector2(CnInputManager.GetAxis("Mouse X touch"), CnInputManager.GetAxis("Mouse Y touch"));
			FPSmotor.Aim(direction);
			if (FPSmotor.character.inventory != null && FPSmotor.character.inventory.FPSEquipment != null)
			{
				fPSItemEquipment = FPSmotor.character.inventory.FPSEquipment;
			}
			if (fPSItemEquipment != null)
			{
				if (CnInputManager.GetButton("Shoot"))
				{
					fPSItemEquipment.Trigger();
				}
				else
				{
					fPSItemEquipment.OnTriggerRelease();
				}
				if (CnInputManager.GetButtonDown("Zoom"))
				{
					fPSItemEquipment.Trigger2();
				}
				else
				{
					fPSItemEquipment.OnTrigger2Release();
				}
			}
		}
		if (CnInputManager.GetButtonDown("Inventory"))
		{
			MouseLock.MouseLocked = !UnitZ.Hud.TogglePanelByName("Inventory");
		}
		if (CnInputManager.GetButtonDown("Fire3") && FPSmotor.character.inventory != null)
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
