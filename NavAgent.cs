using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgent : MonoBehaviour
{
	public GameObject Owner;

	public float DistanceLimit = 1f;

	public NavMeshAgent navMeshAgent;

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.avoidancePriority = Random.Range(0, 100);
		Renderer component = GetComponent<Renderer>();
		if ((bool)component)
		{
			component.enabled = false;
		}
	}

	public void SetTarget(Vector3 pos)
	{
		navMeshAgent.SetDestination(pos);
	}

	private void Update()
	{
		if (Owner == null || Owner.transform.localScale == Vector3.one * 1E-06f)
		{
			Object.Destroy(base.gameObject);
		}
		else if (Vector3.Distance(Owner.transform.position, base.transform.position) >= DistanceLimit)
		{
			base.transform.position = Owner.transform.position;
		}
	}
}
