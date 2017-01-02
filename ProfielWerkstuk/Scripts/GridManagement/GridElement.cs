using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using ProfielWerkstuk.Scripts.Pathfinding;

namespace ProfielWerkstuk.Scripts.GridManagement
{
	public class GridElement
	{
		public readonly int X;
		public readonly int Y;
		private readonly List<ResultInfo> _resultInfoList;
		public GridElementType Type = GridElementType.Empty;

		public GridElement(int x, int y)
		{
			X = x;
			Y = y;
			_resultInfoList = new List<ResultInfo>();
		}

		public bool IsSpecial()
		{
			return Type == GridElementType.Start || Type == GridElementType.End;
		}

		public ResultInfo GetResultInfo()
		{
			return _resultInfoList.Count > 0 ? _resultInfoList[_resultInfoList.Count-1] : null;
		}

		public void AttachResultInfo(ResultInfo resultInfo)
		{
			_resultInfoList.Add(resultInfo);
		}

		public bool HasResultInfo()
		{
			return _resultInfoList.Count > 0;
		}

		public bool RemoveLastInfo()
		{
			if(_resultInfoList.Count <= 0)
				return false;

			_resultInfoList.RemoveAt(_resultInfoList.Count-1);
			return true;
		}

		public void ClearResultInfo()
		{
			if(HasResultInfo())
				_resultInfoList.Clear();
		}

		private bool IsBeingDraggedOver(DraggingInfo dragInfo)
		{
			return dragInfo.TargetElement?.X == X && dragInfo.TargetElement.Y == Y;
		}

		private bool IsDragging(DraggingInfo dragInfo)
		{
			return dragInfo.DragElement?.X == X && dragInfo.DragElement.Y == Y;
		}

		public void Draw(SpriteBatch spriteBatch, Grid grid, DraggingInfo dragInfo, bool showArrows)
		{
			if (dragInfo.DragElement != null && Type != GridElementType.Start && Type != GridElementType.End && IsBeingDraggedOver(dragInfo))
			{
				DrawDragElement(spriteBatch, grid, dragInfo.DragElement);
				return;
			}

			if (!HasResultInfo())
			{
				if(Type == GridElementType.Empty)
					return;

				spriteBatch.FillRectangle(grid.GetGridVector2(X, Y), new Vector2(grid.GridSize, grid.GridSize),
					GetDrawingColor(dragInfo));
				return;
			}

			if (Type == GridElementType.Empty)
			{
				spriteBatch.FillRectangle(grid.GetGridVector2(X, Y), new Vector2(grid.GridSize, grid.GridSize), GetResultInfo().GetColor());
			}
			else
			{
				spriteBatch.FillRectangle(grid.GetGridVector2(X, Y), new Vector2(grid.GridSize, grid.GridSize), GetDrawingColor(dragInfo));

				if (Type != GridElementType.Start && Type != GridElementType.End && Type != GridElementType.Solid)
					spriteBatch.DrawRectangle(grid.GetGridVector2(X, Y), new Vector2(grid.GridSize, grid.GridSize),
						GetResultInfo().GetColor(), grid.GridSize/16f);
			}

			if (showArrows)
				GetResultInfo().DrawArrow(spriteBatch, grid, 40, 10, Color.SaddleBrown);
		}

		private void DrawDragElement(SpriteBatch spriteBatch, Grid grid, GridElement dragElement)
		{
			spriteBatch.FillRectangle(grid.GetGridVector2(X, Y), new Vector2(grid.GridSize, grid.GridSize), dragElement.GetDragElementColor());
		}

		private Color GetDragElementColor()
		{
			switch (Type)
			{
				case GridElementType.Start:
					return Color.Green;
				case GridElementType.End:
					return Color.Red;
			}

			return Color.Yellow;
		}

		public List<GridElement> GetPathToStart()
		{
			if(!HasResultInfo())
				return null;

			List<GridElement> gridElements = new List<GridElement>
			{
				this
			};

			GridElement previous = GetResultInfo().PreviousElement;
			while (previous != null)
			{
				gridElements.Add(previous);

				previous = previous.GetResultInfo().PreviousElement;
			}

			return gridElements;
		}

		private Color GetDrawingColor(DraggingInfo dragInfo)
		{
			switch (Type)
			{
				case GridElementType.Solid:
					return Color.DarkGray;
				case GridElementType.Start:
					return IsDragging(dragInfo) ? new Color(0, 161, 0) : Color.Green;
				case GridElementType.End:
					return IsDragging(dragInfo) ? new Color(255, 66, 66) : Color.Red;
				case GridElementType.Forest:
					return Color.DarkGreen;
				case GridElementType.River:
					return new Color(86, 122, 158);
				case GridElementType.Road:
					return Color.RosyBrown;
			}
			return Color.Yellow;
		}

		public List<GridElement> GetNeighbourElements(GridElement[,] grid, bool allowDiagonal)
		{
			List<GridElement> neighbours = new List<GridElement>();

			bool allowTop = IsWithinRange(X, Y - 1, grid) && grid[Y - 1, X].Type != GridElementType.Solid;
			bool allowLeft = IsWithinRange(X - 1, Y, grid) && grid[Y, X - 1].Type != GridElementType.Solid;
			bool allowRight = IsWithinRange(X + 1, Y, grid) && grid[Y, X + 1].Type != GridElementType.Solid;
			bool allowBottom = IsWithinRange(X, Y + 1, grid) && grid[Y + 1, X].Type != GridElementType.Solid;

			if (allowTop)
				neighbours.Add(grid[Y - 1, X]);
			if (allowBottom)
				neighbours.Add(grid[Y + 1, X]);
			if (allowLeft)
				neighbours.Add(grid[Y, X - 1]);
			if (allowRight)
				neighbours.Add(grid[Y, X + 1]);

			if (!allowDiagonal)
				return neighbours;

			if (IsWithinRange(X - 1, Y - 1, grid) && grid[Y - 1, X - 1].Type != GridElementType.Solid && (allowTop || allowLeft))
				neighbours.Add(grid[Y - 1, X - 1]);
			if (IsWithinRange(X + 1, Y - 1, grid) && grid[Y - 1, X + 1].Type != GridElementType.Solid && (allowTop || allowRight))
				neighbours.Add(grid[Y - 1, X + 1]);
			if (IsWithinRange(X - 1, Y + 1, grid) && grid[Y + 1, X - 1].Type != GridElementType.Solid && (allowBottom || allowLeft))
				neighbours.Add(grid[Y + 1, X - 1]);
			if (IsWithinRange(X + 1, Y + 1, grid) && grid[Y + 1, X + 1].Type != GridElementType.Solid && (allowBottom || allowRight))
				neighbours.Add(grid[Y + 1, X + 1]);

			return neighbours;
		}

		public double GetDistance(GridElement element)
		{
			return Math.Sqrt((X - element.X) * (X - element.X) + (Y - element.Y) * (Y - element.Y));
		}

		private static bool IsWithinRange(int x, int y, GridElement[,] grid)
		{
			return x >= 0 && x < grid.GetLength(1) && y >= 0 && y < grid.GetLength(0);
		}

		public double GetTravelCost()
		{
			switch (Type)
			{
				case GridElementType.River:
					return 50;
				case GridElementType.Forest:
					return 20;
				case GridElementType.Road:
					return 1;
				default:
					return 5;
			}
		}
	}

	public enum GridElementType
	{
		Empty, Solid, Start, End, Null, Forest, River, Road
	}
}
