using RimWorld;
using UnityEngine;
using Verse;

namespace DayCounter
{
    public class DCComponent : GameComponent
    {
        public DCComponent(Game game)
            : base() { }

        public override void GameComponentOnGUI()
        {
            var settings = DCMod.s_Settings;

            if (!settings.Enabled)
                return;

            string text = $"{settings.CustomText} {GenDate.DaysPassed}";
            Vector2 textSize = Text.CalcSize(text);

            Color defaultColor = GUI.color;
            Matrix4x4 defaultMatrix = GUI.matrix;

            float scale = settings.Scale;
            float offsetX = settings.OffsetX;

            GUI.matrix = Matrix4x4.TRS(new Vector3(
                settings.OriginUpRight ?
                UI.screenWidth - (textSize.x * scale) - offsetX : offsetX,
                settings.OffsetY, 0f),
                Quaternion.identity, new Vector3(scale, scale, 1f));

            if (settings.Outline)
            {
                float outline = settings.OutlineFactor;
                GUI.color = settings.OutlineColor;

                for (float x = -outline; x <= outline; x += 0.1f)
                {
                    for (float y = -outline; y <= outline; y += 0.1f)
                    {
                        if (x == 0f && y == 0f)
                            continue;

                        Rect outlineRect = new Rect(x, y, textSize.x, textSize.y);
                        GUI.Label(outlineRect, text);
                    }
                }
            }

            if (settings.DebugBox)
                Widgets.DrawBoxSolid(new Rect(0f, 0f, textSize.x, textSize.y), Color.red * 0.5f);

            GUI.color = settings.FillColor;
            GUI.Label(new Rect(0f, 0f, textSize.x, textSize.y), text);

            GUI.color = defaultColor;
            GUI.matrix = defaultMatrix;

            base.GameComponentOnGUI();
        }
    }
}
