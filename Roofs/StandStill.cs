namespace NoSlipOnRoof.Roofs;

/// <summary>
/// Holding a player still on a steep roof: raised friction while idle
/// (<see cref="Patches.Character_UpdateBodyFriction_Patch"/>) and pulling them back to where they stopped
/// (<see cref="Patches.Character_ApplyGroundForce_Patch"/>). Without it, the slide is still skipped, so the player
/// only creeps slowly down 67 degree roofs while standing still.
/// </summary>
internal static class StandStill
{
    public const string Feature = "Stand still on roofs";
}
