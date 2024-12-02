using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Character/Character Motor")]
public class CharacterMotor : MonoBehaviour
{
    public class CharacterMotorMovement
    {
        [NonSerialized]
        public CollisionFlags collisionFlags;

        [NonSerialized]
        public Vector3 velocity;

        [NonSerialized]
        public Vector3 frameVelocity = Vector3.zero;

        [NonSerialized]
        public Vector3 hitPoint = Vector3.zero;

        [NonSerialized]
        public Vector3 lastHitPoint = new Vector3(float.PositiveInfinity, 0f, 0f);
    }

    private enum MovementTransferOnJump
    {
        None = 0,
        InitTransfer = 1,
        PermaTransfer = 2,
        PermaLocked = 3
    }

    private class CharacterMotorJumping
    {
        public bool enabled = true;

        public float baseHeight = 1.6f;

        public float extraHeight = 1.6f;

        public float perpAmount = 2f;

        public float steepPerpAmount = 1.5f;

        [NonSerialized]
        public bool jumping;

        [NonSerialized]
        public bool holdingJumpButton;

        [NonSerialized]
        public float lastStartTime;

        [NonSerialized]
        public float lastButtonDownTime = -100f;

        [NonSerialized]
        public Vector3 jumpDir = Vector3.up;
    }

    private class CharacterMotorMovingPlatform
    {
        public bool enabled = true;

        public MovementTransferOnJump movementTransfer = MovementTransferOnJump.PermaTransfer;

        [NonSerialized]
        public Transform hitPlatform;

        [NonSerialized]
        public Transform activePlatform;

        [NonSerialized]
        public Vector3 activeLocalPoint;

        [NonSerialized]
        public Vector3 activeGlobalPoint;

        [NonSerialized]
        public Quaternion activeLocalRotation;

        [NonSerialized]
        public Quaternion activeGlobalRotation;

        [NonSerialized]
        public Matrix4x4 lastMatrix;

        [NonSerialized]
        public Vector3 platformVelocity;

        [NonSerialized]
        public bool newPlatform;
    }

    private class CharacterMotorSliding
    {
        public bool enabled = true;

        public float slidingSpeed = 15f;

        public float sidewaysControl = 1f;

        public float speedControl = 0.4f;
    }

    public bool canControl = true;

    public float boostMults = 1f;

    public float maxForwardSpeed = 5f;

    public float maxSidewaysSpeed = 5f;

    public float maxBackwardsSpeed = 5f;

    private bool useFixedUpdate = true;

    public Vector3 VelocityDirection;

    public AnimationCurve slopeSpeedMultiplier = new AnimationCurve(new Keyframe(-90f, 1f), new Keyframe(0f, 1f), new Keyframe(90f, 0f));

    public float maxGroundAcceleration = 30f;

    public float maxAirAcceleration = 20f;

    public float gravity = 10f;

    public float maxFallSpeed = 20f;

    [NonSerialized]
    public Vector3 inputMoveDirection = Vector3.zero;

    [NonSerialized]
    public bool inputJump;

    public CharacterMotorMovement movement = new CharacterMotorMovement();

    public string ID;

    private CharacterMotorJumping jumping = new CharacterMotorJumping();

    private CharacterMotorMovingPlatform movingPlatform = new CharacterMotorMovingPlatform();

    private CharacterMotorSliding sliding = new CharacterMotorSliding();

    [NonSerialized]
    public bool grounded = true;

    [NonSerialized]
    public Vector3 groundNormal = Vector3.zero;

    [HideInInspector]
    public Vector3 OjectVelocity;

    private Vector3 lastGroundNormal = Vector3.zero;

    private Transform tr;

    public CharacterController controller;

    private bool climb;

    public bool IsActive;

    private bool ongroundtest;

    private void Awake()
    {
        controller = base.gameObject.GetComponent<CharacterController>();
        tr = base.transform;
    }

    public void Climb(float speed)
    {
        climb = true;
        controller.Move(Vector3.up * speed * Time.deltaTime);
    }

