using UnityEngine;

public class MyUtils
{
    public const string ATTACKER_WIN_COUNT = "AttackerWinCount";
    public const string DEFENDER_WIN_COUNT = "DefenderWinCount";
    public const string MATCH_COUNT = "MatchCount";

    public static Vector3 TranslateOnXZPlane(in Vector3 currentPosition, in Vector3 targetPosition, float step)
    {
        Vector2 currentPosition2d = new Vector2(currentPosition.x, currentPosition.z); //taking only the xz values
        Vector2 targetPosition2d = new Vector2(targetPosition.x, targetPosition.z);

        Vector2 result = Vector2.MoveTowards(currentPosition2d, targetPosition2d, step);

        return new Vector3(result.x, 0.0f, result.y);
    }

    public static Vector3 GetDirectionInXZPlane(in Vector3 currentPosition, in Vector3 targetPosition)
    {
        Vector2 currentPosition2d = new Vector2(currentPosition.x, currentPosition.z); //taking only the xz values
        Vector2 targetPosition2d = new Vector2(targetPosition.x, targetPosition.z);

        Vector2 direction = (targetPosition2d - currentPosition2d).normalized;
        return new Vector3(direction.x, 0.0f, direction.y);
    }
}
