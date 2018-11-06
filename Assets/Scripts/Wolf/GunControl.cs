using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour {
    public float pivotXOffset;
    public float pivotYOffset;
    public float rotSpeed = 1f;
    public SpriteRenderer spriteRenderer;
    Animator animator;
    public GameObject wolf;
    public float offset;
    public float limiterMax;
    public float limiterMin;
    public float angle;
    public float xX;
    public bool fire;
    //public float yY;
    //public float zZ;

    // Use this for initialization
    void Start () {
        GetSpritePivot();
        spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        LookAt();
        if (fire)
        {
            animator.SetTrigger("fire");
            fire = false;
        }
	}

    public Vector2 GetSpritePivot()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Bounds bounds = sprite.bounds;
        var pivotX = -bounds.center.x / bounds.extents.x / 2 + pivotXOffset;
        var pivotY = -bounds.center.y / bounds.extents.y / 2 + pivotYOffset;

        return new Vector2(pivotX, pivotY);
    }
    protected void LookAt()
    {
        Vector3 direction = Input.mousePosition;
        direction.z = Camera.main.transform.position.z - transform.position.z;
        
        direction = transform.position - Camera.main.ScreenToWorldPoint(direction);
        //xX = direction.x;
        //yY = direction.y;
        //zZ = direction.z;

        if (spriteRenderer.transform.localScale.x == 1)
        {
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg +offset;
            angle = Mathf.Clamp(angle, limiterMin, limiterMax);
        } else
        {
            angle = Mathf.Atan2(direction.y*-1, direction.x*-1) * Mathf.Rad2Deg - offset;
            angle = Mathf.Clamp(angle, -limiterMax, -limiterMin);
        }
        //transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed*Time.deltaTime);
        //TEST.transform.position = direction;
    }
}