    private void UpdateFunction()
    {
        Vector3 velocity = movement.velocity;
        if (!controller.enabled && ID != UnitZ.gameManager.PlayerID)
        {
            IsActive = false;
            return;
        }
        IsActive = true;
        velocity = ApplyInputVelocityChange(velocity);
        velocity = ApplyGravityAndJumping(velocity);
        Vector3 zero = Vector3.zero;
        if (MoveWithPlatform())
        {
            Vector3 vector = movingPlatform.activePlatform.TransformPoint(movingPlatform.activeLocalPoint);
            zero = vector - movingPlatform.activeGlobalPoint;
            if (zero != Vector3.zero)
            {
                VelocityDirection = zero;
                controller.Move(zero);
            }
            Quaternion quaternion = movingPlatform.activePlatform.rotation * movingPlatform.activeLocalRotation;
            float y = (quaternion * Quaternion.Inverse(movingPlatform.activeGlobalRotation)).eulerAngles.y;
            if (y != 0f)
            {
                tr.Rotate(0f, y, 0f);
            }
        }
        Vector3 position = tr.position;
        Vector3 vector2 = velocity * Time.deltaTime;
        float num = Mathf.Max(controller.stepOffset, new Vector3(vector2.x, 0f, vector2.z).magnitude);
        if (grounded)
        {
            vector2 -= num * Vector3.up;
        }
        if (climb)
        {
            vector2.y = 0f;
            float num2 = Vector3.Dot(base.transform.forward, velocity.normalized);
            if (num2 < 0f && !ongroundtest)
            {
                vector2.x = 0f;
                vector2.z = 0f;
            }
        }
        movingPlatform.hitPlatform = null;
        groundNormal = Vector3.zero;
        VelocityDirection = vector2;
        movement.collisionFlags = controller.Move(vector2);
        movement.lastHitPoint = movement.hitPoint;
        lastGroundNormal = groundNormal;
        if (movingPlatform.enabled && movingPlatform.activePlatform != movingPlatform.hitPlatform && movingPlatform.hitPlatform != null)
        {
            movingPlatform.activePlatform = movingPlatform.hitPlatform;
            movingPlatform.lastMatrix = movingPlatform.hitPlatform.localToWorldMatrix;
            movingPlatform.newPlatform = true;
        }
        Vector3 vector3 = new Vector3(velocity.x, 0f, velocity.z);
        movement.velocity = (tr.position - position) / Time.deltaTime;
        Vector3 lhs = new Vector3(movement.velocity.x, 0f, movement.velocity.z);
        if (vector3 == Vector3.zero)
        {
            movement.velocity = new Vector3(0f, movement.velocity.y, 0f);
        }
        else
        {
            float value = Vector3.Dot(lhs, vector3) / vector3.sqrMagnitude;
            movement.velocity = vector3 * Mathf.Clamp01(value) + movement.velocity.y * Vector3.up;
        }
        if ((double)movement.velocity.y < (double)velocity.y - 0.001)
        {
            if (movement.velocity.y < 0f)
            {
                movement.velocity.y = velocity.y;
            }
            else
            {
                jumping.holdingJumpButton = false;
            }
        }
        if (grounded && !IsGroundedTest())
        {
            grounded = false;
            if (movingPlatform.enabled && (movingPlatform.movementTransfer == MovementTransferOnJump.InitTransfer || movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer))
            {
                movement.frameVelocity = movingPlatform.platformVelocity;
                movement.velocity += movingPlatform.platformVelocity;
            }
            SendMessage("OnFall", SendMessageOptions.DontRequireReceiver);
            tr.position += num * Vector3.up;
        }
        else if (!grounded && IsGroundedTest())
        {
            grounded = true;
            jumping.jumping = false;
            SubtractNewPlatformVelocity();
            SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);
        }
        if (MoveWithPlatform())
        {
            movingPlatform.activeGlobalPoint = tr.position + Vector3.up * (controller.center.y - controller.height * 0.5f + controller.radius);
            movingPlatform.activeLocalPoint = movingPlatform.activePlatform.InverseTransformPoint(movingPlatform.activeGlobalPoint);
            movingPlatform.activeGlobalRotation = tr.rotation;
            movingPlatform.activeLocalRotation = Quaternion.Inverse(movingPlatform.activePlatform.rotation) * movingPlatform.activeGlobalRotation;
        }
        ongroundtest = false;
        climb = false;
        OjectVelocity = controller.velocity;
    }

    private void FixedUpdate()
    {
        if (movingPlatform.enabled)
        {
            if (movingPlatform.activePlatform != null)
            {
                if (!movingPlatform.newPlatform)
                {
                    movingPlatform.platformVelocity = (movingPlatform.activePlatform.localToWorldMatrix.MultiplyPoint3x4(movingPlatform.activeLocalPoint) - movingPlatform.lastMatrix.MultiplyPoint3x4(movingPlatform.activeLocalPoint)) / Time.deltaTime;
                }
                movingPlatform.lastMatrix = movingPlatform.activePlatform.localToWorldMatrix;
                movingPlatform.newPlatform = false;
            }
            else
            {
                movingPlatform.platformVelocity = Vector3.zero;
            }
        }
        if (useFixedUpdate)
        {
            UpdateFunction();
        }
    }

    private void Update()
    {
        if (!useFixedUpdate)
        {
            UpdateFunction();
        }
    }

    private Vector3 ApplyInputVelocityChange(Vector3 velocity)
    {
        if (!canControl)
        {
            inputMoveDirection = Vector3.zero;
        }
        Vector3 normalized;
        if (grounded && TooSteep())
        {
            normalized = new Vector3(groundNormal.x, 0f, groundNormal.z).normalized;
            Vector3 vector = Vector3.Project(inputMoveDirection, normalized);
            normalized = normalized + vector * sliding.speedControl + (inputMoveDirection - vector) * sliding.sidewaysControl;
            normalized *= sliding.slidingSpeed;
        }
        else
        {
            normalized = GetDesiredHorizontalVelocity();
        }
        if (movingPlatform.enabled && movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer)
        {
            normalized += movement.frameVelocity * 10f;
            normalized.y = 0f;
        }
        if (grounded)
        {
            normalized = AdjustGroundVelocityToNormal(normalized, groundNormal);
        }
        else
        {
            velocity.y = 0f;
        }
        float num = GetMaxAcceleration(grounded) * Time.deltaTime;
        Vector3 vector2 = normalized - velocity;
        if (vector2.sqrMagnitude > num * num)
        {
            vector2 = vector2.normalized * num;
        }
        if (grounded || canControl)
        {
            velocity += vector2;
        }
        if (grounded)
        {
            velocity.y = Mathf.Min(velocity.y, 0f);
        }
        return velocity;
    }

    private Vector3 ApplyGravityAndJumping(Vector3 velocity)
    {
        if (!inputJump || !canControl)
        {
            jumping.holdingJumpButton = false;
            jumping.lastButtonDownTime = -100f;
        }
        if (inputJump && jumping.lastButtonDownTime < 0f && canControl)
        {
            jumping.lastButtonDownTime = Time.time;
        }
        if (grounded)
        {
            velocity.y = Mathf.Min(0f, velocity.y) - gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = movement.velocity.y - gravity * Time.deltaTime * 2f;
            if (jumping.jumping && jumping.holdingJumpButton && Time.time < jumping.lastStartTime + jumping.extraHeight / CalculateJumpVerticalSpeed(jumping.baseHeight))
            {
                velocity += jumping.jumpDir * gravity * Time.deltaTime;
            }
            velocity.y = Mathf.Max(velocity.y, 0f - maxFallSpeed);
        }
        if (grounded)
        {
            if (jumping.enabled && canControl && (double)(Time.time - jumping.lastButtonDownTime) < 0.2)
            {
                grounded = false;
                jumping.jumping = true;
                jumping.lastStartTime = Time.time;
                jumping.lastButtonDownTime = -100f;
                jumping.holdingJumpButton = true;
                if (TooSteep())
                {
                    jumping.jumpDir = Vector3.Slerp(Vector3.up, groundNormal, jumping.steepPerpAmount);
                }
                else
                {
                    jumping.jumpDir = Vector3.Slerp(Vector3.up, groundNormal, jumping.perpAmount);
                }
                velocity.y = 0f;
                velocity += jumping.jumpDir * CalculateJumpVerticalSpeed(jumping.baseHeight);
                if (movingPlatform.enabled && (movingPlatform.movementTransfer == MovementTransferOnJump.InitTransfer || movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer))
                {
                    movement.frameVelocity = movingPlatform.platformVelocity;
                    velocity += movingPlatform.platformVelocity;
                }
                SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                jumping.holdingJumpButton = false;
            }
        }
        return velocity;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0f && hit.normal.y > groundNormal.y && hit.moveDirection.y < 0f)
        {
            if ((double)(hit.point - movement.lastHitPoint).sqrMagnitude > 0.001 || lastGroundNormal == Vector3.zero)
            {
                groundNormal = hit.normal;
            }
            else
            {
                groundNormal = lastGroundNormal;
            }
            movingPlatform.hitPlatform = hit.collider.transform;
            movement.hitPoint = hit.point;
            movement.frameVelocity = Vector3.zero;
            ongroundtest = true;
        }
    }

    private IEnumerable SubtractNewPlatformVelocity()
    {
        if (!movingPlatform.enabled || (movingPlatform.movementTransfer != MovementTransferOnJump.InitTransfer && movingPlatform.movementTransfer != MovementTransferOnJump.PermaTransfer))
        {
            yield break;
        }
        if (movingPlatform.newPlatform)
        {
            Transform platform = movingPlatform.activePlatform;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            if (grounded && platform == movingPlatform.activePlatform)
            {
                yield return 1;
            }
        }
        movement.velocity -= movingPlatform.platformVelocity;
    }

    private bool MoveWithPlatform()
    {
        return movingPlatform.enabled && (grounded || movingPlatform.movementTransfer == MovementTransferOnJump.PermaLocked) && movingPlatform.activePlatform != null;
    }

    private Vector3 GetDesiredHorizontalVelocity()
    {
        Vector3 vector = tr.InverseTransformDirection(inputMoveDirection);
        float num = MaxSpeedInDirection(vector);
        if (grounded)
        {
            float time = Mathf.Asin(movement.velocity.normalized.y) * 57.29578f;
            num *= slopeSpeedMultiplier.Evaluate(time);
        }
        return tr.TransformDirection(vector * num);
    }

    private Vector3 AdjustGroundVelocityToNormal(Vector3 hVelocity, Vector3 groundNormal)
    {
        Vector3 lhs = Vector3.Cross(Vector3.up, hVelocity);
        return Vector3.Cross(lhs, groundNormal).normalized * hVelocity.magnitude;
    }

    private bool IsGroundedTest()
    {
        return (double)groundNormal.y > 0.01;
    }

    private float GetMaxAcceleration(bool grounded)
    {
        return grounded ? maxGroundAcceleration : maxAirAcceleration;
    }

    private float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        return Mathf.Sqrt(2f * targetJumpHeight * gravity);
    }

    private bool IsJumping()
    {
        return jumping.jumping;
    }

    private bool IsSliding()
    {
        return grounded && sliding.enabled && TooSteep();
    }

    private bool IsTouchingCeiling()
    {
        return (movement.collisionFlags & CollisionFlags.Above) != 0;
    }

    private bool IsGrounded()
    {
        return grounded;
    }

    private bool TooSteep()
    {
        return groundNormal.y <= Mathf.Cos(controller.slopeLimit * ((float)Math.PI / 180f));
    }

    private Vector3 GetDirection()
    {
        return inputMoveDirection;
    }

    private void SetControllable(bool controllable)
    {
        canControl = controllable;
    }

    private float MaxSpeedInDirection(Vector3 desiredMovementDirection)
    {
        if (desiredMovementDirection == Vector3.zero)
        {
            return 0f;
        }
        float num = ((desiredMovementDirection.z > 0f) ? (maxForwardSpeed * boostMults) : (maxBackwardsSpeed * boostMults)) / maxSidewaysSpeed * boostMults;
        Vector3 normalized = new Vector3(desiredMovementDirection.x, 0f, desiredMovementDirection.z / num).normalized;
        return new Vector3(normalized.x, 0f, normalized.z * num).magnitude * maxSidewaysSpeed * boostMults;
    }

    private void SetVelocity(Vector3 velocity)
    {
        grounded = false;
        movement.velocity = velocity;
        movement.frameVelocity = Vector3.zero;
        SendMessage("OnExternalVelocity");
    }
}
