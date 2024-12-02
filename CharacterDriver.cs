using UnityEngine;

[RequireComponent(typeof(CharacterSystem))]
public class CharacterDriver : MonoBehaviour
{
    [HideInInspector]
    public Seat DrivingSeat;

    [HideInInspector]
    public string LastSeat;

    [HideInInspector]
    public CharacterSystem character;

    private void Start()
    {
        character = GetComponent<CharacterSystem>();
    }

    public void Drive(Vector2 input, bool brake)
    {
        if (DrivingSeat != null && DrivingSeat.IsDriver && DrivingSeat.VehicleRoot != null)
        {
            DrivingSeat.VehicleRoot.Drive(new Vector2(input.x, input.y), brake);
        }
    }

    public void OutVehicle()
    {
        if (character != null && DrivingSeat != null)
        {
            DrivingSeat.GetOut(this);
        }
    }

    public void OnVehicle(string vehicleID, string seatID, bool isSit)
    {
        FindAndSitInVehicle(vehicleID, seatID, isSit);
    }

    private void FindAndSitInVehicle(string vehicleID, string seatID, bool isSit)
    {
        if (character == null)
            return;

        Vehicle[] vehicles = FindObjectsOfType<Vehicle>();
        foreach (Vehicle vehicle in vehicles)
        {
            if (vehicle.VehicleID != vehicleID)
                continue;

            foreach (Seat seat in vehicle.Seats)
            {
                if (seat.SeatID == seatID)
                {
                    seat.OnSeat(this, isSit);
                    break;
                }
            }
            break;
        }
    }
}
