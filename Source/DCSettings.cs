using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace DayCounter
{
    public class DCSettings : ModSettings
    {
        public bool Enabled = DCData.Default_Enabled;
        public bool OriginUpRight = DCData.Default_OriginUpRight;
        public bool TextOutline = DCData.Default_TextOutline;

        private string m_BufferOffsetX;
        public int TextOffsetX = DCData.Default_TextOffsetX;

        private string m_BufferOffsetY;
        public int TextOffsetY = DCData.Default_TextOffsetY;

        private string m_BufferScale;
        public float TextScale = DCData.Default_TextScale;

        private string m_HexTextColor = DCData.Default_TextColorHEX;
        public Color TextColor = DCData.Default_TextColor;

        private string m_HexTextOutlineColor = DCData.Default_TextOutlineColorHEX;
        public Color TextOutlineColor = DCData.Default_TextOutlineColor;

        public float TextOutlineThickness;

        public void DoWindowContents(Rect canva)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(canva);

            float rowHeight = Text.LineHeight + 4f;

            // Checkbox
            Rect createCheckbox(Rect row) => new Rect(row.x, row.y, 300f, rowHeight);
            // Enabled
            Widgets.CheckboxLabeled(createCheckbox(listing.GetRect(rowHeight)), DCData.Label_TextEnabled, ref Enabled);

            // Origin up-right
            Widgets.CheckboxLabeled(createCheckbox(listing.GetRect(rowHeight)), DCData.Label_TextOriginUpRight, ref OriginUpRight);

            // Outline
            Widgets.CheckboxLabeled(createCheckbox(listing.GetRect(rowHeight)), DCData.Label_TextOutline, ref TextOutline);

            // Text Field
            Rect createTextField(Rect row, float x) => new Rect(row.x + x, row.y, 180f, rowHeight);
            float spacingRow = 40f;
            // Offset & Scale
            listing.Gap(24f);
            Rect currentRow = listing.GetRect(rowHeight);

            Rect offsetXRect = createTextField(currentRow, 0f);
            Widgets.TextFieldNumericLabeled(offsetXRect, $"{DCData.Label_TextOffsetX}  ", ref TextOffsetX, ref m_BufferOffsetX, -2000f);

            Rect offsetYRect = createTextField(currentRow, offsetXRect.xMax + spacingRow);
            Widgets.TextFieldNumericLabeled(offsetYRect, $"{DCData.Label_TextOffsetY}  ", ref TextOffsetY, ref m_BufferOffsetY, -2000f);

            Rect scaleRect = createTextField(currentRow, offsetYRect.xMax + spacingRow);
            Widgets.TextFieldNumericLabeled(scaleRect, $"{DCData.Label_TextScale}    ", ref TextScale, ref m_BufferScale, 0.1f, 50f);

            // Color
            listing.Gap(24f);
            currentRow = listing.GetRect(rowHeight);

            float labelWidth = 325f;
            float textWidth = 100f;
            float spacing = 15f;

            Widgets.Label(new Rect(currentRow.x, currentRow.y, labelWidth, rowHeight), DCData.Label_TextColor);
            Rect colorRect = new Rect(currentRow.x + labelWidth + spacing, currentRow.y, textWidth, rowHeight);
            m_HexTextColor = Widgets.TextField(colorRect, m_HexTextColor);
            TextColor = HexToColor(ref m_HexTextColor, DCData.Default_TextColor);

            Rect colorPreviewRect = new Rect(colorRect.xMax + spacing, currentRow.y + 2f, rowHeight, rowHeight);
            Widgets.DrawBoxSolid(colorPreviewRect, TextColor);

            // Outline Color
            listing.Gap();
            currentRow = listing.GetRect(rowHeight);

            Widgets.Label(new Rect(currentRow.x, currentRow.y, labelWidth, rowHeight), DCData.Label_TextOutlineColor);
            Rect outlineColorRect = new Rect(currentRow.x + labelWidth + spacing, currentRow.y, textWidth, rowHeight);
            m_HexTextOutlineColor = Widgets.TextField(outlineColorRect, m_HexTextOutlineColor);
            TextOutlineColor = HexToColor(ref m_HexTextOutlineColor, DCData.Default_TextOutlineColor);

            Rect outlineColorPreviewRect = new Rect(outlineColorRect.xMax + spacing, currentRow.y + 2f, rowHeight, rowHeight);
            Widgets.DrawBoxSolid(outlineColorPreviewRect, TextOutlineColor);

            // Outline Thickness
            listing.Gap(48f);
            currentRow = listing.GetRect(rowHeight);

            Rect outlineThicknessRect = new Rect(currentRow.x, currentRow.y, labelWidth + textWidth, rowHeight * 3);
            Widgets.HorizontalSlider(outlineThicknessRect, ref TextOutlineThickness,
                    new FloatRange(0.1f, 1f), $"{DCData.Label_TextOutlineThickness}\n{TextOutlineThickness}", 0.1f);

            listing.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref OriginUpRight, "OriginUpRight", DCData.Default_OriginUpRight);
            Scribe_Values.Look(ref TextOutline, "TextOutline", DCData.Default_TextOutline);

            Scribe_Values.Look(ref TextOffsetX, "TextOffsetX", DCData.Default_TextOffsetX);
            Scribe_Values.Look(ref TextOffsetY, "TextOffsetY", DCData.Default_TextOffsetY);
            Scribe_Values.Look(ref TextScale, "TextScale", DCData.Default_TextScale);

            Scribe_Values.Look(ref m_HexTextColor, "TextColorHEX", DCData.Default_TextColorHEX);
            Scribe_Values.Look(ref TextColor, "TextColor", DCData.Default_TextColor);
            Scribe_Values.Look(ref m_HexTextOutlineColor, "TextOutlineColorHEX", DCData.Default_TextOutlineColorHEX);
            Scribe_Values.Look(ref TextOutlineColor, "TextOutlineColor", DCData.Default_TextOutlineColor);

            Scribe_Values.Look(ref TextOutlineThickness, "TextOutlineThickness", DCData.Default_TextOutlineThickness);

            base.ExposeData();
        }

        private static Color HexToColor(ref string hex, Color fallback)
        {
            if (!hex.StartsWith("#"))
                hex = "#" + hex;

            hex = hex.ToUpperInvariant();
            if (hex.Length > 7)
                hex = hex.Substring(0, 7);

            if (hex.Length == 7 && Regex.IsMatch(hex, "^#[0-9A-F]{6}$"))
            {
                ColorUtility.TryParseHtmlString(hex, out Color parsed);
                return parsed;
            }

            return fallback;
        }
    }
}
