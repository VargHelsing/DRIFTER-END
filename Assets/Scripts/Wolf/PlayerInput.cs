using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    #region Inspector
    public string horizontalAxis = "Horizontal";
    public string attackButton = "Fire1";
    public string jumpButton = "Jump";

    public WolfModel model;

    void OnValidate()
    {
        if (model == null)
            model = GetComponent<WolfModel>();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (model == null) return;

        float currentHorizontal = Input.GetAxisRaw(horizontalAxis);
        model.TryMove(currentHorizontal);

        if (Input.GetButton(attackButton))
            model.TryShoot();

        if (Input.GetButtonDown(jumpButton))
            model.TryJump();
    }
}

