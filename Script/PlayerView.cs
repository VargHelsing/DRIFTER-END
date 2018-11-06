using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;



public class PlayerView : MonoBehaviour
{

    #region Inspector
    [Header("Components")]
    public WolfModel model;
    public SkeletonAnimation skeletonAnimation;

    public AnimationReferenceAsset run, idle, melee, jump;

    #endregion

    WolfBodyState previousViewState;

    void Start()
    {
        if (skeletonAnimation == null) return;
        model.MeleeEvent += PlayMelee;
    }

    void Update()
    {
        if (skeletonAnimation == null) return;
        if (model == null) return;

        if (skeletonAnimation.skeleton.FlipX != model.facingLeft)
        {   // Detect changes in model.facingLeft
            Turn(model.facingLeft);
        }

        // Detect changes in model.state
        var currentModelState = model.state;

        if (previousViewState != currentModelState)
        {
            PlayNewStableAnimation();
        }

        previousViewState = currentModelState;
    }

    void PlayNewStableAnimation()
    {
        var newModelState = model.state;
        Spine.Animation nextAnimation;

        // Add conditionals to not interrupt transient animations.

        if (newModelState == WolfBodyState.Jumping)
        {
            nextAnimation = jump;
        }
        else
        {
            if (newModelState == WolfBodyState.Running)
            {
                nextAnimation = run;
            }
            else
            {
                    nextAnimation = idle;

            }
        }
        if (newModelState == WolfBodyState.Jumping)
            skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, false);
        else skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
    }


    [ContextMenu("Check Tracks")]
    void CheckTracks()
    {
        var state = skeletonAnimation.AnimationState;
        Debug.Log(state.GetCurrent(0));
        Debug.Log(state.GetCurrent(1));
    }

    #region Transient Actions
    public void PlayMelee()
    {
        var track = skeletonAnimation.AnimationState.SetAnimation(1, melee, false);
        track.AttachmentThreshold = 0f;
        track.MixDuration = 0f;
        var empty = skeletonAnimation.state.AddEmptyAnimation(1, 1.7f, 0.1f);
        empty.AttachmentThreshold = 1f;

    }

    public void Turn(bool facingLeft)
    {
        skeletonAnimation.Skeleton.FlipX = facingLeft;
        // Maybe play a transient turning animation too, then call ChangeStableAnimation.
    }
    #endregion

    #region Utility
    public float GetRandomPitch(float maxPitchOffset)
    {
        return 1f + Random.Range(-maxPitchOffset, maxPitchOffset);
    }
    #endregion
}
