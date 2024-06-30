using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PickPlant : MonoBehaviour, IPointerDownHandler
{
    public GameObject plantObject;
    public int id;
    private bool isPick;
    private PanelController panelController;
    // Start is called before the first frame update
    void Start()
    {
        isPick = false;
        panelController = GameObject.Find("PanelController").GetComponent<PanelController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPick)
        {
            panelController.AddGameObject(plantObject, id);
            isPick = true;
        }
        else
        {
            panelController.RemoveGameObject(id);
            isPick = false;
        }
        

    }
}
