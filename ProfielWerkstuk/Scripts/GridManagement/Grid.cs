using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;

namespace ProfielWerkstuk.Scripts.GridManagement
{
	public class Grid
	{
		private ProfielWerkstuk _game;
		public int GridSize;
		public int HalfWidth;
		public int HalfHeight;
		public float LineWidth;
		public RectangleF GridBounds;
		private readonly GridElement[,] _gridElements;
		public GridElementType GridHoldType = GridElementType.Null;
		private GridElement _startElement;
		private GridElement _endElement;
		public bool AlgorithmActive;
		private DraggingInfo _draggingInfo;

		public Grid(ProfielWerkstuk game, int gridSize, int halfWidth, int halfHeight, int lineWidth)
		{
			_game = game;
			GridSize = gridSize;
			HalfWidth = halfWidth;
			HalfHeight = halfHeight;
			LineWidth = lineWidth;

			_gridElements = new GridElement[2 * halfHeight, 2 * halfWidth];

			for (int index = 0; index < _gridElements.Length; index++)
			{
				int x = index%(2*halfWidth);
				int y = index/(2*halfWidth);
				_gridElements[y, x] = new GridElement(x, y);
			}

			float stepRate = GetStepRate();
			GridBounds = new RectangleF(-halfWidth * stepRate, -halfHeight * stepRate,
				2*halfWidth * stepRate, 2*halfHeight * stepRate);
		}

		public void GenerateGrid()
		{
			_startElement = _gridElements[HalfHeight, HalfWidth / 2];
			_endElement = _gridElements[HalfHeight, HalfWidth + HalfWidth / 2];
			_startElement.Type = GridElementType.Start;
			_endElement.Type = GridElementType.End;
		}

		public void ChangeStartElement(GridElement newElement)
		{
			if (AlgorithmActive)
				return;

			_startElement.Type = GridElementType.Empty;
			newElement.Type = GridElementType.Start;
			_startElement = newElement;
		}

		public void ChangeEndElement(GridElement newElement)
		{
			if(AlgorithmActive)
				return;

			_endElement.Type = GridElementType.Empty;
			newElement.Type = GridElementType.End;
			_endElement = newElement;
		}

		public void DrawGridSquares(SpriteBatch spriteBatch)
		{
			foreach (GridElement element in _gridElements)
			{
				element.Draw(spriteBatch, this, _draggingInfo);
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

		public void Update(GameTime gameTime)
		{
			Vector2 mouseLocation = _game.InputManager.GetMouseLocation();

			GridElement dragElement = _game.InputManager.DragElement;
			GridElement targetElement = GetGridElement(mouseLocation);

			bool bothNotNull = dragElement != null && targetElement != null;
			_draggingInfo.DragElement = bothNotNull ? dragElement : null;
			_draggingInfo.TargetElement = bothNotNull ? targetElement : null;
		}

		private float GetStepRate()
		{
			return GridSize + LineWidth / 2 + 1;
		}

		public Vector2 GetGridVector2(int x, int y)
		{
			float stepRate = GetStepRate();

			float xCoord = (stepRate * -HalfWidth) + (x * stepRate) + LineWidth / 2f;
			float yCoord = (stepRate * -HalfHeight) + (y * stepRate) + LineWidth / 2f;
			return new Vector2(xCoord, yCoord);
		}

		public GridElement GetGridElement(Vector2 clickLocation)
		{
			Vector2 worldPos = _game.CameraManager.Camera.ScreenToWorld(clickLocation);
			float stepRate = GetStepRate();
			Vector2 correctedPos = worldPos + new Vector2(stepRate*HalfWidth, stepRate*HalfHeight);
			int xIndex = (int)(correctedPos.X / stepRate);
			int yIndex = (int)(correctedPos.Y / stepRate);
			if (xIndex >= 0 && xIndex < HalfWidth*2 && yIndex >= 0 && yIndex < HalfHeight*2)
				return _gridElements[yIndex, xIndex];

			return null;
		}

		public void GridClicked(Vector2 clickLocation)
		{
			if (AlgorithmActive)
				return;

			GridElement element = GetGridElement(clickLocation);
			if (element == null)
				return;
			switch (element.Type)
			{
				case GridElementType.Empty:
					element.Type = GridElementType.Solid;
					break;
				case GridElementType.Solid:
					element.Type = GridElementType.Empty;
					break;
			}
		}

		public void GridHoldClick(Vector2 mouseLocation)
		{
			if (AlgorithmActive)
				return;

			GridElement element = GetGridElement(mouseLocation);
			if(element == null || element.Type == GridElementType.Start || element.Type == GridElementType.End)
				return;
			if (GridHoldType == GridElementType.Null)
			{
				GridHoldType = element.Type == GridElementType.Empty ? GridElementType.Solid : GridElementType.Empty;
				return;
			}

			element.Type = GridHoldType;
		}

		public GridElement[,] GetGridMap()
		{
			return AlgorithmActive ? null : _gridElements;
		}

		public GridElement GetStartElement()
		{
			return AlgorithmActive ? null : _startElement;
		}

		public DraggingInfo GetDraggingInfo()
		{
			return _draggingInfo;
		}
	}

	public struct DraggingInfo
	{
		public GridElement DragElement;
		public GridElement TargetElement;
	}
}
