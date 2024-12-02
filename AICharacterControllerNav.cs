using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterSystem))]
public class AICharacterControllerNav : MonoBehaviour
{
    public string[] TargetTag = new string[1] { "Player" };

    public GameObject ObjectTarget;

    [HideInInspector]
    public Vector3 PositionTarget;

    [HideInInspector]
    public CharacterSystem character;

    [HideInInspector]
    public float DistanceAttack = 2f;

    public float DistanceMoveTo = 20f;

    public float TurnSpeed = 10f;

    public bool BrutalMode;

    public bool RushMode;

    public float PatrolRange = 10f;

    [HideInInspector]
    public Vector3 positionTemp;

    [HideInInspector]
    public int aiTime;

    [HideInInspector]
    public int aiState;

    private float attackTemp;

    public float AttackDelay = 0.5f;

    public float LifeTime = 30f;

    public float IdleSoundDelay = 10f;

    private float lifeTimeTemp;

    private float jumpTemp;

    private float jumpTime;

    private float soundTime;

    private float soundTimeDuration;

    public int JumpRate = 20;

    public NavAgent NavObject;

    private NavAgent nav;

    private Vector3 targetDirectiom;

    private void Start()
    {
        character = GetComponent<CharacterSystem>();
        positionTemp = transform.position;
        aiState = 0;
        attackTemp = Time.time;
        lifeTimeTemp = Time.time;
        jumpTemp = Time.time;
        soundTime = Time.time;
        soundTimeDuration = Random.Range(0f, IdleSoundDelay);
        character.ID = string.Empty;

        if (NavObject != null)
        {
            GameObject gameObject = Instantiate(NavObject.gameObject, transform.position, transform.rotation);
            nav = gameObject.GetComponent<NavAgent>();
            nav.Owner = gameObject;
        }
    }

    public void Attack(Vector3 targetDirectiom)
    {
        if (Time.time > attackTemp + AttackDelay)
        {
            Vector3[] direction = new Vector3[1] { targetDirectiom.normalized };
            character.DoDamage(transform.position + character.DamageOffset, direction, character.Damage, character.DamageLength, character.Penetrate, string.Empty, character.Team);
            character.AttackAnimation();
            attackTemp = Time.time;
        }
    }

    private void FrontObstacleChecker()
    {
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, direction, 1f))
        {
            if (Time.time >= jumpTemp + jumpTime)
            {
                character.Motor.inputJump = true;
                jumpTime = Random.Range(0, JumpRate) * 0.1f;
                jumpTemp = Time.time;
            }
            else
            {
                character.Motor.inputJump = false;
            }
        }
    }

    private void Update()
    {
        if (character == null)
        {
            return;
        }

        if (Time.time > soundTime + soundTimeDuration)
        {
            character.PlayIdleSound();
            soundTimeDuration = Random.Range(0f, IdleSoundDelay);
            soundTime = Time.time;
        }

        if (Time.time > lifeTimeTemp + LifeTime)
        {
            character.HP = 0;
            character.dieByLifeTime = true;
        }

        DistanceAttack = character.PrimaryWeaponDistance;
        float num = Vector3.Distance(PositionTarget, transform.position);
        targetDirectiom = PositionTarget - transform.position;

        Quaternion targetRotation = transform.rotation;
        float t = TurnSpeed * Time.deltaTime;

        if (targetDirectiom != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(targetDirectiom);
            targetRotation.x = 0f;
            targetRotation.z = 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
        }

        FrontObstacleChecker();

        if (ObjectTarget != null)
        {
            lifeTimeTemp = Time.time;
            PositionTarget = ObjectTarget.transform.position;

            if (aiTime <= 0)
            {
                aiState = Random.Range(0, 4);
                aiTime = Random.Range(10, 100);
            }
            else
            {
                aiTime--;
            }

            if (num <= DistanceAttack)
            {
                if (aiState == 0 || BrutalMode)
                {
                    Attack(targetDirectiom);
                }
            }
            else if (num <= DistanceMoveTo)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            }
            else
            {
                ObjectTarget = null;
                if (aiState == 0)
                {
                    aiState = 1;
                    aiTime = Random.Range(10, 500);
                    PositionTarget = positionTemp + new Vector3(Random.Range(0f - PatrolRange, PatrolRange), 0f, Random.Range(0f - PatrolRange, PatrolRange));
                }
            }
        }
        else
        {
            float num2 = float.MaxValue;
            for (int i = 0; i < TargetTag.Length; i++)
            {
                GameObject[] array = GameObject.FindGameObjectsWithTag(TargetTag[i]);
                if (array == null || array.Length <= 0)
                {
                    continue;
                }

                for (int j = 0; j < array.Length; j++)
                {
                    float num3 = Vector3.Distance(array[j].transform.position, transform.position);
                    if (num3 <= num2 && (num3 <= DistanceMoveTo || num3 <= DistanceAttack || RushMode) && ObjectTarget != array[j].gameObject)
                    {
                        num2 = num3;
                        ObjectTarget = array[j].gameObject;
                    }
                }
            }

            if (aiState == 0)
            {
                aiState = 1;
                aiTime = Random.Range(10, 200);
                PositionTarget = positionTemp + new Vector3(Random.Range(0f - PatrolRange, PatrolRange), 0f, Random.Range(0f - PatrolRange, PatrolRange));
            }

            if (aiTime <= 0)
            {
                aiState = Random.Range(0, 4);
                aiTime = Random.Range(10, 200);
            }
            else
            {
                aiTime--;
            }
        }

        Vector3 vector = PositionTarget;

        if (nav != null)
        {
            nav.SetTarget(PositionTarget);
            nav.navMeshAgent.speed = character.MoveSpeedMax;
            if (nav.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial && nav.navMeshAgent.hasPath)
            {
                vector = nav.transform.position;
            }
        }

        character.MoveToPosition(vector);
    }
}