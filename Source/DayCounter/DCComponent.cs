using RimWorld;
using UnityEngine;
using Verse;

namespace DayCounter
{
    public class DCComponent : GameComponent
    {
        private int m_CurrentDayDisplay = -1;
        private int m_PreviousDay = -1;

        private const float AnimationDuration = 1f;
        private float m_AnimationTime = AnimationDuration;

        private const float DelayDuration = 2f;
        private float m_DelayTime = DelayDuration;

        private Vector2 m_CachedCustomSize;
        private Vector2 m_CachedDaySize;

        public DCComponent(Game game)
            : base() { }

        private void UpdateCachedSize(DCModSettings settings)
        {
            m_CachedCustomSize = Text.CalcSize($"{settings.CustomText} ");
            m_CachedDaySize = Text.CalcSize(new string('8', m_CurrentDayDisplay.ToString().Length));
        }

        private void UpdateLogic(DCModSettings settings)
        {
            int currentDay = GenDate.DaysPassed;

            if (m_CurrentDayDisplay == -1)
            {
                m_CurrentDayDisplay = currentDay;
                m_PreviousDay = currentDay;
                UpdateCachedSize(settings);
                return;
            }

            if (currentDay != m_CurrentDayDisplay && m_DelayTime >= DelayDuration && m_AnimationTime >= AnimationDuration)
            {
                if (settings.Animation)
                {
                    m_DelayTime = 0f;
                }
                else
                {
                    m_CurrentDayDisplay = currentDay;
                    m_PreviousDay = currentDay;
                }
            }

            if (m_DelayTime < DelayDuration)
            {
                m_DelayTime += Time.deltaTime;
                if (m_DelayTime >= DelayDuration)
                {
                    m_PreviousDay = m_CurrentDayDisplay;
                    m_CurrentDayDisplay = currentDay;
                    UpdateCachedSize(settings);

                    m_AnimationTime = 0f;
                }

                return;
            }

            if (m_AnimationTime < AnimationDuration)
                m_AnimationTime += Time.deltaTime;
        }

        private void DrawCounterAnimation(DCModSettings settings, float padding, Vector2 size)
        {
            float progress = Mathf.SmoothStep(0f, 1f, m_AnimationTime / AnimationDuration);
            float shift = progress * size.y;

            Rect lastDayRect = new Rect(new Vector2(padding, padding - shift), m_CachedDaySize);
            DrawText(lastDayRect, m_PreviousDay.ToString(), settings);

            Rect currentDayRect = new Rect(new Vector2(padding, padding + m_CachedDaySize.y - shift), m_CachedDaySize);
            DrawText(currentDayRect, m_CurrentDayDisplay.ToString(), settings);
        }

        public override void GameComponentOnGUI()
        {
            var settings = DCMod.s_Settings;
            if (!settings.Enabled)
                return;

            UpdateLogic(settings);

            Color defaultColor = GUI.color;
            Matrix4x4 defaultMatrix = GUI.matrix;

            Vector2 textSize = new Vector2(m_CachedCustomSize.x + m_CachedDaySize.x, Mathf.Max(m_CachedCustomSize.y, m_CachedDaySize.y));
            GUI.matrix = CalculateMatrix(settings.Offset, textSize.x, settings.Scale, settings.OriginUpRight);

            if (settings.DebugBox)
                Widgets.DrawBoxSolid(new Rect(Vector2.zero, textSize), Color.red * 0.5f);

            DrawText(new Rect(Vector2.zero, m_CachedCustomSize), $"{settings.CustomText} ", settings);

            float padding = settings.OutlineFactor + 1f;
            float fullPadding = padding * 2f;
            Rect dayRect = new Rect(m_CachedCustomSize.x - padding, -padding, m_CachedDaySize.x + fullPadding, textSize.y + fullPadding);
            GUI.BeginGroup(dayRect);
            if (!settings.Animation || m_AnimationTime >= AnimationDuration)
                DrawText(new Rect(padding, padding, m_CachedDaySize.x, textSize.y), m_CurrentDayDisplay.ToString(), settings);
            else
                DrawCounterAnimation(settings, padding, textSize);
            GUI.EndGroup();

            GUI.color = defaultColor;
            GUI.matrix = defaultMatrix;

            base.GameComponentOnGUI();
        }

        private Matrix4x4 CalculateMatrix(Vector2 offset, float sizeX, Vector2 scale, bool originRight)
        {
            float positionX = originRight ? UI.screenWidth - (sizeX * scale.x) - offset.x : offset.x;

            return Matrix4x4.TRS(new Vector3(positionX, offset.y, 0f), Quaternion.identity, new Vector3(scale.x, scale.y, 1f));
        }

        private void DrawText(Rect rect, string text, DCModSettings settings)
        {
            if (settings.Outline)
            {
                float outline = settings.OutlineFactor;
                GUI.color = settings.OutlineColor;

                const float step = 0.1f;
                for (float x = -outline; x <= outline; x += step)
                {
                    for (float y = -outline; y <= outline; y += step)
                    {
                        Vector2 offset = new Vector2(x, y);
                        GUI.Label(new Rect(rect.position + offset, rect.size), text);
                    }
                }
            }

            GUI.color = settings.FillColor;
            GUI.Label(rect, text);
        }
    }
}
