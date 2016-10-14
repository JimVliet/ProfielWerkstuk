using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfielWerkstuk.Scripts
{
	public class Grid
	{
		Game game;
		int gridSize;
		int halfWidth;
		int halfHeight;
		float lineWidth;
		Texture2D texture;

		public Grid(Game game, int gridSize, int halfWidth, int halfHeight, int lineWidth)
		{
			this.game = game;
			this.gridSize = gridSize;
			this.halfWidth = halfWidth;
			this.halfHeight = halfHeight;
			this.lineWidth = lineWidth;
		}

		public void DrawGridSquares(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, new Vector2(1, 1));
		}

		public void DrawGridLines(SpriteBatch spriteBatch)
		{
			//Distance between each line
			float stepRate = gridSize + lineWidth / 2;

			//Draw vertical lines
			float pixelY;
			for(int y = -halfHeight; y <= halfHeight; y++)
			{
				pixelY = stepRate * y;
				MonoGame.Extended.Shapes.SpriteBatchExtensions.DrawLine(spriteBatch, new Vector2(-halfWidth*stepRate - (lineWidth/2), pixelY), 
					new Vector2((float)halfWidth*stepRate + (lineWidth / 2), pixelY), Color.LightGray, lineWidth);
			}

			//Draw horizontal lines
			float pixelX;
			for(int x = -halfWidth; x <= halfWidth; x++)
			{
				pixelX = stepRate * x;
				MonoGame.Extended.Shapes.SpriteBatchExtensions.DrawLine(spriteBatch, new Vector2(pixelX, -halfHeight * stepRate - (lineWidth / 2f)),
					new Vector2(pixelX, halfHeight * stepRate + (lineWidth / 2)), Color.LightGray, lineWidth);
			}
		}

		public void generateTextures()
		{
			int width = 63;
			int height = 63;

			texture = new Texture2D(game.GraphicsDevice, width, height);
			Color[] colorData = new Color[width * height];
			for (var i = 0; i < width * height; i++)
				colorData[i] = Color.Gray;

			texture.SetData<Color>(colorData);
		}

		public void Dispose()
		{
			texture.Dispose();
		}
	}
}
