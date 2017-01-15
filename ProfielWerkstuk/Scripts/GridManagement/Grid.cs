using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using ProfielWerkstuk.Scripts.GUI;

namespace ProfielWerkstuk.Scripts.GridManagement
{
	public class Grid
	{
		private readonly ProfielWerkstuk _game;
		private readonly GridPainter _gridPainter;
		public readonly int GridSize;
		public int Width;
		public int Height;
		private readonly float _lineWidth;
		public RectangleF GridBounds;
		private GridElement[,] _gridElements;
		public GridElementType GridHoldType = GridElementType.Null;
		private GridElement _startElement;
		private GridElement _endElement;
		public bool AlgorithmActive;
		private DraggingInfo _draggingInfo;
		private readonly MapGeneration _mapGeneration;
		private MapSize _mapSize;

		public Grid(ProfielWerkstuk game, int gridSize, MapSize mapSize, int lineWidth)
		{
			_game = game;
			_mapGeneration = new MapGeneration(this);
			_gridPainter = new GridPainter(this, GetEventHandlers());
			GridSize = gridSize;
			_lineWidth = lineWidth;
			_mapSize = mapSize;
			GenerateGrid(mapSize);

			GetEventHandlers().ClearGridClicked += ClearGridClicked;
			GetEventHandlers().ChangeMapSizeClicked += ChangeMapSizeClicked;
			GetEventHandlers().MapsizeChanged += MapSizeChanged;
		}

		private void GenerateGrid(MapSize mapSize)
		{
			WidthHeight widthHeight = GetWidthHeight(mapSize);
			Width = widthHeight.Width;
			Height = widthHeight.Height;

			_gridElements = new GridElement[Height, Width];

			for (int index = 0; index < _gridElements.Length; index++)
			{
				int x = index % Width;
				int y = index / Width;
				_gridElements[y, x] = new GridElement(x, y);
			}

			float stepRate = GetStepRate();
			GridBounds = new RectangleF(-Width / 2f * stepRate, -Height / 2f * stepRate,
				Width * stepRate, Height * stepRate);

			_startElement = _gridElements[Height/2, Width / 4];
			_endElement = _gridElements[Height/2, Width / 4 * 3];
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
				element.Draw(spriteBatch, this, _draggingInfo, _game.AlgorithmManager.ShowArrows(), _game.AlgorithmManager.ShowInfoText(), _game.UserInterface.BoldFont16);
			}
		}

		public void DrawGridLines(SpriteBatch spriteBatch)
		{
			//Distance between each line
			float stepRate = GetStepRate();

			float startHeight = -Height/2f * stepRate;
			float startWidth = -Width/2f*stepRate;

			for (int i = 0; i <= Height; i++)
			{
				float pixelY = startHeight + stepRate*i;
				spriteBatch.DrawLine(new Vector2(-Width / 2f*stepRate - _lineWidth/2, pixelY),
					new Vector2(Width / 2f*stepRate + _lineWidth/2, pixelY), Color.Gray, _lineWidth);
			}

			for (int i = 0; i <= Width; i++)
			{
				float pixelX = startWidth + stepRate*i;
				spriteBatch.DrawLine(new Vector2(pixelX, -Height / 2f*stepRate - _lineWidth/2f),
					new Vector2(pixelX, Height / 2f*stepRate + _lineWidth/2), Color.Gray, _lineWidth);
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

			_mapGeneration.Update();
		}

		private float GetStepRate()
		{
			return GridSize + _lineWidth / 2 + 1;
		}

		public Vector2 GetGridVector2(int x, int y)
		{
			float stepRate = GetStepRate();

			float xCoord = stepRate * -Width/2f + x * stepRate + _lineWidth / 2f;
			float yCoord = stepRate * -Height/2f + y * stepRate + _lineWidth / 2f;
			return new Vector2(xCoord, yCoord);
		}

		public GridElement GetGridElement(Vector2 clickLocation)
		{
			Vector2 worldPos = _game.CameraManager.Camera.ScreenToWorld(clickLocation);
			float stepRate = GetStepRate();
			Vector2 correctedPos = worldPos + new Vector2(stepRate*Width/2f, stepRate*Height/2f);
			int xIndex = (int)(correctedPos.X / stepRate);
			int yIndex = (int)(correctedPos.Y / stepRate);
			if (xIndex >= 0 && xIndex < Width && yIndex >= 0 && yIndex < Height)
				return _gridElements[yIndex, xIndex];

			return null;
		}

		public void GridClicked(Vector2 clickLocation)
		{
			_gridPainter.GridClicked(clickLocation);
		}

		public void GridHoldClick(Vector2 mouseLocation)
		{
			_gridPainter.GridHoldClick(mouseLocation);
		}

		public GridElement[,] GetGridMap()
		{
			return AlgorithmActive ? null : _gridElements;
		}

		public GridElement GetStartElement()
		{
			return AlgorithmActive ? null : _startElement;
		}

		public GridElement GetEndElement()
		{
			return AlgorithmActive ? null : _endElement;
		}

		private void ClearGrid()
		{
			foreach (var element in _gridElements)
			{
				if(!element.IsSpecial())
					element.Type = GridElementType.Empty;
			}
		}

		private void ClearGridClicked()
		{
			ClearGrid();
			GetEventHandlers().ResetDisplayer?.Invoke();

			ChangeStartElement(_gridElements[Height / 2, Width / 4]);
			ChangeEndElement(_gridElements[Height / 2, Width / 4 * 3]);
		}

		public EventHandlers GetEventHandlers()
		{
			return _game.EventHandlers;
		}

		private void ChangeMapSizeClicked()
		{
			GetEventHandlers().MapsizeChanged?.Invoke(NextMapSize(_mapSize));
		}
		private void MapSizeChanged(MapSize mapSize)
		{
			_mapSize = mapSize;
			GenerateGrid(_mapSize);
		}

		private MapSize NextMapSize(MapSize sizeType)
		{
			switch (sizeType)
			{
				case MapSize.ExtraLarge:
					return MapSize.ExtraSmall;
				case MapSize.Large:
					return MapSize.ExtraLarge;
				case MapSize.Medium:
					return MapSize.Large;
				case MapSize.Small:
					return MapSize.Medium;
				case MapSize.ExtraSmall:
					return MapSize.Small;
			}

			return MapSize.Medium;
		}

		private WidthHeight GetWidthHeight(MapSize mapSize)
		{
			switch (mapSize)
			{
				case MapSize.ExtraLarge:
					return new WidthHeight(105, 85);
				case MapSize.Large:
					return new WidthHeight(85, 65);
				case MapSize.Medium:
					return new WidthHeight(64, 45);
				case MapSize.Small:
					return new WidthHeight(45, 25);
				case MapSize.ExtraSmall:
					return new WidthHeight(25, 15);
			}

			return new WidthHeight(15, 11);
		}
	}

	internal struct WidthHeight
	{
		public readonly int Width, Height;

		public WidthHeight(int width, int height)
		{
			Width = width;
			Height = height;
		}
	}

	public struct DraggingInfo
	{
		public GridElement DragElement;
		public GridElement TargetElement;
	}

	public enum MapSize
	{
		ExtraLarge, Large, Medium, Small, ExtraSmall
	}
}
