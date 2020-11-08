using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemy _target;

    public float bulletSpeed = 70f;
    public int damage = 1;

    public void SetTarget(Enemy target)
    {
        _target = target;
    }

    void Update()
    {
        if (!_target)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = _target.transform.position - transform.position;
        float distanceFromFrame = bulletSpeed * Time.deltaTime;

        if (dir.magnitude <= distanceFromFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceFromFrame, Space.World);
    }

    private void HitTarget()
    {
        _target.Hit(damage);
        Destroy(gameObject);
    }
}
