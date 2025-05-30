using UnityEngine;
using Verse;

namespace DayCounter
{
    public class DayCounterMod : Mod
    {
        public static DCSettings s_Settings;

        public DayCounterMod(ModContentPack content)
            : base(content)
        {
            s_Settings = GetSettings<DCSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            s_Settings.DoWindowContents(inRect);
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() => DCData.ModName;

        public override void WriteSettings() => base.WriteSettings();
    }
}
