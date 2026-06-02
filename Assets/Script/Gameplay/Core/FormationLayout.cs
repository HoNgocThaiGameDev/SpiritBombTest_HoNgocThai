using UnityEngine;

public static class FormationLayout
{
    public static float GetTargetY(int rowPosition)
    {
        const float firstRowY = 2.45f;
        const float rowSpacing = 0.48f;
        return firstRowY - Mathf.Max(0, rowPosition) * rowSpacing;
    }

    public static float GetDiagonalTargetY(int rowPosition, int columnPosition)
    {
        const float firstRowY = 2.75f;
        const float parallelLineSpacing = 0.9f;
        const float diagonalStep = 0.42f;
        return firstRowY
            - Mathf.Max(0, rowPosition) * parallelLineSpacing
            - Mathf.Max(0, columnPosition) * diagonalStep;
    }

    public static float GetColumnX(int columnPosition, int columnCount, float visibleLimit)
    {
        int safeColumnCount = Mathf.Max(1, columnCount);
        if (safeColumnCount == 1)
            return 0f;

        float normalized = Mathf.Clamp01((float)Mathf.Clamp(columnPosition, 0, safeColumnCount - 1) / (safeColumnCount - 1));
        return Mathf.Lerp(-visibleLimit, visibleLimit, normalized);
    }

    public static float GetSineAmplitude(float visibleLimit)
    {
        return Mathf.Min(visibleLimit * 0.72f, 1.45f);
    }

    public static float GetCircleRadius(float visibleLimit)
    {
        return Mathf.Min(visibleLimit * 0.62f, 1.15f);
    }
}
