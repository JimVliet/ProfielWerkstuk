using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProfielWerkstuk.Scripts.GUI
{
	public class Button
	{
		public Vector2 ButtonSize;
		private readonly SpriteFont _font;
		public Color ButtonColor = new Color(29, 114, 238);
		public Color TextColor;
		public Color ButtonHoverColor = new Color(51, 134, 255);

		//Make sure LineBorderEnabled is set to true, or it won't have any effect
		public Color LineColor;
		public Vector2 Padding = new Vector2(20f, 20f);
		public string Text;
		public bool LineBorderEnabled;
		public bool IsBeingHovered;
		public ButtonClick ButtonClickedEvent;

		public int LineWidth
		{
			set
			{
				if (value >= 1)
					LineWidth = value;
			}
		}

		public Button(SpriteFont font, Vector2 size, string text)
		{
			_font = font;
			ButtonSize = size;
			TextColor = Color.White;
			Text = text;
		}

		public Button(SpriteFont font, Vector2 size, string text, Color buttonColor)
		{
			_font = font;
			ButtonSize = size;
			ButtonColor = buttonColor;
			TextColor = Color.White;
			Text = text;
		}

		public Button(SpriteFont font, Vector2 size, string text, Color buttonColor, Color textColor)
		{
			_font = font;
			ButtonSize = size;
			ButtonColor = buttonColor;
			TextColor = textColor;
			Text = text;
		}

		public Button(SpriteFont font, Vector2 size, string text, Color buttonColor, Color textColor, Color lineColor)
		{
			_font = font;
			ButtonSize = size;
			ButtonColor = buttonColor;
			TextColor = textColor;
			LineColor = lineColor;
			Text = text;

			LineBorderEnabled = true;
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 position)
		{
			Color drawColor = IsBeingHovered ? ButtonHoverColor : ButtonColor;
			Vector2 buttonSize = GetSize();
			Vector2 drawPosition = position - (buttonSize / 2);

			MonoGame.Extended.Shapes.SpriteBatchExtensions.FillRectangle(spriteBatch, drawPosition, buttonSize, drawColor);
			if(LineBorderEnabled)
				MonoGame.Extended.Shapes.SpriteBatchExtensions.DrawRectangle(spriteBatch, drawPosition, buttonSize, LineColor, 4);

			Vector2 textVector2 = position - (_font.MeasureString(Text)/2);
			//This prevents some nasty anti-aliasing making the letters clearer and less smudged
			textVector2.X = (int)textVector2.X;
			textVector2.Y = (int)textVector2.Y;

			spriteBatch.DrawString(_font, Text, textVector2, TextColor);
		}

		/// <summary>
		/// This method gives the minimal button size
		/// </summary>
		/// <returns>Returns the minimal size of the button in order to prevent text outside the Button.</returns>
		public Vector2 GetMinimalSize()
		{
			return _font.MeasureString(Text) + (2 * Padding);
		}

		public Vector2 GetSize()
		{
			Vector2 buttonMinimalSize = GetMinimalSize();
			return new Vector2(Math.Max(buttonMinimalSize.X, ButtonSize.X),
				Math.Max(buttonMinimalSize.Y, ButtonSize.Y));
		}

		public Vector2 GetTopLeft(Vector2 pos)
		{
			return pos - (GetSize() / 2);
		}

		public Vector2 GetLowerRight(Vector2 pos)
		{
			return pos + (GetSize() / 2);
		}
	}

	public delegate void ButtonClick(Button button, Vector2 clickLocation);
}
