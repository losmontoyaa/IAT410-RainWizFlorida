using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class CollectibleScript : MonoBehaviour
{
    public GameObject bottleCorona;
    public bool canGrabCorona = false;

    [SerializeField] TextMeshProUGUI collectText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canGrabCorona && Input.GetKeyDown(KeyCode.E))
        {
            
            bottleCorona.SetActive(false);
            collectText.gameObject.SetActive(true);



        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RainWizard"))
        {
            canGrabCorona = true;
            Debug.Log("can grab corona");
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RainWizard"))
        {
            canGrabCorona = false;
        }


    }
}
