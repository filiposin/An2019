using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;

    public Transform upperSpine;

    public Transform headCamera;

    public Quaternion CameraRotation;

    private CharacterSystem character;

    private void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterSystem>();
        if (headCamera == null)
        {
            FPSCamera componentInChildren = GetComponentInChildren<FPSCamera>();
            headCamera = componentInChildren.gameObject.transform;
        }
    }

    private void Update()
    {
        if (animator == null || character == null || !upperSpine)
        {
            return;
        }

        CameraRotation = upperSpine.localRotation;
        CameraRotation.eulerAngles = new Vector3(upperSpine.localRotation.eulerAngles.x, upperSpine.localRotation.eulerAngles.y, 0f - headCamera.transform.rotation.eulerAngles.x);

        upperSpine.transform.localRotation = CameraRotation;

        if (animator.GetComponent<Animation>() && animator.GetComponent<Animation>()[animator.GetComponent<Animation>().clip.name])
        {
            animator.GetComponent<Animation>()[animator.GetComponent<Animation>().clip.name].AddMixingTransform(upperSpine);
        }
    }
}
