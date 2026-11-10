using HarmonyLib;

using NoSlipOnRoof.Roofs;

using UnityEngine;

namespace NoSlipOnRoof.Patches;

/// <summary>
/// Vanilla makes a player slide on any ground steeper than 38 degrees, which is why 26 degree roofs are fine
/// but 45 and 67 degree ones are not. While a player stands on a roof piece the slide is skipped, so they
/// keep their footing like on a shallow roof. Monsters, mounts and other ground are unchanged.
/// </summary>
[HarmonyPatch(typeof(Character), "ApplySlide")]
internal static class Character_ApplySlide_Patch
{
    /// <summary>Vanilla's "standing still" threshold for movement input (see <c>UpdateBodyFriction</c>).</summary>
    private const float IdleMoveThreshold = 0.1f;

    [HarmonyPrefix]
    private static bool Prefix(
        Character __instance,
        ref Vector3 currentVel,
        Collider ___m_lastGroundCollider,
        Vector3 ___m_moveDir,
        ref float ___m_slippage)
    {
        if (__instance is not Player || !RoofCheck.IsRoof(___m_lastGroundCollider))
        {
            return true;
        }

        // What vanilla does on shallow ground. This also gives the player their normal friction back.
        ___m_slippage = 0f;

        // Standing-still friction (0.8) only holds up to about 38 degrees, so on the steepest pieces gravity
        // still creeps the player down. Cancel any downward velocity while there's no movement input; jumping
        // (upward) and walking are left alone.
        if (___m_moveDir.magnitude < IdleMoveThreshold && currentVel.y < 0f)
        {
            currentVel.y = 0f;
        }

        return false;
    }
}
