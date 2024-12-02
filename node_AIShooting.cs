using System.Collections;
using UnityEngine;

public class node_AIShooting : MonoBehaviour
{
	[Tooltip("Main Interaction layers. Select all enemy, player and level geometry layers. Typically only 'default', 'localPlayer' and 'enemy' need to be selected.")]
	public LayerMask interactLayers;

	[Tooltip("damage dealt to player when hit.")]
	public float baseDamage = 1f;

	[Tooltip("An audio clip to play when a shot happens.")]
	public AudioClip shotClip;

	[Tooltip("Reference to the laser shot line renderer.")]
	public LineRenderer laserShotLine;

	[Tooltip("how fast enemy fires weapon.")]
	public float fireRate = 0.75f;

	[Tooltip("use this to raise or lower where enemy's laser hits player.")]
	public float playerHeightOffSet = 0.4f;

	private Transform player;

	private node_AIMovement aiM;

	private bool attacking;

	private Animator myAnim;

	private void Awake()
	{
		if ((bool)GameObject.FindGameObjectWithTag("Player"))
		{
			player = GameObject.FindGameObjectWithTag("Player").transform;
		}
		laserShotLine.enabled = false;
		aiM = GetComponent<node_AIMovement>();
		if (GetComponent<Animator>() != null)
		{
			myAnim = GetComponent<Animator>();
		}
	}

	private void Update()
	{
		if (!attacking && aiM.attackOk)
		{
			InvokeRepeating("Shoot", fireRate, fireRate);
			attacking = true;
			if (myAnim != null)
			{
				myAnim.SetBool("Fire", true);
			}
		}
		if (attacking && !aiM.attackOk)
		{
			CancelInvoke();
			attacking = false;
			if (myAnim != null)
			{
				myAnim.SetBool("Fire", false);
			}
		}
		if (laserShotLine.enabled)
		{
			laserShotLine.SetPosition(0, laserShotLine.transform.position);
			laserShotLine.SetPosition(1, new Vector3(player.position.x, player.position.y + playerHeightOffSet, player.position.z));
		}
	}

	private void Shoot()
	{
		RaycastHit hitInfo;
		if (Physics.Linecast(base.transform.position, new Vector3(player.position.x, player.position.y + 0.65f, player.position.z), out hitInfo, interactLayers) && hitInfo.collider.gameObject.layer == player.gameObject.layer)
		{
			ShotEffects();
			hitInfo.collider.SendMessageUpwards("Damage", baseDamage, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void ShotEffects()
	{
		laserShotLine.enabled = true;
		StartCoroutine("laserKill", 0.2f);
		AudioSource.PlayClipAtPoint(shotClip, base.transform.position);
	}

	private IEnumerator laserKill(float chillTime)
	{
		yield return new WaitForSeconds(chillTime);
		laserShotLine.enabled = false;
	}
}
