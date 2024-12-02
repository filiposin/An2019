using UnityEngine;

public class DoorOpener : ObjectTrigger
{
	public DoorFrame Door;

	public DoorFrame IceDoor;

	public bool locked;

	public GameObject zamok;

	public bool boarded;

	public GameObject wood;

	public GameObject wood2;

	public bool woodnull;

	public Vector3 woodpos;

	public Vector3 woodpos2;

	private void Start()
	{
		if (locked)
		{
			IceDoor = Door;
			Door = null;
		}
		if (wood != null)
		{
			woodpos = wood.transform.localPosition;
		}
		if (wood != null)
		{
			woodpos2 = wood2.transform.localPosition;
		}
	}

	public override void Pickup(CharacterSystem character)
	{
		if (boarded && !locked)
		{
			if (wood == null && wood2 == null)
			{
				Door = IceDoor;
				boarded = false;
				if ((bool)Door)
				{
					Door.Access(character);
				}
				base.Pickup(character);
			}
			else
			{
				Debug.Log("border");
			}
		}
		else
		{
			if ((bool)Door)
			{
				Door.Access(character);
			}
			base.Pickup(character);
		}
	}

	public void KeyEnter()
	{
		if (!boarded)
		{
			locked = false;
			Door = IceDoor;
			zamok.AddComponent<Rigidbody>();
			Object.Destroy(zamok, 3.5f);
			Debug.Log("key");
		}
		else
		{
			locked = false;
			zamok.AddComponent<Rigidbody>();
			Object.Destroy(zamok, 3.5f);
		}
	}

	public void Update()
	{
		if (boarded && woodpos != wood.transform.localPosition && woodpos2 != wood2.transform.localPosition)
		{
			wood = null;
			wood2 = null;
		}
	}
}
