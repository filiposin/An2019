using UnityEngine;

[RequireComponent(typeof(CharacterSystem))]
public class CharacterLiving : MonoBehaviour
{
    public int Hungry = 100;
    public int HungryMax = 100;
    public int Water = 100;
    public int WaterMax = 100;

    [HideInInspector]
    public CharacterSystem character;

    private void Start()
    {
        character = GetComponent<CharacterSystem>();
        InvokeRepeating("stomachUpdate", 1f, 1f);
        InvokeRepeating("hungryUpdate", 1f, 15f);
        InvokeRepeating("thirstilyUpdate", 1f, 10f);
    }

    private void Update()
    {
        if (Hungry < 0)
        {
            Hungry = 0;
        }
        if (Hungry > HungryMax)
        {
            Hungry = HungryMax;
        }
        if (Water < 0)
        {
            Water = 0;
        }
        if (Water > WaterMax)
        {
            Water = WaterMax;
        }
    }

    public void stomachUpdate()
    {
        if (character == null)
        {
            return;
        }
        if (Water <= 0)
        {
            character.ApplyDamage(2, Vector3.up, character.ID, string.Empty);
        }
        if (Hungry <= 0)
        {
            character.ApplyDamage(1, Vector3.up, character.ID, string.Empty);
        }
    }

    public void hungryUpdate()
    {
        Eating(-1);
    }

    public void thirstilyUpdate()
    {
        Drinking(-1);
    }

    public void Eating(int num)
    {
        Hungry += num;
    }

    public void Drinking(int num)
    {
        Water += num;
    }

    public void Healing(int num)
    {
        if (character == null)
        {
            return;
        }
        character.HP += num;
    }
}
