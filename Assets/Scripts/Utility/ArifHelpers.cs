using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public static class ArifHelpers
{
    public static bool DistanceCollider(GameObject g1, GameObject g2, float distance)
    {
        if (Vector3.Distance(g1.transform.position, g2.transform.position) < distance)
        {
            return true;
        }
        else
            return false;
    }

    private static readonly Dictionary<float, WaitForSeconds> waitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if (waitDictionary.TryGetValue(time, out WaitForSeconds wait)) return wait;

        waitDictionary[time] = new WaitForSeconds(time);
        return waitDictionary[time];
    }

    public static void DrawCircle(GameObject container, float radius, float lineWidth)
    {
        var segments = 360;
        var line = container.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }

    public static string ToTitleCase(string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
    }

    public static void ListPop<T>(List<T> list)
    {
        list.RemoveAt(list.Count - 1);
    }

    public static void FillImage(Image image, float current, int need)
    {
        image.fillAmount =  current/need;
    }
}
