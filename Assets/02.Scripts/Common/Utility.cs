using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    #region List Extensions

    // (Fisher-Yates)
    public static void Shuffle<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0) return;

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    #endregion


    #region Component Extensions


    public static void SetActive(this Component self, bool active)
    {
        if (self == null) return;

        self.gameObject.SetActive(active);
    }

    public static bool IsActive(this Component self)
    {
        if (self == null) return false;

        return self.gameObject.activeInHierarchy;
    }


    public static Vector3 GetPosition(this GameObject self)
    {
        if (self == null) return Vector3.zero;

        return self.transform.position;
    }

    public static void SetPosition(this GameObject self, Vector3 pos)
    {
        if (self == null) return;
        self.transform.position = pos;
    }


    #endregion

    #region string Extensions

    public static string ToColorText(this string self, string color)
    {
        if (string.IsNullOrEmpty(self)) return string.Empty;

        return $"<color={color}>{self}</color>";
    }

    public static string ToColorText(this string self, Color color)
    {
        if (string.IsNullOrEmpty(self)) return string.Empty;

        return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{self}</color>";
    }

    #endregion

    #region Mathf Extensions

    public static int Clamp(this int value, int min, int max)
    {
        return Mathf.Clamp(value, min, max);
    }

    public static float Clamp(this float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }

    #endregion

}
