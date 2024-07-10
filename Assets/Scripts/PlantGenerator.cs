using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantGenerator : MonoBehaviour
{
    public GameObject prefab; // Assign the prefab in the Inspector
    public GameObject layoutGroup; // Assign the GameObject with HorizontalLayoutGroup in the Inspector

    public Transform spawnPosition; 

    private int maxPlant = 9;

    public int localPositionX = 30;
    public int localPositionY = 400;

    public BowlingType[] bowlingTypes;


    void Start()
    {
        // Start the coroutine to add objects at random intervals
        StartCoroutine(AddObjectsAtRandomIntervals());
    }

    IEnumerator AddObjectsAtRandomIntervals()
    {
        AddNewObject();
        while (true)
        {
            float waitTime = Random.Range(3f, 6f);
            yield return new WaitForSeconds(waitTime);
            // Randomly choose an interval between 1 and 3 seconds
            if (layoutGroup.transform.childCount < maxPlant)
            {
                // Instantiate and add a new object to the layout group
                AddNewObject();
            }
            else
            {
                Debug.Log("Maximum number of plants reached : " + layoutGroup.transform.childCount);
            }
        }
    }

    void AddNewObject()
    {
        if (prefab == null || layoutGroup == null)
        {
            Debug.LogError("Prefab or Layout Group not assigned.");
            return;
        }
         // Select a random index
        int randomIndex = Random.Range(0, bowlingTypes.Length);
        BowlingType randomBowlingType = bowlingTypes[randomIndex];

        // Instantiate the prefab
        GameObject newObject = Instantiate(prefab, spawnPosition.position, spawnPosition.rotation);
        newObject.GetComponent<BowlingGamePlantSlot>().UpdatePlant(randomBowlingType);

        // Set the parent to the layout group
        newObject.transform.SetParent(layoutGroup.transform, false);

        // Reset the local scale to (1, 1, 1)
        newObject.transform.localScale = Vector3.one;

        // Optionally adjust the local position after setting the parent
        // For example, place it at (x, y, z) in local space of the layout group
        newObject.transform.localPosition = new Vector3(localPositionX, localPositionY, -9719/108 );
    }
}
