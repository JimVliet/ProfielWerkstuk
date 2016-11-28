using System;
using System.Collections.Generic;

namespace ProfielWerkstuk.Scripts.GridManagement
{
	public class GridElement
	{
		public int X;
		public int Y;
		public GridElementType Type = GridElementType.Empty;

		public GridElement(int x, int y)
		{
			X = x;
			Y = y;
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
			if (allowLeft)
				neighbours.Add(grid[Y, X - 1]);
			if (allowRight)
				neighbours.Add(grid[Y, X + 1]);
			if (allowBottom)
				neighbours.Add(grid[Y + 1, X]);

			if (allowDiagonal)
			{
				if (IsWithinRange(X - 1, Y - 1, grid) && grid[Y - 1, X - 1].Type != GridElementType.Solid && (allowTop || allowLeft))
					neighbours.Add(grid[Y - 1, X - 1]);
				if (IsWithinRange(X + 1, Y - 1, grid) && grid[Y - 1, X + 1].Type != GridElementType.Solid && (allowTop || allowRight))
					neighbours.Add(grid[Y - 1, X + 1]);
				if (IsWithinRange(X - 1, Y + 1, grid) && grid[Y + 1, X - 1].Type != GridElementType.Solid && (allowBottom || allowLeft))
					neighbours.Add(grid[Y + 1, X - 1]);
				if (IsWithinRange(X + 1, Y + 1, grid) && grid[Y + 1, X + 1].Type != GridElementType.Solid && (allowBottom || allowRight))
					neighbours.Add(grid[Y + 1, X + 1]);
			}

			return neighbours;
		}

		public double GetDistance(GridElement element)
		{
			return Math.Sqrt((X - element.X) * (X - element.X) + (Y - element.Y) * (Y - element.Y));
		}

		public static bool IsWithinRange(int x, int y, GridElement[,] grid)
		{
			return x >= 0 && x < grid.GetLength(1) && y >= 0 && y < grid.GetLength(0);
		}
	}

	public enum GridElementType
	{
		Empty, Solid, Start, End, Null
	}
}
