using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProfielWerkstuk.Scripts.GUI.BaseClasses
{
	public class TextMenuElement : BaseMenuElement
	{
		public string Text;
		public SpriteFont Font;
		public TextAlignment Alignment = TextAlignment.Center;

		public TextMenuElement(MenuContainer parentContainer, Vector2 offset, string text, SpriteFont font) : base(parentContainer)
		{
			Text = text;
			Font = font;
			Offset = offset;
		}

		protected override Vector2 GetMinimalSize()
		{
			return Font.MeasureString(Text) + 2 * Padding;
		}

		public override Vector2 GetTopLeft()
		{
			if (Alignment == TextAlignment.Left)
			{
				return Offset - new Vector2(0, Size.Y / 2);
			}

			return Offset - Size / 2;
		}
		public override Vector2 GetLowerRight()
		{
			if (Alignment == TextAlignment.Left)
			{
				return Offset + new Vector2(Size.X, Size.Y / 2);
			}

			return Offset + Size / 2;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Color text = GetTextColor();

			Vector2 textVector2;
			if (Alignment == TextAlignment.Center)
				textVector2 = GetPosition() - Font.MeasureString(Text) / 2;
			else
				textVector2 = GetPosition() - new Vector2(0, Font.MeasureString(Text).Y / 2);

			//This prevents some nasty anti-aliasing making the letters clearer and less smudged
			textVector2.X = (int)textVector2.X;
			textVector2.Y = (int)textVector2.Y;

			spriteBatch.DrawString(Font, Text, textVector2, text);
		}

		public virtual Color GetTextColor()
		{
			return Color.White;
		}
	}

	public enum TextAlignment
	{
		Center, Left
	}
}
