using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using ScrapInsurance.Misc.Util;
using System.Runtime.Serialization;

namespace ScrapInsurance.Misc
{
    [DataContract]
    public class PluginConfig : SyncedConfig2<PluginConfig>
    {
        [field: SyncedEntryField] public SyncedEntry<int> SCRAP_INSURANCE_PRICE { get; set; }
        public PluginConfig(ConfigFile cfg) : base(Metadata.GUID)
        {
            string topSection = "General";
            SCRAP_INSURANCE_PRICE = cfg.BindSyncedEntry(topSection, Constants.SCRAP_INSURANCE_PRICE_KEY, ScrapInsuranceBehaviour.DEFAULT_PRICE);
        }
    }
}
