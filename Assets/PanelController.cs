using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public Transform[] spawnPoints;
    private int temp;
    private List<GameObject> listPlants;
    private List<int> ids;
    public Transform parentObject;
    // Start is called before the first frame update
    void Start()
    {
        temp = 0;
        listPlants = new List<GameObject>();
        ids = new List<int>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int AddGameObject(GameObject go, int id)
    {
        if (temp >= spawnPoints.Length) return spawnPoints.Length;
        GameObject instantiatedImage = Instantiate(go);
        instantiatedImage.transform.SetParent(parentObject, false);
        listPlants.Add(instantiatedImage);
        ids.Add(id);
        temp++;
        return spawnPoints.Length;
    }

    public void RemoveGameObject(int id)
    {
        int p = ids.FindIndex(i => i == id);
        Destroy(listPlants[p]);
        ids.RemoveAt(p);
        listPlants.RemoveAt(p);
        temp--;
    }
}
