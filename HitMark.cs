using UnityEngine;

public class HitMark : MonoBehaviour
{
	public DamageManager DamageManage;

	public GameObject HitFX;

	public float DamageMult = 1f;

	public bool Freeze = true;

	private void Start()
	{
		if ((bool)base.transform.root)
		{
			DamageManage = base.transform.GetComponent<DamageManager>();
		}
		else
		{
			DamageManage = base.transform.GetComponent<DamageManager>();
		}
	}

	public void OnHit(DamagePackage pack)
	{
		if ((bool)DamageManage)
		{
			DamageManage.ApplyDamage((int)((float)pack.Damage * DamageMult), pack.Direction, pack.ID, pack.Team);
			if (UnitZ.gameManager != null && UnitZ.gameManager.PlayerID == pack.ID && UnitZ.playerManager.playingCharacter != null && UnitZ.playerManager.playingCharacter.inventory != null && UnitZ.playerManager.playingCharacter.inventory.FPSEquipment != null && (bool)UnitZ.playerManager.playingCharacter.inventory.FPSEquipment.GetComponent<Crosshair>())
			{
				UnitZ.playerManager.playingCharacter.inventory.FPSEquipment.GetComponent<Crosshair>().Hit();
			}
		}
		if (!Freeze)
		{
			base.transform.position += pack.Direction.normalized;
		}
		ParticleFX(pack.Position, pack.Normal);
	}

	public void ParticleFX(Vector3 position, Vector3 normal)
	{
		if ((bool)HitFX)
		{
			GameObject gameObject = Object.Instantiate(HitFX, position, Quaternion.identity);
			gameObject.transform.forward = normal;
			Object.Destroy(gameObject, 3f);
		}
	}
}
