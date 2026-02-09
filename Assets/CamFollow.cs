using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 10f;

    public class CameraFollow : MonoBehaviour
    {
        public Transform target;

        void LateUpdate()
        {
            if (target == null) return;
            transform.position = target.position;
        }
    }
}
