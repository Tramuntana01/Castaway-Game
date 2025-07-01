using UnityEngine;

public class SharkMovement : MonoBehaviour
{
    public float speed = 2f;
    public float turnSpeed = 1f;
    public float changeDirectionTime = 5f;

    public Transform patrolCenter;         // ← Este es un Transform, no le asignes un float
    public float patrolRadius = 15f;

    private Vector3 direction;
    private float timer;

    void Start()
    {
        if (patrolCenter == null)
        {
            Debug.LogError("🚫 patrolCenter no asignado en " + gameObject.name);
            enabled = false;
            return;
        }

        PickNewDirection();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        timer += Time.deltaTime;
        if (timer > changeDirectionTime || OutsidePatrolZone())
        {
            PickNewDirection();
            timer = 0f;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    void PickNewDirection()
    {
        if (OutsidePatrolZone())
        {
            direction = (patrolCenter.position - transform.position).normalized;
        }
        else
        {
            direction = new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f)
            ).normalized;
        }
    }

    bool OutsidePatrolZone()
    {
        return Vector3.Distance(transform.position, patrolCenter.position) > patrolRadius;
    }
}
