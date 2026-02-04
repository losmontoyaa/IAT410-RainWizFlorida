using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrolling,
    Following
}

public class SkunkApeScript : MonoBehaviour
{
    public GameObject rainW;
   
    [Header("References")]
    [SerializeField] private PoleScript pole;
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
    private EnemyState state = EnemyState.Patrolling;
    private float timeSinceLostPlayer;
    private bool hasCaughtPlayer = false;


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
        if (pole != null && pole.onPole)
        {
            if (state != EnemyState.Following) EnterFollowing();
        }
        else
        {
            // If not forced, normal detection drives following
            if (state == EnemyState.Patrolling)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer <= detectRange && CanSeePlayer())
                {
                    EnterFollowing();
                }
            }
        }

        switch (state)
        {
            case EnemyState.Patrolling:
                PatrolTick();
                break;

            case EnemyState.Following:
                FollowPlayer();
                break;
        }



    }

    private void EnterFollowing()
    {
        state = EnemyState.Following;
        isWaiting = false;
        agent.isStopped = false;
        timeSinceLostPlayer = 0f;
    }

    private void EnterPatrolling()
    {
        state = EnemyState.Patrolling;
        isWaiting = false;
        agent.isStopped = false;
        timeSinceLostPlayer = 0f;

        GoToClosestPoint();
    }

    private void FollowPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        // While player is on pole, keep following no matter what
        if (pole != null && pole.onPole) return;

        // Otherwise, use lose logic
        if (!CanSeePlayer())
        {
            timeSinceLostPlayer += Time.deltaTime;
            if (timeSinceLostPlayer >= losePlayerTime)
            {
                EnterPatrolling();
            }
        }
        else
        {
            timeSinceLostPlayer = 0f;
        }
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
        }
    }
}
