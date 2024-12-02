using UnityEngine;

public class RagdollReplace : MonoBehaviour
{
	public float DisableDelay = 5f;

	public GameObject RootRagdoll;

	private float timeTemp;

	private void Start()
	{
		timeTemp = Time.time;
		if (RootRagdoll == null)
		{
			FindRoot(base.gameObject);
		}
	}

	private void FindRoot(GameObject dst)
	{
		foreach (Transform item in dst.transform)
		{
			if ((bool)item.GetComponent<Rigidbody>())
			{
				RootRagdoll = item.gameObject;
				break;
			}
			FindRoot(item.gameObject);
		}
	}

	private void FindRigidBody(GameObject dst)
	{
		if ((bool)dst.GetComponent<Rigidbody>())
		{
			dst.GetComponent<Rigidbody>().isKinematic = true;
		}
		if ((bool)dst.GetComponent<Collider>())
		{
			Object.Destroy(dst.GetComponent<Collider>());
		}
		foreach (Transform item in dst.transform)
		{
			FindRigidBody(item.gameObject);
		}
	}

	private void Update()
	{
		if (Time.time >= timeTemp + DisableDelay && (RootRagdoll == null || ((bool)RootRagdoll && RootRagdoll.GetComponent<Rigidbody>().velocity.sqrMagnitude <= 0.01f)))
		{
			FindRigidBody(base.gameObject);
			Object.Destroy(this);
		}
	}

	private void FixedUpdate()
	{
	}
}
