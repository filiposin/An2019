using UnityEngine;
using System.Collections;

public class attacpl : MonoBehaviour
{
    private GameObject enemy;
    private GameObject Sound;
    private GameObject Sound2;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip catchSound;
    private GameObject CharacterMotorOBJ;
    private GameObject _player;

    private void Start()
    {
        CharacterMotorOBJ = GameObject.Find("Character_Salary(Clone)");
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Character_Salary(Clone)")
        {
            CharacterMotorOBJ.GetComponent<CharacterMotor>().canControl = false;
            audioSource.PlayOneShot(catchSound);
            StartCoroutine(CatchRoutine());
        }
    }

    public IEnumerator CatchRoutine()
    {
        Destroy(_player);
        enemy = GameObject.FindWithTag("Enemy");
        Sound = GameObject.FindWithTag("sound");
        Sound2 = GameObject.FindWithTag("sound2");
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }
        Sound.SetActive(false);
        Sound.SetActive(true);
        Sound2.SetActive(false);
        Sound2.SetActive(true);
        Object.Destroy(enemy.gameObject);
        yield return null; // Pause for one frame
    }
}
