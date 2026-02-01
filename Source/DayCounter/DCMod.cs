using UnityEngine;
using Verse;

namespace DayCounter
{
    public class DCMod : Mod
    {
        public static DCModSettings s_Settings;
        public static ModContentPack s_ModContent;

        public DCMod(ModContentPack content)
            : base(content)
        {
            s_Settings = GetSettings<DCModSettings>();
            s_ModContent = content;
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            if (s_Settings == null)
            {
                Log.Error("[DayCounter] DCModSettings is null.");
                return;
            }

            s_Settings.DoWindowContents(inRect);
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() => DCData.ModName;

        public override void WriteSettings() => base.WriteSettings();
    }
}
