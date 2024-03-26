using UnityEngine;

public static class Helper
{
    public static AnimationClip FindAnimation (Animator animator, string name) 
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }
}
