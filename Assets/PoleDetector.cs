using UnityEngine;

public class PoleDetector : MonoBehaviour
{
    [Header("Listening Settings")]
    [SerializeField] float listeningAngle = 60f;
    [SerializeField] LayerMask detectableLayers;

    [Header("References")]
    [SerializeField] PlayerController player;

    public bool IsSomethingDetected { get; private set; }

    void Update()
    {
        if (!player.squatPopped)
        {
            IsSomethingDetected = false;
            return;
        }

        DetectInCone();
    }

    void DetectInCone()
    {
        IsSomethingDetected = false;

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            Mathf.Infinity,
            detectableLayers
        );

        foreach (var hit in hits)
        {
            Vector3 directionToTarget =
                (hit.transform.position - transform.position).normalized;

            float angle =
                Vector3.Angle(transform.forward, directionToTarget);

            if (angle <= listeningAngle * 0.5f)
            {
                IsSomethingDetected = true;
                print("AY YO THERES SOMETHIN HERE");
                return;
            }
        }
    }

    //For testing
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5f);

        Vector3 left = Quaternion.Euler(0, -listeningAngle / 2f, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, listeningAngle / 2f, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + left * 5f);
        Gizmos.DrawLine(transform.position, transform.position + right * 5f);
    }
}

