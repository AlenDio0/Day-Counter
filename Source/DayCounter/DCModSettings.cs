using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace DayCounter
{
    public class DCModSettings : ModSettings
    {
        public bool Enabled;
        public bool OriginUpRight;
        public bool Outline;
        public bool Animation;
        public bool DebugBox;

        public string CustomText;

        public Vector2 Offset;
        public Vector2 Scale;

        public bool LockScale;

        public Color FillColor;
        public Color OutlineColor;
        public float OutlineFactor;

        public void DoWindowContents(Rect canva)
        {
            Rect checkboxPart = canva.BottomPart(1f).TopPart(0.2f).BottomPart(0.9f);
            Rect entryPart = canva.BottomPart(1f).TopPart(0.2f).BottomHalf().RightHalf();
            Rect sliderPart = canva.BottomPart(0.8f).TopPart(0.5f).BottomPart(0.9f);
            Rect colorPart = canva.BottomPart(0.3f).TopPart(0.4f).BottomPart(0.9f);

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

            listing.Gap();
            listing.CheckboxLabeled(DCData.Label_DebugBox, ref DebugBox);

            listing.NewColumn();
            listing.CheckboxLabeled(DCData.Label_TextOriginUpRight, ref OriginUpRight);

            listing.NewColumn();
            listing.CheckboxLabeled(DCData.Label_TextOutline, ref Outline);

            listing.NewColumn();
            listing.CheckboxLabeled(DCData.Label_TextAnimation, ref Animation);

            listing.End();
        }

        private void ShowTextEntry(Rect part)
        {
            Rect labelRect = part.LeftPart(0.2f);
            Rect entryRect = part.RightPart(0.7f).LeftPart(0.3f).TopHalf();

            Widgets.Label(labelRect, DCData.Label_Text);
            CustomText = Widgets.TextField(entryRect, CustomText);
        }

        private void ShowSliders(Rect part)
        {
            Listing_Standard listing = new Listing_Standard
            {
                ColumnWidth = part.width / 1.1f,
            };

            listing.Begin(part);

            Offset.x = (int)listing.SliderLabeled($"{DCData.Label_TextOffsetX} ({Offset.x} px)",
                Offset.x, -UI.screenWidth, UI.screenWidth);

            listing.Gap();
            Offset.y = (int)listing.SliderLabeled($"{DCData.Label_TextOffsetY} ({Offset.y} px)",
                Offset.y, -UI.screenHeight, UI.screenHeight);

            float DrawScaleSliderAndLock(string label, float value)
            {
                const float iconSize = 24f;
                Texture2D lockImage = LockScale ? TexButton.Plus : TexButton.Minus;

                Rect currentRect = listing.GetRect(0f);
                label += $" ({value.ToStringPercent()})";

                value = listing.SliderLabeled(label, value, 0.1f, 50f);
                if (Widgets.ButtonImage(new Rect(currentRect.xMax + 20f, currentRect.y, iconSize, iconSize), lockImage))
                {
                    LockScale = !LockScale;
                    SoundDefOf.Click.PlayOneShotOnCamera();

                    if (LockScale)
                        Scale = new Vector2(value, value);
                }

                return value;
            }

            listing.Gap();
            if (!LockScale)
            {
                Scale.x = DrawScaleSliderAndLock(DCData.Label_TextScaleX, Scale.x);
                Scale.y = DrawScaleSliderAndLock(DCData.Label_TextScaleY, Scale.y);
            }
            else
            {
                Scale.x = DrawScaleSliderAndLock(DCData.Label_TextScale, Scale.x);
                Scale.y = Scale.x;
            }

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
            Scribe_Values.Look(ref Animation, "Animation", true);
            Scribe_Values.Look(ref DebugBox, "DebugBox", false);

            Scribe_Values.Look(ref CustomText, "Text", "DAY");

            Scribe_Values.Look(ref Offset, "TextOffset", new Vector2(0f, -10f));
            Scribe_Values.Look(ref Scale, "TextScale", new Vector2(5f, 5f));

            Scribe_Values.Look(ref LockScale, "LockScale", false);

            Scribe_Values.Look(ref FillColor, "TextColor", Color.white);
            Scribe_Values.Look(ref OutlineColor, "TextOutlineColor", Color.black);
            Scribe_Values.Look(ref OutlineFactor, "TextOutlineThickness", 0.5f);

            base.ExposeData();
        }
    }
}
