using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static float minX = -118f;
    public static float maxX = 118f;
    public static float minY = -118f;
    public static float maxY = 118f;

    public static Color invisible = new Color(1f, 1f, 1f, 0f);
    public static Color visible = new Color(1f, 1f, 1f, 1f);

    public static bool IsDistanceInsignificant(Vector2 pos1, Vector2 pos2, float insignificantDistance)
    {
        float distance = Mathf.Abs(Vector2.Distance(pos1, pos2));

        float insignificantThreshold = insignificantDistance;

        return distance < insignificantThreshold;
    }

    public static bool IsInGameMap(Vector2 point)
    {
        return point.x < maxX && point.x > minX && point.y < maxY && point.y > minY;
    }
}
