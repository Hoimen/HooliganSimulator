using UnityEngine;
using UnityEngine.UI;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel; 

    void Start()
    {
        panel.SetActive(false); 
    }

    void OnMouseDown()
    {
        panel.SetActive(!panel.activeSelf); 
    }
}
