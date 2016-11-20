using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;

namespace ProfielWerkstuk.Scripts.Grid
{
	public class Grid
	{
		public Game1 Game;
		public int GridSize;
		public int HalfWidth;
		public int HalfHeight;
		public float LineWidth;
		public RectangleF GridBounds;
		public GridElement[,] GridElements;

		public Grid(Game1 game, int gridSize, int halfWidth, int halfHeight, int lineWidth)
		{
			Game = game;
			GridSize = gridSize;
			HalfWidth = halfWidth;
			HalfHeight = halfHeight;
			LineWidth = lineWidth;

			GridElements = new GridElement[2 * halfHeight, 2 * halfWidth];

			for (int index = 0; index < GridElements.Length; index++)
			{
				int x = index%(2*halfWidth);
				int y = index/(2*halfWidth);
				GridElements[y, x] = new GridElement(x, y);
			}

			float stepRate = GetStepRate();
			GridBounds = new RectangleF(-halfWidth * stepRate, -halfHeight * stepRate,
				2*halfWidth * stepRate, 2*halfHeight * stepRate);
		}

		public void GenerateGrid()
		{
			
		}

		public void DrawGridSquares(SpriteBatch spriteBatch)
		{
			float stepRate = GetStepRate();

			for (int index = 0; index < GridElements.Length; index++)
			{
				int x = index % (2 * HalfWidth);
				int y = index / (2 * HalfWidth);
				GridElement element = GridElements[y, x];

				if(element.Type == GridElementType.Solid)
					spriteBatch.FillRectangle(GetGridVector2(x, y, stepRate), new Vector2(GridSize, GridSize), Color.DarkGray);
			}
		}

		public void DrawGridLines(SpriteBatch spriteBatch)
		{
			//Distance between each line
			float stepRate = GetStepRate();

			//Draw vertical lines
			for(int y = -HalfHeight; y <= HalfHeight; y++)
			{
				float pixelY = stepRate * y;
				spriteBatch.DrawLine(new Vector2(-HalfWidth*stepRate - (LineWidth/2), pixelY), 
					new Vector2(HalfWidth*stepRate + (LineWidth / 2), pixelY), Color.Gray, LineWidth);
			}

			//Draw horizontal lines
			for (int x = -HalfWidth; x <= HalfWidth; x++)
			{
				float pixelX = stepRate*x;
				spriteBatch.DrawLine(new Vector2(pixelX, -HalfHeight*stepRate - (LineWidth/2f)),
					new Vector2(pixelX, HalfHeight*stepRate + (LineWidth/2)), Color.Gray, LineWidth);
			}
		}

		public float GetStepRate()
		{
			return GridSize + LineWidth / 2 + 1;
		}

		public Vector2 GetGridVector2(int x, int y, float stepRate)
		{
			float xCoord = (stepRate * -HalfWidth) + (x * stepRate) + LineWidth / 2f;
			float yCoord = (stepRate * -HalfHeight) + (y * stepRate) + LineWidth / 2f;
			return new Vector2(xCoord, yCoord);
		}

		public GridElement GetGridElement(Vector2 clickLocation)
		{
			Vector2 worldPos = Game.CameraManager.Camera.ScreenToWorld(clickLocation);
			float stepRate = GetStepRate();
			Vector2 correctedPos = worldPos + new Vector2(stepRate*HalfWidth, stepRate*HalfHeight);
			int xIndex = (int)(correctedPos.X / stepRate);
			int yIndex = (int)(correctedPos.Y / stepRate);
			if (xIndex < HalfWidth*2 && yIndex < HalfHeight*2)
				return GridElements[yIndex, xIndex];

			return null;
		}
	}
}
