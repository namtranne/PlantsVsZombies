using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public GameObject panel;
    public Button settingsButton;
    public Button okButton;

    // Start is called before the first frame update
    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }

        settingsButton.onClick.AddListener(() =>
        {
            panel.SetActive(true);
        });

        okButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
