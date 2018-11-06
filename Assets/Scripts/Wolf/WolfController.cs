using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class WolfController : PhysicsObject {
    public float maxSpeed;
    public float minSpeed;
    public float currentSpeed;
    public float dragSpeed;
    public float maxDragSpeed;
    public float jumpTakeOffSpeed;
    public float lastMoveX;
    private Vector2 lastMove;

    public bool aiming = false;

    public bool meleeing = false;

    public bool aerial = false;

    public float xX;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentSpeed = minSpeed;
        Vector2 lastMove = Vector2.zero;

    }
    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");
        if (meleeing)
        {
            move = Vector2.zero;
        }
        computeSpeed(move.x);
        if (!meleeing)
        {
            doJump();
        }
        rotateScale(move.x);
        animatorCon(move.x);
        if (aerial) animator.ResetTrigger("melee");
        targetVelocity = move * currentSpeed + lastMove * dragSpeed;
    }

    void computeSpeed(float x)
    {
        if ((x > 0 && x >= lastMoveX) || (x < 0 && x <= lastMoveX))
        {
            lastMoveX = Mathf.Sign(x);
            dragSpeed = maxDragSpeed;
            currentSpeed = currentSpeed + 0.35f;
        }
        else
        {
            currentSpeed = minSpeed;
            dragSpeed = dragSpeed - 0.2f;
            lastMove.x = lastMoveX;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
        dragSpeed = Mathf.Clamp(dragSpeed, 0, maxDragSpeed);

    }

    void doJump()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else
        {
            if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                    velocity.y = velocity.y * 0.7f;
            }
        }
        if (velocity.y != 0)
        {
            aerial = true;
        }
        else aerial = false;
    }

    void rotateScale(float x)
    {
        Vector3 theScale = spriteRenderer.transform.localScale;
        Vector3 theLocal = spriteRenderer.transform.localPosition;
        if (!aiming)
        {
            if (x != 0)
                theScale.x = ((x < 0) ? -1 : 1);
        } else
        {
            Vector3 direction = Input.mousePosition;
            direction.z = Camera.main.transform.position.z - transform.position.z;
            direction = transform.position - Camera.main.ScreenToWorldPoint(direction);
            xX = direction.x;

            if (direction.x >0 )
            {
                theScale.x = 1;
            } else
            {
                theScale.x = -1;
            }
        }
       
        spriteRenderer.transform.localScale = theScale;
    }

    void doCombo()
    {
        
    }

    void animatorCon(float x)
    {
        animator.SetBool("grounded", grounded);
        animator.SetFloat("hspeed", Mathf.Abs(x) * currentSpeed);
        animator.SetFloat("vspeed", velocity.y);
    }
}
