using System;

using UnityEngine;

namespace NoSlipOnRoof.Roofs;

/// <summary>
/// Tells whether a collider belongs to a built roof piece. Every vanilla roof has "roof" in its prefab name
/// (<c>wood_roof_45</c>, <c>scale_wall_roof_67</c>, <c>piece_grausten_roof_45</c>) or in its name token
/// (<c>$piece_ashwoodwallrooftop67</c> on <c>ashwood_wall_cross_67</c>), so both are checked. That also
/// covers most modded roofs.
/// </summary>
internal static class RoofCheck
{
    private const string Roof = "roof";

    /// <summary>The player stands on the same collider for many frames, so the last answer is kept.</summary>
    private static Collider _lastCollider;
    private static bool _lastResult;

    public static bool IsRoof(Collider collider)
    {
        if (collider == null)
        {
            return false;
        }

        if (collider != _lastCollider)
        {
            _lastCollider = collider;
            _lastResult = Check(collider);
        }

        return _lastResult;
    }

    private static bool Check(Collider collider)
    {
        Piece piece = collider.GetComponentInParent<Piece>();
        return piece != null
            && (Mentions(Utils.GetPrefabName(piece.gameObject)) || Mentions(piece.m_name));
    }

    private static bool Mentions(string name)
    {
        return name?.IndexOf(Roof, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
