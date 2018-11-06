using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboControl : MonoBehaviour {
    public WolfController wolfController;
    Animator animator;
    bool ActivateTimerToReset = false;
    //The more bools, the less readibility, try to stick with the essentials.
    //If you were to press 10 buttons in a row
    //having 10 booleans to check for those would be confusing
    public bool meleePlaying = false;
    public float currentComboTimer;
    //public int currentComboState = -1;
    //public int maxCombo;
    public bool startComboCountdown = false;
    float origTimer;
    public float comboLength;
    public int clicklimit;
    //private int noOfClicks=0;
    public float meleeDur;
    public float meleeDurMax;
    public bool meleeAble= true;
    public float timeSinceMelee = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Store original timer reset duration
        origTimer = currentComboTimer;
    }

    // Update is called once per frame, yeah everybody knows this

    void Update()
    {
        NewComboSystem();

        //Initially set to false, so the method won't start
        animator.SetFloat("comboDur", currentComboTimer);
        ResetComboState(ActivateTimerToReset);
        //animator.SetInteger("comboCounter", currentComboState);
        StateCountDown();
       
    }

    void ResetComboState(bool resetTimer)
    {
        if (resetTimer)
        //if the bool that you pass to the method is true
        // (aka if ActivateTimerToReset is true, then the timer start
        {
            currentComboTimer -= Time.deltaTime;
            //If the parameter bool is set to true, a timer start, when the timer
            //runs out (because you don't press fast enought Z the second time)
            //currentComboState is set again to zero, and you need to press it twice again
            if (currentComboTimer <= 0)
            {
                //currentComboState = -1;
                currentComboTimer = origTimer;
                ActivateTimerToReset = false;
            }
        }
    }

    void NewComboSystem()
    {
        if (Input.GetButtonDown("Fire1") && meleeAble && !wolfController.aiming)
        {
            //currentComboTimer = origTimer;
            meleeDur = meleeDurMax;
            meleeAble = false;

            //currentComboState = (currentComboState + 1) % maxCombo;

            //No need to create a comboStateUpdate()
            //function while you can directly
            //increment a variable using ++ operator

            //Okay, you pressed fire once, so now the resetcombostate Function is
            //set to true, and the timer starts to reset the currcombostate

            animator.ResetTrigger("melee");
            animator.SetTrigger("melee");

            //switch (currentComboState)
            //{
            //    case 0:
            //        //animator.ResetTrigger("melee");
                    
            //        if (!meleePlaying)
            //        {
            //            StateCountDownStart(1f);
            //        }
            //        //Debug.Log(comboLength);
            //        break;
            //    case 1:
            //        //animator.ResetTrigger("melee");
            //        if (!meleePlaying)
            //        {
            //            StateCountDownStart(1f);
            //        }
            //        //Debug.Log("2 hit, The combo Should Start");
            //        break;
            //}


            //if (animator.GetCurrentAnimatorStateInfo(0).IsName("melee1")) animator.ResetTrigger("melee2");
            //if (animator.GetCurrentAnimatorStateInfo(0).IsName("melee2")) animator.ResetTrigger("melee1");

        }
    }

    void StateCountDownStart(float mod)
    {
        comboLength = animator.GetCurrentAnimatorStateInfo(0).length*mod;

        meleePlaying = true;
        animator.SetBool("meleeing", true);
    }

    void StateCountDown()
    {
        if (meleePlaying)
        {
            wolfController.meleeing = true;
            comboLength -= Time.deltaTime;
            if (comboLength <= 0)
            {
                ActivateTimerToReset = true;
                meleePlaying = false;
                animator.SetBool("meleeing",false);
            }
        } else
        {
            wolfController.meleeing = false;
        }

        if (!meleeAble)
        {
            meleeDur -= Time.deltaTime;
            if (meleeDur <= 0)
            {
                meleeAble = true;
            }
        }
    }
}
