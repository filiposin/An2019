using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterFootStep))]
[RequireComponent(typeof(FPSRayActive))]
public class CharacterSystem : DamageManager
{
    [HideInInspector]
    public string CharacterKey = string.Empty;

    [HideInInspector]
    public CharacterInventory inventory;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public FPSRayActive rayActive;

    [HideInInspector]
    public CharacterController controller;

    [HideInInspector]
    public CharacterMotor Motor;

    [HideInInspector]
    public bool Sprint;

    public float MoveSpeed = 0.7f;

    public float MoveSpeedMax = 5f;

    public float TurnSpeed = 5f;

    public float PrimaryWeaponDistance = 1f;

    public int PrimaryItemType;

    public int AttackType;

    public int Damage = 2;

    public float DamageLength = 1f;

    public int Penetrate = 1;

    public Vector3 DamageOffset = Vector3.up;

    public AudioClip[] DamageSound;

    public AudioClip[] SoundIdle;

    [HideInInspector]
    public float spdMovAtkMult = 1f;

    private void Awake()
    {
        SetupAwake();
    }

    public void SetupAwake()
    {
        Object.DontDestroyOnLoad(base.gameObject);
        Motor = GetComponent<CharacterMotor>();
        controller = GetComponent<CharacterController>();
        Audiosource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rayActive = GetComponent<FPSRayActive>();
        inventory = GetComponent<CharacterInventory>();
        spdMovAtkMult = 1f;
    }

    private void Update()
    {
        UpdateFunction();
    }

    public void UpdateFunction()
    {
        Motor.ID = ID;
        // ׃האכÿול גûחמג UpdatePosition
        DamageUpdate();
    }

    public virtual void PlayAttackAnimation(bool attacking, int attacktype)
    {
    }

    public virtual void PlayMoveAnimation(float magnitude)
    {
    }

    public void MoveAnimation()
    {
        PlayMoveAnimation(Motor.OjectVelocity.magnitude);
    }

    public void MoveTo(Vector3 dir)
    {
        float num = MoveSpeed;
        if (Sprint)
        {
            num = MoveSpeedMax;
        }
        Move(dir * num * spdMovAtkMult);
        MoveAnimation();
    }

    public void MoveToPosition(Vector3 position)
    {
        float num = MoveSpeed;
        if (Sprint)
        {
            num = MoveSpeedMax;
        }
        Vector3 vector = position - base.transform.position;
        vector = Vector3.ClampMagnitude(vector, 1f);
        vector.y = 0f;
        Move(vector.normalized * num * vector.magnitude * spdMovAtkMult);
        if (vector != Vector3.zero)
        {
            Quaternion b = Quaternion.LookRotation(vector);
            base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * TurnSpeed * vector.magnitude);
        }
        MoveAnimation();
    }

    public void AttackAnimation(int attacktype)
    {
        AttackType = attacktype;
        attackAnimation(attacktype);
    }

    public void AttackTo(Vector3 direction, int attacktype)
    {
        attackTo(direction, attacktype);
    }

    private void attackAnimation(int attacktype)
    {
        PlayAttackAnimation(true, attacktype);
    }

    private void attackTo(Vector3 direction, int attacktype)
    {
        PlayAttackAnimation(true, attacktype);
    }

    public void AttackAnimation()
    {
        attackAnimation(AttackType);
    }

    public void DoDamage(Vector3 origin, Vector3[] direction, int damage, float distance, int penetrate, string id, string team)
    {
        doDamage(origin, direction, damage, distance, penetrate, id, team);
    }

    public void DoDamage()
    {
        Vector3[] direction = new Vector3[1] { base.transform.forward };
        doDamage(base.transform.position + DamageOffset, direction, Damage, DamageLength, Penetrate, ID, Team);
    }

    private void doDamage(Vector3 origin, Vector3[] direction, int damage, float distance, int penetrate, string id, string team)
    {
        if ((bool)rayActive && rayActive.ShootRay(origin, direction, damage, distance, penetrate, id, team))
        {
            PlayDamageSound();
        }
        if ((bool)inventory && !IsMine)
        {
            inventory.EquipmentOnAction();
        }
    }

    public void DoOverlapDamage(Vector3 origin, Vector3 direction, int damage, float distance, float dot, string id, string team)
    {
        doOverlapDamage(origin, direction, damage, distance, dot, id, team);
    }

    private void doOverlapDamage(Vector3 origin, Vector3 direction, int damage, float distance, float dot, string id, string team)
    {
        if ((bool)rayActive && rayActive.Overlap(origin, direction, damage, distance, dot, id, team))
        {
            PlayDamageSound();
        }
        if ((bool)inventory && !IsMine)
        {
            inventory.EquipmentOnAction();
        }
    }

    public void Interactive(Vector3 origin, Vector3 direction)
    {
        interactive(origin, direction);
    }

    public void Checking(Vector3 origin, Vector3 direction)
    {
        if ((bool)rayActive)
        {
            rayActive.CheckingRay(origin, direction);
        }
    }

    private void interactive(Vector3 origin, Vector3 direction)
    {
        if ((bool)rayActive)
        {
            rayActive.ActiveRay(origin, direction);
        }
    }

    public void Move(Vector3 directionVector)
    {
        if ((bool)Motor)
        {
            Motor.inputMoveDirection = directionVector;
        }
    }

    public void PlayIdleSound()
    {
        if ((bool)Audiosource && SoundIdle.Length > 0)
        {
            Audiosource.PlayOneShot(SoundIdle[Random.Range(0, SoundIdle.Length)]);
        }
    }

    public void PlayDamageSound()
    {
        if ((bool)Audiosource && DamageSound.Length > 0)
        {
            Audiosource.PlayOneShot(DamageSound[Random.Range(0, DamageSound.Length)]);
        }
    }

    public string ConvertArrayToString(Vector3[] vector3s)
    {
        string text = string.Empty;
        for (int i = 0; i < vector3s.Length; i++)
        {
            string text2 = text;
            text = text2 + vector3s[i].x + "," + vector3s[i].y + "," + vector3s[i].z;
            if (i < vector3s.Length - 1)
            {
                text += "|";
            }
        }
        return text;
    }

    public Vector3[] ReadArrayFromString(string vector3stext)
    {
        string[] array = vector3stext.Split("|"[0]);
        Vector3[] array2 = new Vector3[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            string[] array3 = array[i].Split(","[0]);
            array2[i].x = float.Parse(array3[0]);
            array2[i].y = float.Parse(array3[1]);
            array2[i].z = float.Parse(array3[2]);
        }
        return array2;
    }

    public virtual void OnThisThingDead()
    {
    }
}
