using Dalamud.Configuration;
using Dalamud.Game.Text;
using Newtonsoft.Json;
using ZodiacBuddy.BonusLight;
using ZodiacBuddy.InformationWindow;
using ZodiacBuddy.Stages.Brave;
using ZodiacBuddy.Stages.Novus;

namespace ZodiacBuddy;

public class PluginConfiguration : IPluginConfiguration
{

    // 台服預設：不自動傳送。點擊武器筆記本項目時不會自動把角色傳送到最近的乙太之光
    // （避免非預期的位移與傳送費）。使用者可在設定視窗 General 分頁取消勾選「Disable Teleport」以重新啟用。
    public bool DisableTeleport = true;

    [JsonProperty("BraveEchoChannel")] public XivChatType ChatType { get; set; } = XivChatType.Echo;

    public bool BraveEchoTarget { get; set; } = true;

    public bool BraveCopyTarget { get; set; } = true;

    public BonusLightConfiguration BonusLight { get; } = new();

    public NovusConfiguration Novus { get; } = new();

    public BraveConfiguration Brave { get; } = new();

    public InformationWindowConfiguration InformationWindow { get; } = new();
    public int Version { get; set; } = 1;

    public void Save()
    {
        Service.Interface.SavePluginConfig(this);
    }
}
