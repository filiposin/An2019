using System;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
	public float m_Amplitude = 1f;

	public float m_Period = 1f;

	public Vector3 m_Direction = Vector3.up;

	private Vector3 m_StartPosition;

	private void Start()
	{
		m_StartPosition = base.transform.position;
	}

	private void Update()
	{
		Vector3 position = m_StartPosition + m_Direction * m_Amplitude * Mathf.Sin((float)Math.PI * 2f * Time.time / m_Period);
		base.transform.position = position;
	}
}
