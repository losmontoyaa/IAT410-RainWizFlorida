using UnityEngine;

public class PoleScript : MonoBehaviour
{

    //public GameObject rainW;
    public Transform rainW;
    
    private bool canClimb = false;
    public bool onPole = false;


    public Transform topOfPole;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canClimb && Input.GetKeyDown(KeyCode.E))
        {
            var cc = rainW.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            rainW.position = topOfPole.position;
            if (cc != null) cc.enabled = true;

            var mover = rainW.GetComponent<RainWizzardScript>();
            if (mover != null) mover.ResetVerticalVelocity();

            onPole = true;
            Debug.Log("Rain Wizard is on the pole");
        }
        else if (rainW.position != topOfPole.position)
        {
            onPole = false;
        }

        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RainWizard"))
        {
            //rainW = other.gameObject;
            canClimb = true;
            Debug.Log("Rain Wizard is next to the pole");
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RainWizard"))
        {
            canClimb = false;
            //rainW = null;
            Debug.Log("Rain Wizard left the pole");
        }
        
    }

}
