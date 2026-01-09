using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private PlayerMovement playerScript;

    [Header("Detection Settings")]
    [SerializeField] private float viewLength;
    [SerializeField, Range(0, 360)] private float detectionAngle = 90f;
    private float increasedDetectionAngle = 120f;
    private float normalDetectionAngle = 90f;

    [Header("Line of Sight")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform eyePosition;

    private Transform cachedTarget;
    public Transform lastPOS;
    [SerializeField] private GameObject lastKnownPOSPrefab;

    public float ViewLength => viewLength;
    public float DetectionAngle => detectionAngle;

    private Transform EyesTransform => eyePosition != null ? eyePosition : transform;

    public bool hasKey;
    [SerializeField] public GameObject keyIcon;
    [SerializeField] public GameObject alertIcon;

    public bool withinHearingRange;
    private float hearingRange = 10f;
    public Transform whistlePOS;

    public bool canSeePlayer;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObj.GetComponent<PlayerMovement>();
        cachedTarget = playerObj != null ? playerObj.transform : null;
        hasKey = true;
        alertIcon.SetActive(false);

    }

    private void Update()
    {
        canSeePlayer = CanSeeTarget();

        Debug.DrawRay(EyesTransform.position, EyesTransform.forward * viewLength, Color.red);
        Debug.Log(canSeePlayer.ToString());

        if (canSeePlayer)
        {
            alertIcon.SetActive(true);
        }
        else
        {
            alertIcon.SetActive(false);
        }
    }

    public bool CanSeeTarget()
    {
        if (cachedTarget == null)
            return false;
        Vector3 directionToTarget = (cachedTarget.position - EyesTransform.position).normalized;
        float distanceToTarget = Vector3.Distance(EyesTransform.position, cachedTarget.position);
        if (distanceToTarget <= viewLength)
        {
            float angleToTarget = Vector3.Angle(EyesTransform.forward, directionToTarget);
            if (angleToTarget <= detectionAngle / 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(EyesTransform.position, directionToTarget, out hit, viewLength, playerLayer))
                {
                    if (hit.transform == cachedTarget)
                    {
                        //Instantiate(lastKnownPOSPrefab, cachedTarget.position + Vector3.up * 0.5f, Quaternion.identity);
                        lastPOS = cachedTarget.transform;
                        detectionAngle = increasedDetectionAngle;
                        canSeePlayer = true;
                        return true;
                    }
                }
            }
        }
        detectionAngle = normalDetectionAngle;
        canSeePlayer = false;
        return false;
    }

    public bool IsWithinHearingRange()
    {
        if (cachedTarget == null)
        {
            return false;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, cachedTarget.transform.position);
        withinHearingRange = distanceToPlayer <= hearingRange;
        return withinHearingRange;
    }
}
