using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
	private NavMeshAgent m_Agent;

	private RaycastHit m_HitInfo = default(RaycastHit);

	private void Start()
	{
		m_Agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
			{
				m_Agent.destination = m_HitInfo.point;
			}
		}
	}
}
