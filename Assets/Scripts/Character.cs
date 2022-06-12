using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public int health;
    public int armor;
    public int score;
    int damage;
    [SerializeField] private Canvas Finish;
    [SerializeField] private GameObject objectToThrow;
    [SerializeField] private Transform Target;
    [SerializeField] private Text Score;
    [SerializeField] private Text Armor;
    [SerializeField] private Text Health;
    [SerializeField] private List<Transform> SpawnPoints;
    public Transform attackPoint;
    public float throwForce;
    public float throwUpwardForce;
    public  bool isAlive = true;

    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
       
        Score.text = score.ToString();
        if (health > 0)
        {
            Health.text = "Health: " + health.ToString();
        }
        else Health.text = "Health: " + 0;

        if (armor > 0)
        {
            Armor.text = "Armor: " + armor.ToString();
        }
        else Armor.text = "Armor: " + 0;
    }

    public void Spawn()
    {
        int  rnd = Random.Range(0, SpawnPoints.Count);
        transform.position = SpawnPoints[rnd].position;
        Debug.Log(rnd);
        score++;
        health = 100;
        armor = 100;
        isAlive = true;
    }
    public void ThrowBase()
    {
        

        GameObject projectile = Instantiate(objectToThrow, attackPoint.position,transform.rotation);

        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = transform.forward;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward,out hit , 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
            float rnd = Random.Range(0, 0.1f);
            forceDirection.x += rnd;
            Debug.Log(forceDirection);
;            
        }

        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

    }

    void EndOfGame()
    {
        if (score == 10)
        {
            Finish.gameObject.SetActive(true);
        }
    }
   public void TakeDamage()
    {
        damage = Random.Range(30, 50);
        
        if (armor > 0)
        {
            armor -= damage * 80 / 100;
            health -= damage * 20 / 100;
        }
        else if(armor < 0)
        {
            health -= damage;
        }
    }
   
}
