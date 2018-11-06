using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfModel : MonoBehaviour {
    #region Inspector
    [Header("Current State")]
    public WolfBodyState state;
    public bool facingLeft;
    [Range(-1f, 1f)]
    public float currentSpeed;

    [Header("Balance")]
    public float meleeInterval = 0.9f;
    #endregion

    float lastMeleeTime;
    public event System.Action MeleeEvent;  // Lets other scripts know when Spineboy is shooting. Check C# Documentation to learn more about events and delegates.

    #region API
    public void TryJump()
    {
        StartCoroutine(JumpRoutine());
    }

    public void TryShoot()
    {
        float currentTime = Time.time;

        if (currentTime - lastMeleeTime > meleeInterval)
        {
            lastMeleeTime = currentTime;
            if (MeleeEvent != null) MeleeEvent();   // Fire the "ShootEvent" event.
        }
    }

    public void TryMove(float speed)
    {
        currentSpeed = speed; // show the "speed" in the Inspector.

        if (speed != 0)
        {
            bool speedIsNegative = (speed < 0f);
            facingLeft = speedIsNegative; // Change facing direction whenever speed is not 0.
        }

        if (state != WolfBodyState.Jumping)
        {
            state = (speed == 0) ? WolfBodyState.Idle : WolfBodyState.Running;
        }

    }
    #endregion

    IEnumerator JumpRoutine()
    {
        if (state == WolfBodyState.Jumping) yield break;   // Don't jump when already jumping.

        state = WolfBodyState.Jumping;

        // Fake jumping.
        {
            var pos = transform.localPosition;
            const float jumpTime = 0.7f;
            const float half = jumpTime * 0.5f;
            const float jumpPower = 20f;
            for (float t = 0; t < half; t += Time.deltaTime)
            {
                float d = jumpPower * (half - t);
                transform.Translate((d * Time.deltaTime) * Vector3.up);
                yield return null;
            }
            for (float t = 0; t < half; t += Time.deltaTime)
            {
                float d = jumpPower * t;
                transform.Translate((d * Time.deltaTime) * Vector3.down);
                yield return null;
            }
            transform.localPosition = pos;
        }

        state = WolfBodyState.Idle;
    }
}

public enum WolfBodyState
{
    Idle,
    Running,
    Jumping,
    Melee
}



