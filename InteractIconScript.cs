using UnityEngine;

public class InteractIconScript : MonoBehaviour
{

    public GameObject InteractIcon;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Character_Salary(Clone)")
        {
            InteractIcon.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Character_Salary(Clone)")
        {
            InteractIcon.SetActive(false);
        }
    }
}
