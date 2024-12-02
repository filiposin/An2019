using UnityEngine;

public class Car : Vehicle
{
    public AudioClip ClashSound;
    public int ClashDamage = 10000;
    public CarControl car;

    private Vector2 inputTemp;
    private bool brakeTemp;

    private void Start()
    {
        Audiosource = GetComponent<AudioSource>();
        car = base.gameObject.GetComponent<CarControl>();
    }

    public override void Drive(Vector2 input, bool brake)
    {
        // Multiplayer logic removed; now just calls the driving method directly.
        driving(new Vector3(input.x, input.y, 0f), brake);
        base.Drive(input, brake);
    }

    // Method for driving the car based on input and brake.
    public void driving(Vector3 input, bool brake)
    {
        inputTemp = input;
        brakeTemp = brake;
        incontrol = true;
        if (input.x == 0f && input.y == 0f && !brake)
        {
            incontrol = false;
        }
    }

    private void Update()
    {
        if (incontrol && (bool)car)
        {
            car.Controller(new Vector2(inputTemp.x, inputTemp.y), brakeTemp);
        }
        UpdateFunction();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((bool)Audiosource && (bool)ClashSound && GetComponent<Rigidbody>().velocity.magnitude > 3f)
        {
            Audiosource.PlayOneShot(ClashSound);
        }
        DamagePackage damagePackage = default(DamagePackage);
        damagePackage.Damage = (int)((float)ClashDamage * GetComponent<Rigidbody>().velocity.magnitude);
        damagePackage.Normal = GetComponent<Rigidbody>().velocity.normalized;
        damagePackage.Direction = GetComponent<Rigidbody>().velocity * 2f;
        damagePackage.Position = base.transform.position;
        if (Seats.Length > 0 && (bool)Seats[0].passenger)
        {
            damagePackage.ID = Seats[0].passenger.character.ID;
            damagePackage.Team = "car";
        }
        collision.collider.SendMessage("DirectDamage", damagePackage, SendMessageOptions.DontRequireReceiver);
    }
}
