using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using StarCore.Utils;

namespace LupercaliaMGCore;

public static class StarDust
{
    public static void GiveStarDust(CCSPlayerController playerController, int amount, string reason)
    {
        if (!Lib.IsPlayerValid(playerController) || amount < 1 || string.IsNullOrEmpty(reason) || Lib.IsWarmupPeriod()) return;
        var curPlayers = Utilities.GetPlayers().Count;
        LupercaliaMGCore.StarDustStoreServices.Get()?.GivePlayerStarDust(playerController, amount);
        Server.NextFrame(() =>
        {
            if (!Lib.IsPlayerValid(playerController)) return;
            playerController.PrintToChat($" {ChatColors.Purple}[私信] {ChatColors.White}获得 {amount} 星尘 - {reason}[!sds打开星尘商店]");
        });
    }
}