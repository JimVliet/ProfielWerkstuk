using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProfielWerkstuk.Scripts.GUI.BaseClasses
{
	public class ButtonMenuElement : BaseMenuElement
	{
		public SpriteFont Font;
		public Color ButtonColor = new Color(29, 114, 238);
		public Color TextColor = Color.White;
		public Color ButtonHoverColor = new Color(51, 134, 255);

		public string Text;
		public bool IsBeingHovered;

		public ButtonMenuElement(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer)
		{
			Text = text;
			Font = font;
			Padding = new Vector2(20, 20);
			MatchToContainer = true;
		}

		protected override Vector2 GetMinimalSize()
		{
			return Font.MeasureString(Text) + 2 * Padding;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 elementPosition = GetPosition();

			Color drawColor = IsBeingHovered ? ButtonHoverColor : ButtonColor;
			Vector2 drawPosition = elementPosition - Size/2;

			MonoGame.Extended.Shapes.SpriteBatchExtensions.FillRectangle(spriteBatch, drawPosition, Size, drawColor);

			Vector2 textVector2 = elementPosition - Font.MeasureString(Text) / 2;
			//This prevents some nasty anti-aliasing making the letters clearer and less smudged
			textVector2.X = (int)textVector2.X;
			textVector2.Y = (int)textVector2.Y;

			spriteBatch.DrawString(Font, Text, textVector2, TextColor);
		}

		public override void Hover()
		{
			IsBeingHovered = true;
		}

		public override void UnHover()
		{
			IsBeingHovered = false;
		}
	}
}
