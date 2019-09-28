using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace Pacman.GameStates
{
    public abstract class BaseGameState
    {
        public BaseGameState()
        {
            LoadContent();
        }
        protected static void DrawText(RenderTarget target, Font font, string value, float x, float y, Color color, int size, bool bold, bool textshadow)
        {
            var text = new Text()
            {
                DisplayedString = value,
                Font = font,
                CharacterSize = (uint)size,
                FillColor = color,
                Style = bold ? Text.Styles.Bold : Text.Styles.Regular,
                OutlineThickness = 1,
                OutlineColor = new Color(0, 0, 0, 189),
            };

            text.Position = new Vector2f((x - (text.GetLocalBounds().Width / 2f)), y);
            if (textshadow)
            {
                var rectangle = new RectangleShape(new Vector2f((text.GetLocalBounds().Width + 40f), (text.GetLocalBounds().Height + 40f)))
                {
                    Position = new Vector2f((text.Position.X - 20f), (text.Position.Y - 20f)),
                    FillColor = new Color(100, 100, 100, 50)
                };

                target.Draw(rectangle);
            }

            target.Draw(text);
        }
        protected static void PlaySound(Sound sound)
        {
            sound.Play();
        }
        protected abstract void LoadContent();
    }
}
