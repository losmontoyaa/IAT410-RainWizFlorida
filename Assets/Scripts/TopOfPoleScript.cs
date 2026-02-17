using UnityEngine;

public class TopOfPoleScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool IsInside { get; private set; }
    

    private void Reset()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RainWizard"))
            IsInside = true;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RainWizard"))
            IsInside = true;

        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RainWizard"))
            IsInside = false;
    }
}
