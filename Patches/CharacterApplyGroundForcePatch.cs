using HarmonyLib;

using NoSlipOnRoof.Roofs;

using UnityEngine;

namespace NoSlipOnRoof.Patches;

/// <summary>
/// Even with high friction, the physics solver lets a player standing still on a 67 degree roof creep down
/// over a few minutes. Vanilla solves the same problem on ships in <c>ApplyGroundForce</c>: when the player
/// stops, it remembers where they stood and each frame pulls them back horizontally by ten times the drift,
/// letting go once they move or drift 4m. This does the same on roofs. Roofs don't move, so the spot is kept
/// in world space, and pinning the horizontal position is enough because the roof surface fixes the height.
/// </summary>
[HarmonyPatch(typeof(Character), "ApplyGroundForce")]
internal static class Character_ApplyGroundForce_Patch
{
    /// <summary>Vanilla's values for standing still on a ship: below this target speed counts as idle.</summary>
    private const float IdleTargetSpeed = 0.01f;
    private const float MaxDrift = 4f;
    private const float PullStrength = 10f;

    private static Character _anchored;
    private static Vector3 _anchor;

    [HarmonyPostfix]
    private static void Postfix(
        Character __instance,
        ref Vector3 vel,
        Vector3 targetVel,
        Rigidbody ___m_body,
        Collider ___m_lastGroundCollider,
        Vector3 ___m_pushForce)
    {
        bool hold = __instance is Player
            && __instance.IsOnGround()
            && targetVel.magnitude <= IdleTargetSpeed
            && ___m_pushForce == Vector3.zero // let knockback move the player
            && RoofCheck.IsRoof(___m_lastGroundCollider);

        if (!hold)
        {
            if (_anchored == __instance)
            {
                _anchored = null;
            }

            return;
        }

        if (_anchored != __instance)
        {
            _anchored = __instance;
            _anchor = ___m_body.position;
        }

        Vector3 drift = _anchor - ___m_body.position;
        drift.y = 0f;
        if (drift.magnitude < MaxDrift)
        {
            vel += drift * PullStrength;
        }
        else
        {
            _anchored = null;
        }
    }
}
