using UnityEngine;

public class EnableIffSleeping : MonoBehaviour
{
	public Behaviour m_Behaviour;

	private Rigidbody m_Rigidbody;

	private void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (!(m_Rigidbody == null) && !(m_Behaviour == null))
		{
			if (m_Rigidbody.IsSleeping() && !m_Behaviour.enabled)
			{
				m_Behaviour.enabled = true;
			}
			if (!m_Rigidbody.IsSleeping() && m_Behaviour.enabled)
			{
				m_Behaviour.enabled = false;
			}
		}
	}
}
