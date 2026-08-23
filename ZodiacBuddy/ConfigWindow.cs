using Dalamud.Bindings.ImGui;
using Dalamud.Game.Text;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.UI;
using System;
using System.Numerics;

namespace ZodiacBuddy;

/// <summary>
///     Plugin configuration window.
/// </summary>
internal class ConfigWindow : Window
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigWindow" /> class.
    /// </summary>
    public ConfigWindow() : base("ZodiacBuddy 設定")
    {
        RespectCloseHotkey = true;

        SizeCondition = ImGuiCond.FirstUseEver;
        Size = new Vector2(740, 490);
    }

    /// <inheritdoc />
    public override void Draw()
    {
        if (ImGui.CollapsingHeader("一般"))
        {
            DrawGeneral();
        }

        if (ImGui.CollapsingHeader("介面"))
        {
            DrawInterface();
        }

        if (ImGui.CollapsingHeader("光之加成"))
        {
            DrawBonusLight();
        }

        if (ImGui.CollapsingHeader("Atma"))
        {
            DrawAtma();
        }

        if (ImGui.CollapsingHeader("新星"))
        {
            DrawNovus();
        }

        if (ImGui.CollapsingHeader("黃道勇士"))
        {
            DrawBrave();
        }

        if (Service.Interface.IsDevMenuOpen && ImGui.CollapsingHeader("Debug"))
        {
            Debug();
        }
    }

    private void DrawGeneral()
    {
        var names = Enum.GetNames<XivChatType>();
        var channels = Enum.GetValues<XivChatType>();
        var current = Array.IndexOf(channels, Service.Configuration.ChatType);
        if (current == -1)
        {
            current = Array.IndexOf(channels, Service.Configuration.ChatType = XivChatType.Echo);
            Service.Configuration.Save();
        }

        ImGui.SetNextItemWidth(200f);
        if (ImGui.Combo("聊天頻道", ref current, names, names.Length))
        {
            Service.Configuration.ChatType = channels[current];
            Service.Configuration.Save();
        }

        ImGui.Spacing();

        if (ImGui.Checkbox("停用自動傳送", ref Service.Configuration.DisableTeleport))
        {
            Service.Configuration.Save();
        }

        if (ImGui.IsItemHovered())
        {
            ImGui.SetTooltip(
                "台服預設：開啟（不自動傳送）。\n" +
                "取消勾選後，在武器筆記本中點擊目標會自動將角色\n" +
                "傳送至最近的乙太之光（此動作會移動角色，且可能花費 Gil）。");
        }
    }

    private void DrawInterface()
    {
        var manualSize = Service.Configuration.InformationWindow.ManualSize;
        if (ImGui.Checkbox("手動設定黃道武器資訊視窗大小", ref manualSize))
        {
            Service.Configuration.InformationWindow.ManualSize = manualSize;
            Service.Configuration.Save();
        }

        var clickThrough = Service.Configuration.InformationWindow.ClickThrough;
        if (ImGui.Checkbox("黃道武器資訊視窗可穿透點擊", ref clickThrough))
        {
            Service.Configuration.InformationWindow.ClickThrough = clickThrough;
            Service.Configuration.Save();
        }

        ImGui.PushItemWidth(150f);
        var progressSize = Service.Configuration.InformationWindow.ProgressSize;
        if (ImGui.SliderInt("光之進度條大小", ref progressSize, 80, 500))
        {
            Service.Configuration.InformationWindow.ProgressSize = progressSize;
            Service.Configuration.Save();
        }

        ImGui.SameLine();
        var progressAutoSize = Service.Configuration.InformationWindow.ProgressAutoSize;
        if (ImGui.Checkbox("自動", ref progressAutoSize))
        {
            Service.Configuration.InformationWindow.ProgressAutoSize = progressAutoSize;
            Service.Configuration.Save();
        }

        var progressColor = ImGui.ColorConvertU32ToFloat4(Service.Configuration.InformationWindow.ProgressColor);
        if (ImGui.ColorEdit4("光之進度條顏色", ref progressColor, ImGuiColorEditFlags.DisplayHex | ImGuiColorEditFlags.PickerHueWheel))
        {
            Service.Configuration.InformationWindow.ProgressColor = ImGui.ColorConvertFloat4ToU32(progressColor);
            Service.Configuration.Save();
        }

        ImGui.PopItemWidth();
        ImGui.SameLine();
        if (ImGui.Button("重設"))
        {
            Service.Configuration.InformationWindow.ResetProgressColor();
            Service.Configuration.Save();
        }

        ImGui.Spacing();
    }

    private void DrawBonusLight()
    {
        string status;
        Vector4 statusColor;
        if (Service.BonusLightManager.LastRequestIsSuccess)
        {
            status = "正常";
            statusColor = ImGuiColors.HealerGreen;
        }
        else
        {
            status = "錯誤";
            statusColor = ImGuiColors.DalamudRed;
        }

        ImGui.Text("提示：光之加成資訊由社群共同回報彙整。");
        ImGui.Text("伺服器狀態：");
        ImGui.SameLine();
        ImGui.TextColored(statusColor, status);
        ImGui.Spacing();

        var displayBonusDuty = Service.Configuration.BonusLight.DisplayBonusDuty;
        if (ImGui.Checkbox("分享並顯示光之加成副本（社群伺服器）", ref displayBonusDuty))
        {
            Service.Configuration.BonusLight.DisplayBonusDuty = displayBonusDuty;
            Service.Configuration.Save();
        }

        if (ImGui.IsItemHovered())
        {
            ImGui.SetTooltip(
                "台服預設：關閉（不與外部伺服器連線）。\n" +
                "啟用後，外掛會與社群伺服器 zodiac-buddy-db.fly.dev 通訊：\n" +
                "取得目前擁有光之加成的副本清單，並回報你偵測到的\n" +
                "結果。回報內容包含你的角色 Content ID（未經雜湊處理）。\n" +
                "若不希望任何資料傳送到該伺服器，請保持關閉。");
        }

        ImGui.Separator();

        var notifyBonusDuty = Service.Configuration.BonusLight.NotifyLightBonusOnlyWhenEquipped;
        if (ImGui.Checkbox("僅在裝備對應黃道武器時才通知副本加成", ref notifyBonusDuty))
        {
            Service.Configuration.BonusLight.NotifyLightBonusOnlyWhenEquipped = notifyBonusDuty;
            Service.Configuration.Save();
        }

        var playSound = Service.Configuration.BonusLight.PlaySoundOnLightBonusNotification;
        if (ImGui.Checkbox("通知光之加成時播放音效", ref playSound))
        {
            Service.Configuration.BonusLight.PlaySoundOnLightBonusNotification = playSound;
            Service.Configuration.Save();
        }

        ImGui.SetNextItemWidth(150f);
        var soundId = Service.Configuration.BonusLight.LightBonusNotificationSound;
        if (ImGui.SliderInt("##LightBonusSound", ref soundId, 1, 16, "<se.%d>"))
        {
            Service.Configuration.BonusLight.LightBonusNotificationSound = soundId;
            Service.Configuration.Save();
        }

        ImGui.SameLine();
        if (ImGui.Button("播放音效##LightBonusSound"))
        {
            UIGlobals.PlayChatSoundEffect((uint)soundId);
        }

        ImGui.Spacing();
    }

    private void DrawAtma()
    {
        ImGui.Text("小技巧：搭配 Sonar 外掛可追蹤整個大區的緊急遭遇戰出現時間。\n");

        var braveEcho = Service.Configuration.BraveEchoTarget;
        if (ImGui.Checkbox("在聊天視窗顯示已選擇的目標", ref braveEcho))
        {
            Service.Configuration.BraveEchoTarget = braveEcho;
            Service.Configuration.Save();
        }

        var braveCopy = Service.Configuration.BraveCopyTarget;
        if (ImGui.Checkbox("自動複製目標名稱到剪貼簿", ref braveCopy))
        {
            Service.Configuration.BraveCopyTarget = braveCopy;
            Service.Configuration.Save();
        }

        ImGui.Spacing();
    }

    private void DrawNovus()
    {
        var showRelicWindow = Service.Configuration.Novus.DisplayRelicInfo;
        if (ImGui.Checkbox("裝備時顯示新星黃道武器資訊", ref showRelicWindow))
        {
            Service.Configuration.Novus.DisplayRelicInfo = showRelicWindow;
            Service.Configuration.Save();
        }

        var skipAnimation = Service.Configuration.Novus.DontPlayRelicGlassAnimation;
        if (ImGui.Checkbox("略過新星強化視窗的文字動畫", ref skipAnimation))
        {
            Service.Configuration.Novus.DontPlayRelicGlassAnimation = skipAnimation;
            Service.Configuration.Save();
        }

        var showNumbers = Service.Configuration.Novus.ShowNumbersInRelicGlass;
        if (ImGui.Checkbox("在新星強化視窗顯示光之數值", ref showNumbers))
        {
            Service.Configuration.Novus.ShowNumbersInRelicGlass = showNumbers;
            Service.Configuration.Save();
        }

        ImGui.Spacing();
    }

    private void DrawBrave()
    {
        var showRelicWindow = Service.Configuration.Brave.DisplayRelicInfo;
        if (ImGui.Checkbox("裝備時顯示黃道勇士武器資訊", ref showRelicWindow))
        {
            Service.Configuration.Brave.DisplayRelicInfo = showRelicWindow;
            Service.Configuration.Save();
        }

        var skipAnimation = Service.Configuration.Brave.DontPlayRelicMagiciteAnimation;
        if (ImGui.Checkbox("略過黃道勇士強化視窗的文字動畫", ref skipAnimation))
        {
            Service.Configuration.Brave.DontPlayRelicMagiciteAnimation = skipAnimation;
            Service.Configuration.Save();
        }

        var showNumbers = Service.Configuration.Brave.ShowNumbersInRelicMagicite;
        if (ImGui.Checkbox("在黃道勇士強化視窗顯示光之數值", ref showNumbers))
        {
            Service.Configuration.Brave.ShowNumbersInRelicMagicite = showNumbers;
            Service.Configuration.Save();
        }

        ImGui.Spacing();
    }

    private void Debug()
    {
        if (ImGui.Button("Check Light Bonus territory"))
        {
            DebugTools.CheckBonusLightDutyTerritories();
        }

        if (ImGui.Button("Check Brave books territory"))
        {
            DebugTools.CheckBraveDutyTerritory();
        }

        var bonusLightWindow = Util.CurrentBonusLightWindow();
        if (bonusLightWindow.HasValue)
        {
            var (startWindow, endWindow) = bonusLightWindow.Value;

            var startWindowServerTime = startWindow.ToString(@"HH\:mm");
            var endWindowServerTime = endWindow.ToString(@"HH\:mm");
            var startWindowLocal = startWindow.ToLocalTime().ToString(@"HH\:mm");
            var endWindowLocal = endWindow.ToLocalTime().ToString(@"HH\:mm");

            ImGui.Text(
                $"Current bonus light window: {startWindowLocal} - {endWindowLocal} ({startWindowServerTime} - {endWindowServerTime} Server Time)");
        }
        else
        {
            ImGui.Text($"No bonus light window found, check logs");
        }
    }
}
