using UnityEngine;

public class Tower : MonoBehaviour
{
    public Enemy target;
    [SerializeField]
    private Transform partToRotation;

    [SerializeField]
    private GameObject bulletPrefabs;
    [SerializeField]
    private Transform firePosition;

    public float attackRadius = 5f;
    public float rotationSpeed = 2f;

    public float fireRate = 1f;
    public float fireCountdown = 0f;

    public bool isActive;
    private float prevDistance = float.PositiveInfinity;
    public AudioSource shot;

    void Start()
    {
        shot.enabled = GameController.instance.audioTurnedOn;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        if (target && prevDistance <= attackRadius)
        {
            prevDistance = Vector3.Distance(transform.position, target.transform.position);
            return;
        }
        else
        {
            target = null;
            float nearestDistance = Mathf.Infinity;
            Enemy nearestObject = null;

            foreach (Enemy enemy in GameController.instance.enemiesList)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestObject = enemy;
                }
            }

            if (nearestObject && nearestDistance <= attackRadius)
            {
                target = nearestObject;
                fireCountdown = 1f / fireRate;
                prevDistance = nearestDistance;
            }
        }
    }

    void Update()
    {
        if (!target || !isActive)
            return;

        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotation.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        partToRotation.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        if (shot) shot.Play(0);
        GameObject bulletGo = Instantiate(bulletPrefabs, firePosition.position, firePosition.rotation) as GameObject;
        Bullet bullet = bulletGo.GetComponent<Bullet>();

        if (bullet)
            bullet.SetTarget(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
