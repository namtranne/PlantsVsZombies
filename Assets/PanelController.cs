using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    private int currentCount;
    private List<GameObject> listPlants;
    private List<int> ids;
    public Transform parentObject;
    private int maxSlot;

    void Start()
    {
        currentCount = 0;
        listPlants = new List<GameObject>();
        ids = new List<int>();
        maxSlot = 7;
    }

    public bool AddGameObject(GameObject go, int id)
    {
        if (currentCount >= maxSlot)
            return false;

        GameObject instantiatedImage = Instantiate(go);
        instantiatedImage.transform.SetParent(parentObject, false);
        listPlants.Add(instantiatedImage);
        ids.Add(id);
        currentCount++;
        return true;
    }

    public bool RemoveGameObject(int id)
    {
        int index = ids.IndexOf(id);
        if (index != -1)
        {
            Destroy(listPlants[index]);
            ids.RemoveAt(index);
            listPlants.RemoveAt(index);
            currentCount--;
            return true;
        }
        else
        {
            Debug.LogWarning("Attempted to remove an item that does not exist: " + id);
            return false;
        }
    }

    public int GetAvailableSlots()
    {
        return maxSlot - currentCount;
    }
}