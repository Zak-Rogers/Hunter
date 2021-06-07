using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 2.0f, chargeRate = 5.0f, maxCharge = 30.0f;
    float x, z, charge;
    Vector2 mouseDirection;
    NavMeshAgent agent;
    GameObject bow, arrow;
    Animator anim; 
    Transform bowPivot, releasePoint;
    [SerializeField] GameObject arrowPrefab;
    AudioSource[] characterAudio, bowAudio;
    [SerializeField] private float fireRate = 0.5f;
    private float fireRateTimer = 0;
    private bool canFire = true;
    private bool arrowKnocked = false;
        

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        bow = GameObject.FindGameObjectWithTag("Bow");
        bowPivot = bow.transform.parent;
        releasePoint = bow.transform.GetChild(0);

        Cursor.lockState = CursorLockMode.Locked;
        
        PlayerPrefs.SetFloat("charge", 0.0f);
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetFloat("time", 0.0f);

        // Get Audio
        characterAudio = GetComponents<AudioSource>();
        bowAudio = bow.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        charge = PlayerPrefs.GetFloat("charge");
        Movement();
        CameraMovement();

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        //Start firing, spawn arrow.
        if(Input.GetKeyDown(KeyCode.Mouse0)&& canFire)
        {
            arrow = Instantiate(arrowPrefab, releasePoint.position, Quaternion.identity, releasePoint); 
            arrowKnocked = true;
            arrow.transform.localRotation = Quaternion.Euler(180, 90, 0);
            bowAudio[0].Play();
        }

        //Charge shot.
        if (Input.GetKey(KeyCode.Mouse0) && canFire && arrowKnocked)
        {
            if (arrow != null)
            {
                Vector3 arrowLocalPosition = arrow.transform.localPosition;


                if (charge < maxCharge)
                {
                    charge += chargeRate * Time.deltaTime;
                    PlayerPrefs.SetFloat("charge", charge);
                    float pullBackSpeed = Time.deltaTime * 0.1f;
                    if (arrowLocalPosition.y <= 0 && arrowLocalPosition.y >= -0.23f)
                    {
                        arrow.transform.localPosition = new Vector3(arrowLocalPosition.x, arrowLocalPosition.y - pullBackSpeed, arrowLocalPosition.z);
                    }
                }
            }
        }

        //Release shot.
        if (Input.GetKeyUp(KeyCode.Mouse0) && canFire && arrowKnocked)
        {
            Fire();
            PlayerPrefs.SetFloat("charge", 0.0f);
            fireRateTimer = fireRate;
            canFire = false;
            arrowKnocked = false;
        }

        if(fireRateTimer <= 0)
        {
            canFire = true;
        }
        else
        {
            fireRateTimer -= Time.deltaTime;
        }
    }

    private void Movement()
    {
        x = Input.GetAxis("Horizontal") * speed;
        z = Input.GetAxis("Vertical") * speed;

        agent.Move(transform.forward * z * Time.deltaTime);
        agent.Move(transform.right * x * Time.deltaTime);
    }

    private void CameraMovement()
    {
        float mouseX, mouseY;

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        Vector2 mouseChange = new Vector2(mouseX, mouseY);
        
        //add to mouse direction
        mouseDirection += mouseChange;

        //rotate camera around x axis in the mouse change y direction
        Camera.main.transform.localRotation = Quaternion.AngleAxis(-mouseDirection.y, Vector3.right);

        //rotates the body in the x direction.
        transform.localRotation = Quaternion.AngleAxis(mouseDirection.x, Vector3.up);
        bowPivot.localRotation = Quaternion.AngleAxis(-mouseDirection.y, Vector3.right);

    }

    private void Fire()
    {
        arrow.GetComponent<ArrowScript>().Fire(charge);
        bowAudio[1].Play();
    }
}
