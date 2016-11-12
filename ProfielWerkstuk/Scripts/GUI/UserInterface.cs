using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ProfielWerkstuk.Scripts.GUI
{
	public class UserInterface
	{
		public static void DrawButton(SpriteBatch spriteBatch, string text, float fontSize, Vector2 position, SpriteFont font, Vector2 padding)
		{
			//Measure the length in pixels of the string
			Vector2 length = font.MeasureString(text);
			Vector2 buttonSize = length + (2 * padding) - new Vector2(0, fontSize / 3.2f);

			MonoGame.Extended.Shapes.SpriteBatchExtensions.FillRectangle(spriteBatch, position - padding, buttonSize, new Color(147, 188, 255));
			MonoGame.Extended.Shapes.SpriteBatchExtensions.DrawRectangle(spriteBatch, position - padding, buttonSize, new Color(86, 148, 255), 4);
			spriteBatch.DrawString(font, text, position, new Color(80, 97, 127));
		}

		public static void DrawButton(SpriteBatch spriteBatch, string text, float fontSize, Vector2 position, SpriteFont font)
		{
			//Basic padding
			Vector2 padding = new Vector2(5, 5);

			DrawButton(spriteBatch, text, fontSize, position, font, padding);
		}
	}
}
