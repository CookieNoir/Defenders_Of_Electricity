using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public int healthPoints;
    public int value;
    public int damage;
    private float attackCooldown;
    public float attackRate;

    public void SetDestination(Transform destination)
    {
        agent.destination = destination.position;
    }

    public void Hit(int damage)
    {
        healthPoints-=damage;
        if (healthPoints <= 0)
        {
            GameController.instance.UpdateMoney(value);
            Destroy(gameObject);
        }
    }

    public void Attack()
    {
        GameController.instance.UpdateHealthPoints(-damage);
    }

    private void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1f / attackRate;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        GameController.instance.enemiesList.Remove(this);
    }
}
