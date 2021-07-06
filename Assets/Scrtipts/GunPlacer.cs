using System.Collections.Generic;
using UnityEngine;

public class GunPlacer : MonoBehaviour
{
    [SerializeField] private List<Transform> hidingSpots;
    [SerializeField] private List<GameObject> gunModels;

    private void Start()
    {
        // Iterate every gun model
        foreach (GameObject gun in gunModels)
        {
            // If don't have any more hiding spots, just exit the loop
            if (hidingSpots.Count == 0)
            {
                Debug.Log("Not enogth hiding spots!");
                break;
            }
            // Select a random hiding spot
            Transform hidingSpot = hidingSpots[Random.Range(0, hidingSpots.Count)];
            // Instantiate the gun model at that position
            Instantiate(gun, hidingSpot.position, Quaternion.identity, hidingSpot);
            // Remove the hiding spot from the list so no other gun be in it
            hidingSpots.Remove(hidingSpot);
        }
    }
}