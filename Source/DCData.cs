using UnityEngine;
using Verse;

namespace DayCounter
{
    [StaticConstructorOnStartup]
    public static class DCData
    {
        public static string ModName => "Day Counter";

        public static string TextDay => "TextDay".Translate();

        public static string Label_TextEnabled => "Label_TextEnabled".Translate();
        public static string Label_TextOriginUpRight => "Label_TextOriginUpRight".Translate();
        public static string Label_TextOutline => "Label_TextOutline".Translate();

        public static string Label_TextOffsetX => "Label_TextOffsetX".Translate();
        public static string Label_TextOffsetY => "Label_TextOffsetY".Translate();
        public static string Label_TextScale => "Label_TextScale".Translate();
        public static string Label_TextColor => "Label_TextColor".Translate();
        public static string Label_TextOutlineColor => "Label_TextOutlineColor".Translate();

        public static string Label_TextOutlineThickness => "Label_TextOutlineThickness".Translate();

        public static bool Default_Enabled => true;
        public static bool Default_OriginUpRight => true;
        public static bool Default_TextOutline => true;

        public static int Default_TextOffsetX => 5;
        public static int Default_TextOffsetY => 5;
        public static float Default_TextScale => 4f;

        public static string Default_TextColorHEX => "#FFFFFF";
        public static Color Default_TextColor => Color.white;
        public static string Default_TextOutlineColorHEX => "#000000";
        public static Color Default_TextOutlineColor => Color.black;

        public static float Default_TextOutlineThickness => 0.5f;
    }
}
