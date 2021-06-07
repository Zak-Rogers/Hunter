using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] float speed = 1.5f, destructionTime = 20.0f;
    bool release = false;
    bool held = false;
    float chargePower, timer;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer = 0.0f;
        held = true;
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Bow").GetComponent<Collider>());
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>());
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Arrow").GetComponent<Collider>());
    }

    void FixedUpdate()
    {
        if (!held)
        {
            timer += Time.deltaTime;
        }
        
        if(release)
        {
            transform.parent = null;
            rb.AddForce(-transform.up * speed * chargePower * Time.fixedDeltaTime, ForceMode.Impulse); 
            release = false;
        }

        if(timer >= destructionTime)
        {
            Destroy(gameObject);
        }
    }

    public void Fire(float charge)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        release = true;
        held = false;
        chargePower = charge;
    }

    private void OnTriggerEnter(Collider other)
    {
        int score = PlayerPrefs.GetInt("score");
        if(other.tag == "Fox")
        {
            score += 20;

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "Rabbit")
        {
            score += 10;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        PlayerPrefs.SetInt("score", score);
    }
}
