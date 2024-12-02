using UnityEngine;

public class node_AIHealth : MonoBehaviour
{
	public float HP = 8f;

	public GameObject spawnOnDeath;

	private node_AIMovement aim;

	private void Awake()
	{
		aim = GetComponent<node_AIMovement>();
	}

	public void Damage(float damage)
	{
		HP -= damage;
		if (HP <= 0f)
		{
			if ((bool)spawnOnDeath)
			{
				Object.Instantiate(spawnOnDeath, base.transform.position, base.transform.rotation);
			}
			Object.Destroy(base.gameObject);
		}
		else if (!aim.chase)
		{
			if (aim.playerVisible())
			{
				aim.pursuePlayer(true);
			}
			else
			{
				aim.pursuePlayer(false);
			}
		}
	}
}
