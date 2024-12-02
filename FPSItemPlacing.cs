using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FPSItemPlacing : FPSItemEquipment
{
    public bool HoldFire;

    public GameObject Item;

    public GameObject ItemIndicator;

    public float FireRate = 0.1f;

    public int UsingType;

    public ItemData ItemUsed;

    public bool InfinityAmmo;

    public bool OnAnimationEvent;

    public bool PlacingNormal = true;

    public AudioClip SoundPlaced;

    public float Ranged = 4f;

    public string[] KeyPair = new string[1] { string.Empty };

    private CharacterSystem character;

    private FPSController fpsController;

    private float timeTemp;

    private AudioSource audioSource;

    private Animator animator;

    private GameObject preplacingObject;

    private GameObject objectToSnap;

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
        if (ItemIndicator)
        {
            preplacingObject = Object.Instantiate(ItemIndicator.gameObject, transform.position, ItemIndicator.transform.rotation);
        }
    }

    private void OnDestroy()
    {
        if (preplacingObject)
        {
            Object.Destroy(preplacingObject);
        }
    }

    private void Update()
    {
        if (preplacingObject == null)
        {
            return;
        }
        RaycastHit raycastHit = GoundPlacing();
        if (raycastHit.distance != 0f)
        {
            preplacingObject.SetActive(true);
            if (objectToSnap != null)
            {
                preplacingObject.transform.position = objectToSnap.transform.position;
                preplacingObject.transform.rotation = objectToSnap.transform.rotation;
                return;
            }
            preplacingObject.transform.position = raycastHit.point;
            if (PlacingNormal)
            {
                preplacingObject.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
            }
        }
        else
        {
            preplacingObject.SetActive(false);
        }
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
                Use();
                timeTemp = Time.time;
            }
            base.Trigger();
        }
    }

    public override void OnAction()
    {
        RaycastHit raycastHit = GoundPlacing();
        if (raycastHit.distance != 0f)
        {
            if (Item)
            {
                Vector3 position = raycastHit.point;
                Quaternion rotation = Item.gameObject.transform.rotation;
                if (PlacingNormal)
                {
                    rotation = Quaternion.LookRotation(raycastHit.normal);
                }
                if (objectToSnap != null)
                {
                    position = objectToSnap.transform.position;
                    rotation = objectToSnap.transform.rotation;
                }

                // Удалены элементы мультиплеера
                GameObject gameObject2 = Object.Instantiate(Item, position, rotation);
                gameObject2.SendMessage("SetItemID", ItemID, SendMessageOptions.DontRequireReceiver);
                gameObject2.SendMessage("GenItemUID", SendMessageOptions.DontRequireReceiver);

                if (SoundPlaced && audioSource && audioSource.enabled)
                {
                    audioSource.PlayOneShot(SoundPlaced);
                }
            }
            if (ItemUsed != null && !InfinityAmmo && character != null && character.inventory != null && !character.inventory.RemoveItem(ItemUsed, 1))
            {
                return;
            }
        }
        base.OnAction();
    }

    private RaycastHit GoundPlacing()
    {
        float ranged = Ranged;
        RaycastHit[] array = Physics.RaycastAll(transform.position, transform.forward, ranged);
        for (int i = 0; i < array.Length; i++)
        {
            PlacingArea component = array[i].collider.GetComponent<PlacingArea>();
            if (component && component.KeyPairChecker(KeyPair))
            {
                if (component.Snap)
                {
                    objectToSnap = component.gameObject;
                }
                if (array[i].collider && component)
                {
                    return array[i];
                }
            }
        }
        objectToSnap = null;
        return default(RaycastHit);
    }
}