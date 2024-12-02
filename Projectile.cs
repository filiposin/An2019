using UnityEngine;

public class Projectile : DamageBase
{
    public float Duration = 3f;

    public GameObject Spawn;

    private float timeTemp;

    private bool isQuitting;

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (!isQuitting && Spawn && !Application.isLoadingLevel)
        {
            GameObject gameObject = Object.Instantiate(Spawn, base.transform.position, base.transform.rotation);
            DamageBase component = gameObject.GetComponent<DamageBase>();
            if (component)
            {
                component.OwnerID = OwnerID;
                component.OwnerTeam = OwnerTeam;
            }
        }
    }

    private void Start()
    {
        timeTemp = Time.time;
    }

    private void Update()
    {
        if (Time.time >= timeTemp + Duration)
        {
            OnDead();
        }
    }

    private void OnDead()
    {
        Object.Destroy(base.gameObject);
    }
}
