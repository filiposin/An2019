using UnityEngine;

public class SpectreCamera : MonoBehaviour
{
	public Vector3 LookAtPosition;

	public Vector3 PositionOffset = new Vector3(0f, 2f, 3f);

	public GameObject LookAtObject;

	private bool useragdoll;

	private bool lookAtSomething;

	private AudioListener audioList;

	private void Start()
	{
		LookAtPosition = base.transform.forward * 10f;
	}

	private void Awake()
	{
		audioList = GetComponent<AudioListener>();
		if ((bool)audioList)
		{
			audioList.enabled = GetComponent<Camera>().enabled;
		}
	}

	public void Active(bool active)
	{
		GetComponent<Camera>().enabled = active;
		if ((bool)audioList)
		{
			audioList.enabled = active;
		}
	}

	private void Update()
	{
		lookAtSomething = false;
		if ((bool)LookAtObject)
		{
			lookAtSomething = true;
			if (!useragdoll)
			{
				LookAtPosition = LookAtObject.transform.position;
			}
			RagdollReplace component = LookAtObject.GetComponent<RagdollReplace>();
			if ((bool)component)
			{
				useragdoll = true;
				lookAtSomething = true;
				LookAtPosition = component.RootRagdoll.transform.position;
			}
		}
		if (lookAtSomething)
		{
			Quaternion b = Quaternion.LookRotation(LookAtPosition - base.transform.position);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, Time.deltaTime * 10f);
			base.transform.position = LookAtPosition + PositionOffset;
		}
	}

	public void LookingAt(Vector3 position)
	{
		LookAtPosition = position;
	}

	public void LookingAtObject(GameObject obj)
	{
		LookAtObject = obj;
	}
}
