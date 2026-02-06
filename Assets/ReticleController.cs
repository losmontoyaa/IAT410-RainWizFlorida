using UnityEngine;

public class ReticleController : MonoBehaviour
{

    [SerializeField] PlayerController p;
    [SerializeField] GameObject arcSprite;

    // Update is called once per frame
    void Update()
    {
        arcSprite.SetActive(p.squatPopped);

        if (!p.squatPopped) return;

        transform.rotation = Quaternion.Euler(0, p.transform.eulerAngles.y, 0);
        
    }
}
