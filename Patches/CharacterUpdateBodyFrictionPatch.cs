using HarmonyLib;

using NoSlipOnRoof.Roofs;

using UnityEngine;

namespace NoSlipOnRoof.Patches;

/// <summary>
/// Without the slide, the player's own collider friction is all that holds them when standing still. Vanilla
/// uses 0.8, which only holds up to about 38 degrees (friction must be at least the tangent of the slope), so
/// on 67 degree pieces (tangent about 2.4) gravity creeps the player down a little every physics step. While
/// a player stands still on a roof, their friction is raised enough to hold any roof. Walking keeps vanilla's
/// friction, so movement feels the same.
/// </summary>
[HarmonyPatch(typeof(Character), "UpdateBodyFriction")]
internal static class Character_UpdateBodyFriction_Patch
{
    /// <summary>Holds slopes up to about 80 degrees, steeper than any vanilla roof.</summary>
    private const float HoldFriction = 7f;

    /// <summary>Vanilla's "standing still" threshold for movement input in <c>UpdateBodyFriction</c>.</summary>
    private const float IdleMoveThreshold = 0.1f;

    [HarmonyPostfix]
    private static void Postfix(
        Character __instance,
        CapsuleCollider ___m_collider,
        Collider ___m_lastGroundCollider,
        Vector3 ___m_moveDir)
    {
        if (__instance is not Player
            || ___m_moveDir.magnitude >= IdleMoveThreshold
            || !__instance.IsOnGround()
            || __instance.IsDead()
            || __instance.IsSwimming()
            || !RoofCheck.IsRoof(___m_lastGroundCollider))
        {
            return;
        }

        PhysicsMaterial material = ___m_collider.material;
        material.staticFriction = HoldFriction;
        material.dynamicFriction = HoldFriction;
        material.frictionCombine = PhysicsMaterialCombine.Maximum;
    }
}
