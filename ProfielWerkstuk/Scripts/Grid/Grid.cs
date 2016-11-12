using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;

namespace ProfielWerkstuk.Scripts.Grid
{
	public class Grid
	{
		public Game Game;
		public int GridSize;
		public int HalfWidth;
		public int HalfHeight;
		public float LineWidth;
		public Texture2D Texture;
		public RectangleF GridBounds;

		public Grid(Game game, int gridSize, int halfWidth, int halfHeight, int lineWidth)
		{
			Game = game;
			GridSize = gridSize;
			HalfWidth = halfWidth;
			HalfHeight = halfHeight;
			LineWidth = lineWidth;

			float stepRate = GridSize + LineWidth / 2 + 1;
			GridBounds = new RectangleF(-halfWidth * stepRate, -halfHeight * stepRate,
				2*halfWidth * stepRate, 2*halfHeight * stepRate);
		}

		public void DrawGridSquares(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, new Vector2(1, 1));
		}

		public void DrawGridLines(SpriteBatch spriteBatch)
		{
			//Distance between each line
			float stepRate = GridSize + LineWidth / 2 + 1;

			//Draw vertical lines
			for(int y = -HalfHeight; y <= HalfHeight; y++)
			{
				float pixelY = stepRate * y;
				spriteBatch.DrawLine(new Vector2(-HalfWidth*stepRate - (LineWidth/2), pixelY), 
					new Vector2(HalfWidth*stepRate + (LineWidth / 2), pixelY), Color.LightGray, LineWidth);
			}

			//Draw horizontal lines
			for (int x = -HalfWidth; x <= HalfWidth; x++)
			{
				float pixelX = stepRate*x;
				spriteBatch.DrawLine(new Vector2(pixelX, -HalfHeight*stepRate - (LineWidth/2f)),
					new Vector2(pixelX, HalfHeight*stepRate + (LineWidth/2)), Color.LightGray, LineWidth);
			}
		}

		public void GenerateTextures()
		{
			int width = GridSize;
			int height = GridSize;

			Texture = new Texture2D(Game.GraphicsDevice, width, height);
			Color[] colorData = new Color[width * height];
			for (var i = 0; i < width * height; i++)
				colorData[i] = Color.Gray;

			Texture.SetData(colorData);
		}

		public void Dispose()
		{
			Texture.Dispose();
		}
	}
}
