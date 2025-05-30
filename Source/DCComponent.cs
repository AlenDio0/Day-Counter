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
            if (!DayCounterMod.s_Settings.Enabled)
            {
                return;
            }

            Text.Font = GameFont.Small;
            Color defaultColor = GUI.color;
            Matrix4x4 defaultMatrix = GUI.matrix;

            string text = $"{DCData.TextDay} {GenDate.DaysPassed}";
            Vector2 textSize = Text.CalcSize(text);

            float scale = DayCounterMod.s_Settings.TextScale;

            float x = DayCounterMod.s_Settings.TextOffsetX;
            GUI.matrix = Matrix4x4.TRS(new Vector3(
                DayCounterMod.s_Settings.OriginUpRight ?
                UI.screenWidth - (textSize.x * scale) - x : x,
                DayCounterMod.s_Settings.TextOffsetY, 0f),
                Quaternion.identity, new Vector3(scale, scale, 1f));

            if (DayCounterMod.s_Settings.TextOutline)
            {
                float thickness = DayCounterMod.s_Settings.TextOutlineThickness;
                GUI.color = DayCounterMod.s_Settings.TextOutlineColor;
                Widgets.Label(new Rect(thickness, 0f, textSize.x, textSize.y), text);
                Widgets.Label(new Rect(-thickness, 0f, textSize.x, textSize.y), text);
                Widgets.Label(new Rect(0f, thickness, textSize.x, textSize.y), text);
                Widgets.Label(new Rect(0f, -thickness, textSize.x, textSize.y), text);

                //Widgets.Label(new Rect(thickness, thickness, textSize.x, textSize.y), text);
                //Widgets.Label(new Rect(-thickness, thickness, textSize.x, textSize.y), text);
                //Widgets.Label(new Rect(thickness, -thickness, textSize.x, textSize.y), text);
                //Widgets.Label(new Rect(-thickness, -thickness, textSize.x, textSize.y), text);
            }

            GUI.color = DayCounterMod.s_Settings.TextColor;
            Widgets.Label(new Rect(0f, 0f, textSize.x, textSize.y), text);

            GUI.color = defaultColor;
            GUI.matrix = defaultMatrix;

            base.GameComponentOnGUI();
        }
    }
}
