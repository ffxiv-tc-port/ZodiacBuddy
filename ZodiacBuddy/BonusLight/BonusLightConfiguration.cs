using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZodiacBuddy.BonusLight;

/// <summary>
///     Configuration class for Nexus relic.
/// </summary>
public class BonusLightConfiguration
{
    /// <summary>
    ///     Gets the list of Territory Id of duty with bonus of light.
    /// </summary>
    [JsonIgnore] public List<uint> ActiveBonus { get; } = [];

    /// <summary>
    ///     Gets or sets a value indicating whether to participate in the crowdsourced bonus-light feature.
    ///     When enabled, the plugin both fetches active bonus-light duties from the community server and reports
    ///     your own detections to it, and displays them on the Novus information window.
    ///     台服預設關閉：不與外部社群伺服器（zodiac-buddy-db.fly.dev）通訊，也不上傳角色資訊。
    ///     這個開關同時控管「顯示」與「對外連線」——見 BonusLightManager 的 RetrieveLastReport / SendReport 閘門。
    /// </summary>
    public bool DisplayBonusDuty { get; set; } = false;

    /// <summary>
    ///     Gets or sets a value indicating whether to notify the user of new duty with bonus light when the relic is not
    ///     equipped.
    /// </summary>
    public bool NotifyLightBonusOnlyWhenEquipped { get; set; } = true;

    /// <summary>
    ///     Gets or sets a value indicating whether play sound when notifying about the bonus of light.
    /// </summary>
    public bool PlaySoundOnLightBonusNotification { get; set; } = true;

    /// <summary>
    ///     Gets or sets the sound to play when notifying about the bonus of light.
    /// </summary>
    public int LightBonusNotificationSound { get; set; } = 9;
}
