using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum EnemyState
{
    Patrolling,
    Following
}

public class SkunkApeScript : MonoBehaviour
{
    public GameObject rainW;
    [SerializeField] private GameObject interactUI;

    [Header("References")]
    [SerializeField] private PoleScript pole1;
    [SerializeField] private PoleScript pole2;
    [SerializeField] private PoleScript pole3;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;

    [Header("References")]
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float stopAtDist = 0.5f;
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private float losePlayerTime = 3f;

    private double cantSeePlayerTime = 300;


    private NavMeshAgent agent;
    private int currentPatrolIndex;
    private bool isWaiting;
    private bool patroling = true;
    private EnemyState state = EnemyState.Patrolling;
    private float timeSinceLostPlayer;
    private bool hasCaughtPlayer = false;
    public float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        
        GoToNextPoint();
    }

    // Update is called once per frame
    void Update()
    {



        if (hasCaughtPlayer) return;

        // Force follow while player is on the pole
        if ((pole1 != null && pole1.isSquating) || (pole2 != null && pole2.isSquating) || (pole3 != null && pole3.isSquating))
        {
            patroling = false;
            isWaiting = false;
            StopAllCoroutines();
            FollowPlayer();

        }
        else
        {
            // If not forced, normal detection drives following
            patroling = true;
            if (patroling)
            {
                PatrolTick();

                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer <= detectRange && CanSeePlayer())
                {
                    FollowPlayer();
                }
            }

            
        }

        



    }

   

    private void FollowPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        if ((pole1 != null && pole1.isSquating) || (pole2 != null && pole2.isSquating) || (pole3 != null && pole3.isSquating))
        {
            Debug.Log("Skunk Ape forced-follow (player on pole)");
            timeSinceLostPlayer = 0f;
            return;
        }

        if (!CanSeePlayer())
        {
            timeSinceLostPlayer += Time.deltaTime;
            if (timeSinceLostPlayer >= losePlayerTime)
                patroling = true;
        }
        else
        {
            timeSinceLostPlayer = 0f;
        }

        Debug.Log("Skunk Ape following rain W");
    }

    private void PatrolTick()
    {
        if (isWaiting) return;

        if (!agent.pathPending && agent.remainingDistance <= stopAtDist)
        {
            StartCoroutine(WaitThenNext());
        }
    }

    private IEnumerator WaitThenNext()
    {
        isWaiting = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(patrolWaitTime);

        agent.isStopped = false;
        isWaiting = false;
        GoToNextPoint();
    }

    private void GoToNextPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void GoToClosestPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float d = Vector3.Distance(transform.position, patrolPoints[i].position);
            if (d < closestDistance)
            {
                closestDistance = d;
                closestIndex = i;
            }
        }

        currentPatrolIndex = closestIndex;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    private bool CanSeePlayer()
    {
        return IsInFOV(player.position);
    }
    

    private bool IsInFOV(Vector3 targetPos)
    {
        // 2.5D/isometric: ignore height
        Vector3 toTarget = targetPos - transform.position;
        toTarget.y = 0f;

        // Range check (fast)
        float sqrDist = toTarget.sqrMagnitude;
        if (sqrDist > detectRange * detectRange) return false;
        if (sqrDist < 0.0001f) return true;

        // Angle check (cone)
        Vector3 forward = transform.forward;
        forward.y = 0f;

        float angle = Vector3.Angle(forward.normalized, toTarget.normalized);
        return angle <= viewAngle * 0.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RainWizard"))
        {
            agent.isStopped = true;
            hasCaughtPlayer = true;
            if (rainW != null) rainW.SetActive(false);
            interactUI.SetActive(true);

        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // Draw FOV boundaries (ignore height, matches your IsInFOV)
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 leftDir = Quaternion.Euler(0f, -viewAngle * 0.5f, 0f) * forward;
        Vector3 rightDir = Quaternion.Euler(0f, viewAngle * 0.5f, 0f) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * detectRange);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * detectRange);

        // Optional: draw a line to the player (green if visible, gray if not)
        if (player != null)
        {
            Gizmos.color = CanSeePlayer() ? Color.green : Color.gray;
            Vector3 p = player.position;
            p.y = transform.position.y; // keep it flat like your math
            Gizmos.DrawLine(transform.position, p);
        }
    }
}
