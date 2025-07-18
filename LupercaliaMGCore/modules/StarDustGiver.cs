using System.Runtime.InteropServices.Marshalling;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using StarCore.Utils;
using TNCSSPluginFoundation.Models.Plugin;

namespace LupercaliaMGCore.modules;

public sealed class StarDustGiver(IServiceProvider serviceProvider) : PluginModuleBase(serviceProvider)
{
    public override string PluginModuleName => "StarDustGiver";

    public override string ModuleChatPrefix => "[Hoshi-Star]";
    protected override bool UseTranslationKeyInModuleChatPrefix => false;

    protected override void OnInitialize()
    {
        Plugin.RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath, HookMode.Post);
        Plugin.RegisterEventHandler<EventRoundEnd>(OnRoundEnd, HookMode.Post);
    }

    protected override void OnUnloadModule()
    {
        Plugin.DeregisterEventHandler<EventPlayerDeath>(OnPlayerDeath, HookMode.Post);
        Plugin.DeregisterEventHandler<EventRoundEnd>(OnRoundEnd, HookMode.Post);
    }

    private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        if (Lib.IsWarmupPeriod()) return HookResult.Continue;
        CCSPlayerController? victim = @event.Userid;
        CCSPlayerController? attacker = @event.Attacker;

        if (!Lib.IsPlayerValid(victim)) return HookResult.Continue;
        if (!Lib.IsPlayerValidAlive(attacker)) return HookResult.Continue;
        if (victim == attacker) return HookResult.Continue;
        
        var playerCount = Utilities.GetPlayers().Count;
        if (playerCount < 20) return HookResult.Continue;

        StarDust.GiveStarDust(attacker, 1, "击杀");

        return HookResult.Continue;
    }

    private HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        var playerCount = Utilities.GetPlayers().Count;
        if (playerCount < 20) return HookResult.Continue;
        foreach (var player in Utilities.GetPlayers())
        {
            if (!Lib.IsPlayerValid(player)) continue;
            StarDust.GiveStarDust(player, 1, "回合结束");
        }
        return HookResult.Continue;
    }
}