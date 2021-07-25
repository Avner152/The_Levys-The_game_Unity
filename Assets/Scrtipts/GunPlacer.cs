using System.Collections.Generic;
using UnityEngine;

// A class that places models instances in random position
public class GunPlacer : MonoBehaviour
{
    public List<Transform> hidingSpots;
    public List<GameObject> gunModels;
    private List<Transform> gunPositions = new List<Transform>();

    public List<Transform> GetGunPositions()
    {
        return gunPositions;
    }

    private void Start()
    {
        // Iterate every gun model
        foreach (GameObject gun in gunModels)
        {
            // If don't have any more hiding spots, just exit the loop
            if (hidingSpots.Count == 0)
            {
                print("Not enogth hiding spots!");
                break;
            }
            // Select a random hiding spot
            Transform hidingSpot = hidingSpots[Random.Range(0, hidingSpots.Count)];
            // Instantiate the gun model at that position
            GameObject gunInstance = Instantiate(gun, hidingSpot.position, Quaternion.identity, hidingSpot);
            // Remove the hiding spot from the list so no other gun be in it
            hidingSpots.Remove(hidingSpot);
            // If the model is indeed a gun add it to the gun positions list
            // This list is used by the NPC to select which gun to pick, and because
            // NPC can't use a grenade we only add guns to it
            if (gunInstance.CompareTag("Gun"))
            {
                gunPositions.Add(gunInstance.transform);
            }
        }
    }
}