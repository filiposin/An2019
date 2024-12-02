using UnityEngine;

public class Ladder : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnTriggerStay(Collider player)
	{
		FPSController component = player.GetComponent<FPSController>();
		if ((bool)component)
		{
			component.Climb(component.inputDirection.z);
		}
	}
}
