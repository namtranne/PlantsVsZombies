using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlantSelect : MonoBehaviour
{
    public static List<GameObject> Plants = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic if needed
    }

    private void FixedUpdate()
    {

    }

    public static void AddPlant(GameObject plant, Transform position)
    {
        if (!Plants.Contains(plant))
        {
            Plants.Add(plant);
            Debug.Log("Plant added");
            InstantiatePlant(plant, position);
            //LogPlants();
        }
        else
        {
            Debug.Log("Plant already in the list");
        }
    }

    private static void InstantiatePlant(GameObject plant, Transform position)
    {
        GameObject instantiatedPlant = Instantiate(plant, position);
    }

    public static void RemovePlant(GameObject plant)
    {
        if (Plants.Contains(plant))
        {
            Plants.Remove(plant);
            Debug.Log("Plant removed");
            Destroy(plant);
            //LogPlants();
        }
        else
        {
            Debug.Log("Plant not found in the list");
        }
    }

}
