using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfielWerkstuk.Scripts.GridManagement
{
	public class MazeGenerator
	{
		private readonly Cell[,] _cells;
		private readonly List<CellPath> _paths = new List<CellPath>();

		public MazeGenerator(int width, int height)
		{
			_cells = new Cell[height, width];

			for (int index = 0; index < _cells.Length; index++)
			{
				int x = index%width;
				int y = index/width;
				
				_cells[y, x] = new Cell(x, y);
			}
		}

		public void GenerateMaze()
		{
			Random rng = new Random();
			List<Cell> cellStack = new List<Cell>
			{
				_cells[0,0]
			};
			_cells[0,0].CellStatus = CellStatus.Visited;

			while (cellStack.Count != 0)
			{
				Cell currentCell = cellStack[cellStack.Count-1];

				List<Cell> neighbours = currentCell.GetNeighbourCells(_cells);
				if (neighbours.Count == 0)
				{
					cellStack.RemoveAt(cellStack.Count - 1);
					continue;
				}

				neighbours = neighbours.OrderBy(a => rng.Next()).ToList();

				Cell newCell = neighbours[0];
				newCell.CellStatus = CellStatus.Visited;
				cellStack.Add(newCell);
				_paths.Add(new CellPath(currentCell, newCell));
			}
		}

		public List<CellPath> GetCellPaths()
		{
			return _paths;
		}
	}

	public struct CellPath
	{
		public readonly Cell CellA;
		public readonly Cell CellB;

		public CellPath(Cell a, Cell b)
		{
			CellA = a;
			CellB = b;
		}
	}

	public class Cell
	{
		public readonly int X, Y;
		public CellStatus CellStatus;

		public Cell(int x, int y)
		{
			X = x;
			Y = y;
			CellStatus = CellStatus.Open;
		}

		internal List<Cell> GetNeighbourCells(Cell[,] cells)
		{
			List<Cell> neighbours = new List<Cell>();

			bool allowTop = IsWithinRange(X, Y - 1, cells) && cells[Y - 1, X].CellStatus == CellStatus.Open;
			bool allowLeft = IsWithinRange(X - 1, Y, cells) && cells[Y, X - 1].CellStatus == CellStatus.Open;
			bool allowRight = IsWithinRange(X + 1, Y, cells) && cells[Y, X + 1].CellStatus == CellStatus.Open;
			bool allowBottom = IsWithinRange(X, Y + 1, cells) && cells[Y + 1, X].CellStatus == CellStatus.Open;

			if (allowTop)
				neighbours.Add(cells[Y - 1, X]);
			if (allowBottom)
				neighbours.Add(cells[Y + 1, X]);
			if (allowLeft)
				neighbours.Add(cells[Y, X - 1]);
			if (allowRight)
				neighbours.Add(cells[Y, X + 1]);

			return neighbours;
		}

		private static bool IsWithinRange(int x, int y, Cell[,] cells)
		{
			return x >= 0 && x < cells.GetLength(1) && y >= 0 && y < cells.GetLength(0);
		}
	}

	public enum CellStatus
	{
		Open, Visited
	}
}
