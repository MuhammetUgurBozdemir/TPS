using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform MovePosition;
    [SerializeField] private Character Enemy;

    private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    public GameObject target;
    float distance;
    bool isInRange = false;
    public bool isChase = true;
   
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //navMeshAgent.destination = MovePosition.transform.position;
        StartCoroutine(SideMovement());
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void Update()
    {
        Die();
    }

    void Movement()
    {
        if (Enemy.isAlive == true)
        {
            if (navMeshAgent.velocity.magnitude != 0)
            {
                animator.SetBool("isRunning", true);
            }
            else animator.SetBool("isRunning", false);
            distance = Vector3.Distance(transform.position, target.transform.position);
            navMeshAgent.destination = MovePosition.transform.position;

            if (distance > 4)
            {
                isInRange = false;
                navMeshAgent.speed = 3.5f;
                animator.SetBool("isThrowing", false);
            }
            else if (distance < 4)
            {
                isInRange = true;
                navMeshAgent.speed = 0f;
                animator.SetBool("isThrowing", true);
                transform.LookAt(target.transform);
            }
        }
       
    }
    private IEnumerator SideMovement()
    {
        while (true)
        {
            if (isInRange && Enemy.isAlive==true)
            {
                transform.DOLocalMoveX(transform.localPosition.x + 0.5f, 0.5f).OnComplete(() =>
                {
                    transform.DOLocalMoveX(transform.localPosition.x - 0.5f, 0.5f);
                });
                if (transform.position.z - target.transform.position.z <= 2 && transform.position.z - target.transform.position.z >= -2)
                {
                    transform.DOLocalMoveZ(transform.localPosition.z - 0.5f, 0.5f).OnComplete(() =>
                    {
                        transform.DOLocalMoveZ(transform.localPosition.z + 0.5f, 0.5f);
                    });
                }

            }

            yield return new WaitForSeconds(1);
        }
    }
    void Die()
    {
        if (Enemy.health <= 0)
        {
            animator.SetBool("isDeath", true);
            animator.SetBool("isThrowing", false);
            animator.SetBool("isRunning", false);
            isInRange = false;
            Enemy.isAlive = false;
        }
        else animator.SetBool("isDeath", false);
    }
    private IEnumerator TakeHit()
    {
        animator.SetBool("isTakeHit", true);
        yield return new WaitForSeconds(1);
        Enemy.TakeDamage();
        animator.SetBool("isTakeHit", false);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "projectile")
        {
            StartCoroutine(TakeHit());
            Destroy(other.gameObject);
        }
    }

}
