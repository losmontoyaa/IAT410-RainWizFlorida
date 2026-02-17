using UnityEngine;
using TMPro;

public class PoleScript : MonoBehaviour
{

    //public GameObject rainW;
    public Transform rainW;
    public int gossipCharge;
    
    private bool canClimb = false;
    public bool charged = false;
    //public bool onPole = false;
    public bool isSquating;
    public GameObject topPole;

    [SerializeField] TextMeshProUGUI chargedText;
    [SerializeField] TextMeshProUGUI chargingText;
    [SerializeField] TextMeshProUGUI eText;

    public Transform topofPole;

    private TopOfPoleScript topOfPole;
    private RadioTowerScript radioTower;
    public PlayerController rainWiz;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (topPole != null)
            topOfPole = topPole.GetComponent<TopOfPoleScript>();

        if (rainWiz != null)
            rainWiz = rainWiz.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        isSquating = topOfPole != null && topOfPole.IsInside;
              

        if (canClimb && Input.GetKeyDown(KeyCode.E))
        {
            //var cc = rainW.GetComponent<CharacterController>();
            //if (cc != null) cc.enabled = false;
            var rb = rainW.GetComponent<Rigidbody>();
            Vector3 safePos = topOfPole.transform.position + Vector3.up * 0.2f;

            //rainW.position = topofPole.position;

            Physics.SyncTransforms(); // helps triggers update after teleport

            //if (cc != null) cc.enabled = true;

            //var mover = rainW.GetComponent<PlayerController>();
            if (rainWiz != null)
            {
                rainWiz.canMove = false;
                //mover.ResetVerticalVelocity();
            }
            
            if (rb != null)
            {
                rb.position = safePos;
                rb.linearVelocity = Vector3.zero;          
                rb.angularVelocity = Vector3.zero;
                Physics.SyncTransforms();
                rb.freezeRotation = true;
            }

            rainW.rotation = Quaternion.identity;



           // Debug.Log("Rain Wizard is on the pole");

            


        }
       
        if (isSquating)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                gossipCharge++;
                Debug.Log(gossipCharge);
                chargingText.gameObject.SetActive(gossipCharge < 1000);

                if (gossipCharge == 1000)
                {
                    chargedText.gameObject.SetActive(gossipCharge == 1000);
                    charged = true;
                    rainWiz.canMove = true;

                }

            }
        }
        print("can climb =" + canClimb);
        eText.gameObject.SetActive(canClimb);

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
            //Debug.Log("Rain Wizard left the pole");
        }
        
    }

}
