using UnityEngine;
using UnityEngine.UI;

public class CigFallController : MonoBehaviour
{
    public Button triggerButton; 
    private string parameterName = "CigFallBool"; 
    private Animator animator; 

    void Start()
    {
        
        animator = FindAnimatorWithParameter(parameterName);

        
        if (animator == null)
        {
            Debug.LogError("No Animator found with parameter: " + parameterName);
            return;
        }

        
        if (triggerButton != null)
        {
            triggerButton.onClick.AddListener(ActivateCigFall);
        }
        else
        {
            Debug.LogError("No button assigned to trigger the animation.");
        }
    }

    void ActivateCigFall()
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, true);
            Debug.Log("CigFallBool set to true!");
        }
    }

    
    Animator FindAnimatorWithParameter(string paramName)
    {
        Animator[] animators = FindObjectsOfType<Animator>(); 
        foreach (Animator anim in animators)
        {
            foreach (var param in anim.parameters)
            {
                if (param.name == paramName)
                {
                    return anim;
                }
            }
        }
        return null; 
    }
}
