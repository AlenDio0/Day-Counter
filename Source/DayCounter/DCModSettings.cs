using UnityEngine;
using Verse;

namespace DayCounter
{
    public class DCModSettings : ModSettings
    {
        public bool Enabled;
        public bool OriginUpRight;
        public bool Outline;
        public bool DebugBox;

        public string CustomText;

        public int OffsetX;
        public int OffsetY;
        public float Scale;

        public Color FillColor;
        public Color OutlineColor;
        public float OutlineFactor;

        public void DoWindowContents(Rect canva)
        {
            Rect checkboxPart = canva.BottomPart(1f).TopPart(0.1f).BottomPart(0.9f);
            Rect entryPart = canva.BottomPart(0.9f).TopPart(0.1f).BottomPart(0.9f);
            Rect sliderPart = canva.BottomPart(0.8f).TopPart(0.4f).BottomPart(0.9f);
            Rect colorPart = canva.BottomPart(0.4f).TopPart(0.4f).BottomPart(0.9f);

            ShowCheckboxes(checkboxPart);
            ShowTextEntry(entryPart);
            ShowSliders(sliderPart);
            ShowColorSliders(colorPart);
        }

        private void ShowCheckboxes(Rect part)
        {
            Listing_Standard listing = new Listing_Standard
            {
                ColumnWidth = part.width / 5f,
            };

            listing.Begin(part);

            listing.CheckboxLabeled(DCData.Label_TextEnabled, ref Enabled);

            listing.NewColumn();
            listing.CheckboxLabeled(DCData.Label_TextOriginUpRight, ref OriginUpRight);

            listing.NewColumn();
            listing.CheckboxLabeled(DCData.Label_TextOutline, ref Outline);

            listing.NewColumn();
            listing.CheckboxLabeled(DCData.Label_DebugBox, ref DebugBox);

            listing.End();
        }

        private void ShowTextEntry(Rect part)
        {
            Rect labelRect = part.LeftPart(0.15f);
            Rect entryRect = part.RightPart(0.8f).LeftPart(0.3f).TopHalf();

            Widgets.Label(labelRect, DCData.Label_Text);
            CustomText = Widgets.TextField(entryRect, CustomText);
        }

        private void ShowSliders(Rect part)
        {
            Listing_Standard listing = new Listing_Standard
            {
                ColumnWidth = part.width / 1.125f,
            };

            listing.Begin(part);

            OffsetX = (int)listing.SliderLabeled($"{DCData.Label_TextOffsetX} ({OffsetX} px)",
                OffsetX, -UI.screenWidth, UI.screenWidth);

            listing.Gap();
            OffsetY = (int)listing.SliderLabeled($"{DCData.Label_TextOffsetY} ({OffsetY} px)",
                OffsetY, -UI.screenHeight, UI.screenHeight);

            listing.Gap();
            Scale = listing.SliderLabeled($"{DCData.Label_TextScale} ({Scale.ToStringPercent()})", Scale, 0.1f, 50f);

            listing.Gap();
            OutlineFactor = listing.SliderLabeled($"{DCData.Label_TextOutlineFactor} ({OutlineFactor.ToStringPercent()})",
                OutlineFactor, 0.1f, 1f);

            listing.End();
        }

        private void ShowColorSliders(Rect part)
        {
            Rect topPart = part.TopHalf();
            Rect bottomPart = part.BottomHalf();

            Listing_Standard listing = new Listing_Standard
            {
                ColumnWidth = part.width / 5f,
            };

            listing.Begin(topPart);

            Text.Font = GameFont.Medium;
            listing.Label(DCData.Label_TextFillColor);
            Text.Font = GameFont.Small;

            ShowRGBSlider(listing, ref FillColor);

            listing.End();

            listing.Begin(bottomPart);

            Text.Font = GameFont.Medium;
            listing.Label(DCData.Label_TextOutlineColor);
            Text.Font = GameFont.Small;

            ShowRGBSlider(listing, ref OutlineColor);

            listing.End();
        }

        private static void ShowRGBSlider(Listing_Standard listing, ref Color color)
        {
            float red = color.r, green = color.g, blue = color.b;

            listing.NewColumn();
            red = listing.SliderLabeled($"{DCData.Label_Red} ({(int)(red * 255)})", red, 0f, 1f);
            listing.NewColumn();
            green = listing.SliderLabeled($"{DCData.Label_Green} ({(int)(green * 255)})", green, 0f, 1f);
            listing.NewColumn();
            blue = listing.SliderLabeled($"{DCData.Label_Blue} ({(int)(blue * 255)})", blue, 0f, 1f);

            color = new Color(red, green, blue);

            Widgets.DrawBoxSolid(listing.GetRect(50f).LeftHalf().ContractedBy(10f), color);
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Enabled, "Enabled", true);
            Scribe_Values.Look(ref OriginUpRight, "OriginUpRight", true);
            Scribe_Values.Look(ref Outline, "TextOutline", false);
            Scribe_Values.Look(ref DebugBox, "DebugBox", false);

            Scribe_Values.Look(ref CustomText, "Text", "DAY");

            Scribe_Values.Look(ref OffsetX, "TextOffsetX", 0);
            Scribe_Values.Look(ref OffsetY, "TextOffsetY", -10);
            Scribe_Values.Look(ref Scale, "TextScale", 3f);

            Scribe_Values.Look(ref FillColor, "TextColor", Color.white);
            Scribe_Values.Look(ref OutlineColor, "TextOutlineColor", Color.black);
            Scribe_Values.Look(ref OutlineFactor, "TextOutlineThickness", 0.5f);

            base.ExposeData();
        }
    }
}
