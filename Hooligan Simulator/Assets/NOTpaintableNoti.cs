using UnityEngine;
using UnityEngine.UI;

public class ClickToShowPanel : MonoBehaviour
{
    public GameObject panelToShow;
    public float panelVisibleTime = 3f; 
    private bool isPanelActive = false;
    private float timer = 0f;

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            
            if (Physics.Raycast(ray, out hit))
            {
                
                if (!hit.collider.CompareTag("Paintable"))
                {
                    
                    ShowPanel();
                }
            }
        }

        
        if (isPanelActive)
        {
            timer += Time.deltaTime;
            if (timer >= panelVisibleTime)
            {
                HidePanel();
            }
        }
    }

    void ShowPanel()
    {
        panelToShow.SetActive(true);
        isPanelActive = true;
        timer = 0f;
    }

    void HidePanel()
    {
        panelToShow.SetActive(false);
        isPanelActive = false;
        timer = 0f;
    }
}
