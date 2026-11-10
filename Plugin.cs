using BepInEx;

using HarmonyLib;

using Jotunn.Utils;

namespace NoSlipOnRoof;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[BepInDependency(Jotunn.Main.ModGuid)]
[NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.None)]
public class NoSlipOnRoofPlugin : BaseUnityPlugin
{
    public const string PluginGUID = "javidahmed64592.nosliponroof";
    public const string PluginName = "No Slip On Roof";
    public const string PluginVersion = "0.1.0";

    private Harmony _harmony;

    private void Awake()
    {
        _harmony = new Harmony(PluginGUID);
        _harmony.PatchAll();
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}
