using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D.Utilities 
{
    public static class Utilities
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, list.Count);
                T value = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = value;
            }
        }

        public static Vector3 WorldToUISpace(Canvas canvas, Vector3 worldPosition)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out Vector2 localPoint);
            return canvas.transform.TransformPoint(localPoint);
        }
    }
}

