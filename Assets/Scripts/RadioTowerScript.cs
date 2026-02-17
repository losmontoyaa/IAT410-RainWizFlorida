using UnityEngine;




public class RadioTowerScript : MonoBehaviour
{
    public PoleScript pole1;
    public PoleScript pole2;
    public PoleScript pole3;
    public bool topOfTower = false;
    public int megaCharge;

    [SerializeField] private GameObject interactUI;

    public Transform rainW;
    public bool canClimbTower = false;
    public bool isOnTower = false;

    public GameObject topTower;

    public Transform topofTower;

    public PlayerController rainWiz;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
         if (rainWiz != null)
            rainWiz = rainWiz.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pole1.charged && pole2.charged && pole3.charged)
        {
            if (canClimbTower && Input.GetKeyDown(KeyCode.E))
            {
                //var cc = rainW.GetComponent<CharacterController>();
                //if (cc != null) cc.enabled = false;
                var rb = rainW.GetComponent<Rigidbody>();
                Vector3 safePos = topofTower.transform.position + Vector3.up * 0.2f;

                //rainW.position = topofPole.position;

                Physics.SyncTransforms(); // helps triggers update after teleport

                //if (cc != null) cc.enabled = true;

                //var mover = rainW.GetComponent<PlayerController>();
                if (rainW != null)
                {
                    rainWiz.canMove = false;
                    //mover.ResetVerticalVelocity();
                }

                if (rb != null)
                {
                    rb.position = safePos;
                    rb.linearVelocity = Vector3.zero;          // or rb.linearVelocity in Unity 6
                    rb.angularVelocity = Vector3.zero;
                    Physics.SyncTransforms();
                    rb.freezeRotation = true;
                }

                rainW.rotation = Quaternion.identity;


                isOnTower = true;
                //Debug.Log("Rain Wizard is on the pole");




            }

            if (isOnTower)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    megaCharge++;
                    Debug.Log(megaCharge);
                    if (megaCharge == 100)
                    {
                        interactUI.SetActive(true);
                    }
                }
            }
            



        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RainWizard"))
            canClimbTower = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RainWizard"))
            canClimbTower = true;

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RainWizard"))
            canClimbTower = false;
    }
}

