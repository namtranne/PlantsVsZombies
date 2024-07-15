using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }

        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Pausing();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        if (panel != null)
        {
            panel.SetActive(true);
            panel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            GameManager.Pausing();


            Animator[] allAnimators = FindObjectsOfType<Animator>();

            // Loop through all found Animators and disable them
            foreach (Animator animator in allAnimators)
            {
                animator.enabled = false;
            }
        }
    }
}
