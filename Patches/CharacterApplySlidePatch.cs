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
    [HarmonyPrefix]
    private static bool Prefix(Character __instance, Collider ___m_lastGroundCollider, ref float ___m_slippage)
    {
        if (__instance is not Player || !RoofCheck.IsRoof(___m_lastGroundCollider))
        {
            return true;
        }

        // What vanilla does on shallow ground. This also gives the player their normal friction back.
        ___m_slippage = 0f;
        return false;
    }
}
