using UnityEngine;

public class DestroyTest : MonoBehaviour
{
    public float duration = 5f;

    private float temp;

    private void Start()
    {
        temp = Time.time;
        Object.DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Time.time > temp + duration)
        {
            Remove();
        }
    }

    private void Remove()
    {
        Destroy(gameObject);
    }
}