using UnityEngine;

public class node_ConditionalText : MonoBehaviour
{
	public GameObject demoText;

	private node_AIMovement myaiMovement;

	private void Start()
	{
		myaiMovement = GetComponent<node_AIMovement>();
	}

	private void Update()
	{
		if (myaiMovement.agent.speed == 0f && !demoText.activeSelf)
		{
			demoText.SetActive(true);
		}
		if (myaiMovement.agent.speed != 0f && demoText.activeSelf)
		{
			demoText.SetActive(false);
		}
	}
}
