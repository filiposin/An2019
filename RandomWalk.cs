using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RandomWalk : MonoBehaviour
{
	public float m_Range = 25f;

	private NavMeshAgent m_agent;

	private void Start()
	{
		m_agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (!m_agent.pathPending && !(m_agent.remainingDistance > 0.1f))
		{
			m_agent.destination = m_Range * Random.insideUnitCircle;
		}
	}
}
