using UnityEngine;

public class MassiveAIController : MonoBehaviour
{
    public string PlayerTag = "Player";
    public AICharacterController[] ai;

    private void Start()
    {
    }

    private void Update()
    {
        ai = (AICharacterController[])Object.FindObjectsOfType(typeof(AICharacterController));

        for (int i = 0; i < ai.Length; i++)
        {
            ai[i].DistanceAttack = ai[i].character.PrimaryWeaponDistance;
            float distanceToTarget = Vector3.Distance(ai[i].PositionTarget, ai[i].transform.position);
            Vector3 direction = ai[i].PositionTarget - ai[i].transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation.x = 0f;
            targetRotation.z = 0f;
            float rotationSpeed = ai[i].TurnSpeed * Time.deltaTime;
            ai[i].transform.rotation = Quaternion.Lerp(ai[i].transform.rotation, targetRotation, rotationSpeed);

            if (ai[i].ObjectTarget != null)
            {
                ai[i].PositionTarget = ai[i].ObjectTarget.transform.position;

                if (ai[i].aiTime <= 0)
                {
                    ai[i].aiState = Random.Range(0, 4);
                    ai[i].aiTime = Random.Range(10, 100);
                }
                else
                {
                    ai[i].aiTime--;
                }

                if (distanceToTarget <= ai[i].DistanceAttack)
                {
                    if (ai[i].aiState == 0 || ai[i].BrutalMode)
                    {
                        ai[i].Attack(direction);
                    }
                }
                else if (distanceToTarget <= ai[i].DistanceMoveTo)
                {
                    ai[i].transform.rotation = Quaternion.Lerp(ai[i].transform.rotation, targetRotation, rotationSpeed);
                }
                else
                {
                    ai[i].ObjectTarget = null;

                    if (ai[i].aiState == 0)
                    {
                        ai[i].aiState = 1;
                        ai[i].aiTime = Random.Range(10, 500);
                        ai[i].PositionTarget = ai[i].positionTemp + new Vector3(
                            Random.Range(-ai[i].PatrolRange, ai[i].PatrolRange),
                            0f,
                            Random.Range(-ai[i].PatrolRange, ai[i].PatrolRange));
                    }
                }
            }
            else
            {
                float closestDistance = float.MaxValue;

                for (int j = 0; j < ai[i].TargetTag.Length; j++)
                {
                    GameObject[] targets = GameObject.FindGameObjectsWithTag(ai[i].TargetTag[j]);

                    for (int k = 0; k < targets.Length; k++)
                    {
                        float targetDistance = Vector3.Distance(targets[k].transform.position, ai[i].transform.position);

                        if (targetDistance <= closestDistance &&
                            (targetDistance <= ai[i].DistanceMoveTo || targetDistance <= ai[i].DistanceAttack || ai[i].RushMode) &&
                            ai[i].ObjectTarget != targets[k])
                        {
                            closestDistance = targetDistance;
                            ai[i].ObjectTarget = targets[k];
                        }
                    }
                }

                if (ai[i].aiState == 0)
                {
                    ai[i].aiState = 1;
                    ai[i].aiTime = Random.Range(10, 200);
                    ai[i].PositionTarget = ai[i].positionTemp + new Vector3(
                        Random.Range(-ai[i].PatrolRange, ai[i].PatrolRange),
                        0f,
                        Random.Range(-ai[i].PatrolRange, ai[i].PatrolRange));
                }

                if (ai[i].aiTime <= 0)
                {
                    ai[i].aiState = Random.Range(0, 4);
                    ai[i].aiTime = Random.Range(10, 200);
                }
                else
                {
                    ai[i].aiTime--;
                }
            }

            // Перемещаем AI к целевой позиции.
            ai[i].character.MoveToPosition(ai[i].PositionTarget);
        }
    }
}
