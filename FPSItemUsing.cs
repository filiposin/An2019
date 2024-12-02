using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FPSItemUsing : FPSItemEquipment
{
    public bool HoldFire;

    public GameObject Item;

    public float FireRate = 0.1f;

    public int UsingType;

    public ItemData ItemUsed;

    public bool InfinityAmmo;

    public bool OnAnimationEvent;

    public AudioClip SoundUse;

    private CharacterSystem character;

    private FPSController fpsController;

    private float timeTemp;

    private AudioSource audioSource;

    private Animator animator;

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
        Debug.Log("Use item");
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
                Use();
                timeTemp = Time.time;
            }
            base.Trigger();
        }
    }

    public override void OnAction()
    {
        if (Item)
        {
            // Удалены элементы мультиплеера
            GameObject gameObject2 = Object.Instantiate(Item, transform.position, transform.rotation);
            gameObject2.transform.parent = character.transform;

            if (SoundUse && audioSource && audioSource.enabled)
            {
                audioSource.PlayOneShot(SoundUse);
            }
        }
        if (!(ItemUsed != null) || InfinityAmmo || !(character != null) || !(character.inventory != null) || character.inventory.RemoveItem(ItemUsed, 1))
        {
            base.OnAction();
        }
    }
}