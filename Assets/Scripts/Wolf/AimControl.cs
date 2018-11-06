using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimControl : MonoBehaviour {
    public bool aiming = false;
    public WolfController wolfController;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public float rotSpeed=5f;
    public bool unarmed = true;
    public float angle;
    public Quaternion targetRotation;
    public GameObject rightArm;
    public GameObject leftArm;
    public GameObject leftArmHolder;
    public GameObject rightArmHolder;
    public bool destroyTest;
    public GameObject crossHair;
    bool rFired=false;
    bool lFired=false;
    Vector2 mousePosition;
    Vector3 modPosR;
    Vector3 modPosL = new Vector3(0, 1.8f, 1);
    Vector2 leftPivot, rightPivot;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        

        modPosL = new  Vector3(transform.position.x+0.2f*spriteRenderer.transform.localScale.x, transform.position.y+1.8f, transform.position.z+1);
        modPosR = new Vector3(transform.position.x-0.2f*spriteRenderer.transform.localScale.x, transform.position.y+1.8f, transform.position.z-1);

        if (Input.GetButton("Fire2"))
        {
            aiming = true;
            animator.SetBool("aiming", true);
            wolfController.aiming = true;
        }
        else
        {
            aiming = false;
            wolfController.aiming = false;
            animator.SetBool("aiming", false);
        }

        if (aiming)
        {
            if (unarmed && animator.GetCurrentAnimatorStateInfo(0).IsName("Wolf_locomotion"))
            {
                unarmed = false;

                rightArmHolder = Instantiate(rightArm, modPosR, Quaternion.identity, gameObject.transform);
                leftArmHolder = Instantiate(leftArm, modPosL, Quaternion.identity, gameObject.transform);

                rightPivot = rightArmHolder.GetComponent<GunControl>().GetSpritePivot();

                leftPivot = leftArmHolder.GetComponent<GunControl>().GetSpritePivot();

            } else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (!rFired && rightArmHolder!=null)
                    {
                        rightArmHolder.GetComponent<GunControl>().fire = true;
                        rFired = true;
                        lFired = false;
                    }
                    else
                    {
                        if (!lFired && leftArmHolder!=null)
                        {
                            leftArmHolder.GetComponent<GunControl>().fire = true;
                            lFired = true;
                            rFired = false;
                        }
                    }
                }
            }
            
        }
        if(!aiming || wolfController.aerial || animator.GetCurrentAnimatorStateInfo(0).IsName("Wolf_quick"))
        {
            unarmed = true;
            if (!destroyTest)
            {
                Destroy(rightArmHolder);
                Destroy(leftArmHolder);
            }
        }
        MoveCross();

    }

    void MoveCross()
    {
        Vector3 direction = Input.mousePosition;
        direction.z = Camera.main.transform.position.z - transform.position.z;

        direction = transform.position*2 - Camera.main.ScreenToWorldPoint(direction);

        direction.z = 0;
        crossHair.transform.position = direction;

    }

    float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

}
