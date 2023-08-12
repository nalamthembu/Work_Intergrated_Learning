using System.Collections.Generic;
using UnityEngine;

public static class Extentions
{
    #region ANIMATOR_EXTENTIONS
    public static void CrossFade(this Animator animator, CrossFadeSettings settings)
    {
        animator.CrossFade
            (
                settings.stateName,
                settings.transitionDuration,
                settings.layer,
                settings.timeOffset
            );
    }

    public static void CrossFadeInFixedTime(this Animator animator, CrossFadeSettings settings)
    {
        animator.CrossFadeInFixedTime
            (
                settings.stateName,
                settings.transitionDuration,
                settings.layer,
                settings.timeOffset
            );
    }

    #endregion

    #region GAMEOBJECT_EXTENTIONS
    public static GameObject FindInActiveObjectByName(this GameObject gameObject, string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    public static GameObject FindInActiveObjectByTag(this GameObject gameObject, string tag)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    public static GameObject FindInActiveObjectByLayer(this GameObject gameObject, int layer)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].gameObject.layer == layer)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    public static GameObject[] FindInActiveObjectsByName(this GameObject gameObject, string name)
    {
        List<GameObject> validTransforms = new List<GameObject>();
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].gameObject.name == name)
                {
                    validTransforms.Add(objs[i].gameObject);
                }
            }
        }
        return validTransforms.ToArray();
    }

    public static GameObject[] FindInActiveObjectsByTag(this GameObject gameObject, string tag)
    {
        List<GameObject> validTransforms = new List<GameObject>();
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].gameObject.CompareTag(tag))
                {
                    validTransforms.Add(objs[i].gameObject);
                }
            }
        }
        return validTransforms.ToArray();
    }

    public static GameObject[] FindInActiveObjectsByLayer(this GameObject gameObject, int layer)
    {
        List<GameObject> validTransforms = new List<GameObject>();
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].gameObject.layer == layer)
                {
                    validTransforms.Add(objs[i].gameObject);
                }
            }
        }
        return validTransforms.ToArray();
    }
    #endregion
}
