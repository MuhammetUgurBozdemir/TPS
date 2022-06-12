using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Character Player;
    private Rigidbody rb;
    public FloatingJoystick joystick;
    public float speed;
    [SerializeField] private Animator animator;
    Vector3 dir;
    [SerializeField] private GameObject Enemy;
    bool isThrowing = false;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        Die();
        if (isThrowing == false)
        {
            dir = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;

            Vector3 rot = Vector3.forward * joystick.Vertical + (Vector3.right * joystick.Horizontal);
            if (rot != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rot), 20 * Time.deltaTime);
            }

            if (joystick.Vertical != 0 || joystick.Horizontal != 0)
            {
                animator.SetBool("isRunning", true);

            }
            else animator.SetBool("isRunning", false);
        }
        else if (isThrowing == true)
        {
            Quaternion rot = Quaternion.LookRotation(Enemy.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 20 * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(dir * speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void Throw()
    {
        isThrowing = true;
        animator.SetBool("isThrowing", true);
    }
    public void ThrowStop()
    {
        isThrowing = false;
        animator.SetBool("isThrowing", false);
    }

    void Die()
    {
        if (Player.health <= 0)
        {
            animator.SetBool("isDeath", true);
         
        }
        else animator.SetBool("isDeath", false);
    }
    private IEnumerator TakeHit()
    {
        animator.SetBool("isTakeHit", true);
        yield return new WaitForSeconds(1);
        Player.TakeDamage();
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
