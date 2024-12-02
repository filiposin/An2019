using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FPSItemThrow : FPSItemEquipment
{
    public bool HoldFire;

    public GameObject Item;

    public float FireRate = 0.1f;

    public int UsingType;

    public ItemData ItemUsed;

    public bool InfinityAmmo;

    public bool OnAnimationEvent;

    public float Force1 = 15f;

    public float Force2 = 5f;

    public AudioClip SoundThrow;

    private CharacterSystem character;

    private FPSController fpsController;

    private float timeTemp;

    private AudioSource audioSource;

    private Animator animator;

    private int throwType;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (transform.root)
        {
            character = transform.root.GetComponent<CharacterSystem>();
            fpsController = transform.root.GetComponent<FPSController>();
            if (character == null)
            {
                character = transform.root.GetComponentInChildren<CharacterSystem>();
            }
            if (fpsController == null)
            {
                fpsController = transform.root.GetComponentInChildren<FPSController>();
            }
        }
        else
        {
            character = GetComponent<CharacterSystem>();
            fpsController = GetComponent<FPSController>();
        }
        timeTemp = Time.time;
    }

    private void Update()
    {
    }

    private void Use()
    {
        if (!(ItemUsed != null) || InfinityAmmo || !(character != null) || !(character.inventory != null) || character.inventory.CheckItem(ItemUsed, 1))
        {
            if (!OnAnimationEvent)
            {
                OnAction();
            }
            if (animator)
            {
                animator.SetInteger("shoot_type", UsingType);
                animator.SetTrigger("shoot");
            }
            if (character != null)
            {
                character.AttackAnimation(UsingType);
            }
        }
    }

    public override void Trigger()
    {
        if (HoldFire || !OnFire1)
        {
            if (character && fpsController && Time.time > timeTemp + FireRate)
            {
                throwType = 0;
                Use();
                timeTemp = Time.time;
            }
            base.Trigger();
        }
    }

    public override void Trigger2()
    {
        if (HoldFire || !OnFire2)
        {
            if (character && fpsController && Time.time > timeTemp + FireRate)
            {
                throwType = 1;
                Use();
                timeTemp = Time.time;
            }
            base.Trigger2();
        }
    }

    public override void OnAction()
    {
        if (Item)
        {
            GameObject gameObject = Object.Instantiate(Item, transform.position, transform.rotation);
            DamageBase component = gameObject.GetComponent<DamageBase>();
            if (component && character && UnitZ.gameManager)
            {
                component.OwnerID = character.ID;
                component.OwnerTeam = character.Team;
            }
            if (throwType == 0)
            {
                if (gameObject.GetComponent<Rigidbody>())
                {
                    gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * Force1, ForceMode.Impulse);
                }
            }
            else if (gameObject.GetComponent<Rigidbody>())
            {
                gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * Force2, ForceMode.Impulse);
            }

            if (SoundThrow && audioSource && audioSource.enabled)
            {
                audioSource.PlayOneShot(SoundThrow);
            }
        }
        if (!(ItemUsed != null) || InfinityAmmo || !(character != null) || !(character.inventory != null) || character.inventory.RemoveItem(ItemUsed, 1))
        {
            base.OnAction();
        }
    }
}