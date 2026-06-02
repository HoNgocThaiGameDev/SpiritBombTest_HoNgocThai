using UnityEngine;

public static class PlayerPlaneSpriteApplicator
{
    public static void ApplyToHierarchy(GameObject root, PlayerPlaneConfigSO config)
    {
        if (root == null || config == null)
            return;

        SpriteRenderer[] renderers = root.GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            ApplyRenderer(renderers[i], config);
        }
    }

    public static void ApplyToMenuPreviews(PlayerPlaneConfigSO config)
    {
        if (config == null)
            return;

        SpriteRenderer[] renderers = Object.FindObjectsOfType<SpriteRenderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            SpriteRenderer renderer = renderers[i];
            if (HasAncestor(renderer.transform, "GeneralDialog") || HasAncestor(renderer.transform, "UpgradeDialog"))
            {
                ApplyRenderer(renderer, config);
            }
        }
    }

    private static void ApplyRenderer(SpriteRenderer renderer, PlayerPlaneConfigSO config)
    {
        if (renderer == null)
            return;

        if (renderer.name == "Main")
        {
            if (config.planeSprite != null)
                renderer.sprite = config.planeSprite;
            return;
        }

        if (IsSupportRenderer(renderer.transform) && config.supportPlaneSprite != null)
        {
            renderer.sprite = config.supportPlaneSprite;
        }
    }

    private static bool IsSupportRenderer(Transform current)
    {
        if (current == null)
            return false;

        if (current.name == "Sp" || current.name.StartsWith("Sp ("))
            return true;

        while (current != null)
        {
            if (current.name == "Support")
                return true;

            current = current.parent;
        }

        return false;
    }

    private static bool HasAncestor(Transform current, string name)
    {
        while (current != null)
        {
            if (current.name == name)
                return true;

            current = current.parent;
        }

        return false;
    }
}
