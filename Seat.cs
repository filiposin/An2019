using UnityEngine;

public class Seat : MonoBehaviour
{
    public Vehicle VehicleRoot;

    public string SeatID = "S1";

    public string CharacterID = string.Empty;

    public bool IsDriver;

    public bool Active;

    public CharacterDriver passenger;

    private void Start()
    {
        if (VehicleRoot == null && transform.root)
        {
            VehicleRoot = transform.root.GetComponent<Vehicle>();
        }
    }

    public void GetIn(CharacterDriver driver)
    {
        if (driver)
        {
            Active = true;
            driver.transform.position = transform.position;
            driver.transform.parent = transform;
            driver.character.controller.enabled = false;
            driver.DrivingSeat = this;
            driver.LastSeat = SeatID;
            passenger = driver;
            if (VehicleRoot != null)
            {
                VehicleRoot.UpdateSeat(driver, this, true);
            }
        }
    }

    public void GetOut(CharacterDriver driver)
    {
        if (driver)
        {
            Active = false;
            driver.transform.parent = null;
            driver.character.controller.enabled = true;
            driver.DrivingSeat = null;
            passenger = null;
            if (VehicleRoot != null)
            {
                VehicleRoot.UpdateSeat(driver, this, false);
            }
        }
    }

    public void OnSeat(CharacterDriver driver, bool sit)
    {
        if (driver)
        {
            if (sit)
            {
                Active = true;
                passenger = driver;
                driver.character.controller.enabled = false;
                driver.DrivingSeat = this;
            }
            else
            {
                driver.transform.parent = null;
                driver.character.controller.enabled = true;
                driver.DrivingSeat = null;
                passenger = null;
            }
        }
    }

    public void CleanSeat()
    {
        CharacterDriver[] componentsInChildren = transform.GetComponentsInChildren<CharacterDriver>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            if (componentsInChildren[i])
            {
                componentsInChildren[i].transform.parent = null;
            }
        }
        Active = false;
        passenger = null;
        Debug.Log("Clean Seat! " + SeatID);
    }

    public void CheckSeat()
    {
        if (transform.childCount <= 0)
        {
            Active = false;
            passenger = null;
        }
    }

    private void Update()
    {
        if (passenger)
        {
            CharacterID = passenger.character.ID;
            passenger.OnVehicle(VehicleRoot.VehicleID, SeatID, true);
        }
        else
        {
            Active = false;
        }
        CheckSeat();
    }

    private void FixedUpdate()
    {
        if (passenger)
        {
            passenger.transform.position = transform.position;
            passenger.transform.parent = transform;
        }
    }

    private void OnDrawGizmos()
    {
    }
}