using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ZodiacBuddy.BonusLight;

namespace ZodiacBuddy;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class Service
{
    [PluginService] public static IDalamudPluginInterface Interface { get; set; }
    [PluginService] public static IChatGui ChatGui { get; set; }
    [PluginService] public static IClientState ClientState { get; set; }

    // API13 把 IClientState.LocalPlayer / LocalContentId 標為過時，替代品分別是
    // IObjectTable.LocalPlayer 與 IPlayerState.ContentId。Dalamud 端的舊成員本身就是
    // 對這兩者的純轉發（LocalContentId 另含 IsLoaded 判斷），改用新成員不改變行為。
    [PluginService] public static IObjectTable Objects { get; set; }
    [PluginService] public static IPlayerState PlayerState { get; set; }

    [PluginService] public static IDutyState DutyState { get; set; }
    [PluginService] public static ICommandManager CommandManager { get; set; }
    [PluginService] public static IDataManager DataManager { get; set; }
    [PluginService] public static IFramework Framework { get; set; }
    [PluginService] public static IGameGui GameGui { get; set; }
    [PluginService] public static IToastGui Toasts { get; set; }
    [PluginService] public static IPluginLog PluginLog { get; set; }
    [PluginService] public static IAddonLifecycle AddonLifecycle { get; set; }

    public static ZodiacBuddyPlugin Plugin { get; set; } = null!;
    public static PluginConfiguration Configuration { get; set; } = null!;
    public static BonusLightManager BonusLightManager { get; set; } = null!;
}
