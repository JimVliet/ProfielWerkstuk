using System.Collections.Generic;
using System.Threading;

namespace ProfielWerkstuk.Scripts.GridManagement
{
	public class MapGeneration
	{
		private readonly Grid _grid;
		private Thread _mazeThread;
		private MazeGenerator _mazeGenerator;
		private bool _isCalculating;

		public MapGeneration(Grid grid)
		{
			_grid = grid;
			_grid.GetEventHandlers().GenerateMaze += GenerateMazeEvent;
		}

		private void GenerateMaze()
		{
			if (_mazeThread != null && _mazeThread.IsAlive)
				return;

			_mazeGenerator = new MazeGenerator((_grid.Width + 1)/2, (_grid.Height + 1)/2);
			_mazeThread = new Thread(_mazeGenerator.GenerateMaze);

			_isCalculating = true;
			_mazeThread.Start();
		}

		public void Update()
		{
			if (_grid.AlgorithmActive)
				return;

			if (_isCalculating && !_mazeThread.IsAlive)
			{
				CarveMaze(_mazeGenerator.GetCellPaths());
				_isCalculating = false;
			}
		}

		private void CarveMaze(List<CellPath> paths)
		{
			GridElement[,] gridMap = _grid.GetGridMap();

			_grid.ChangeStartElement(gridMap[0,0]);
			_grid.ChangeEndElement(gridMap[_grid.Height - (2-_grid.Height%2), _grid.Width - (2 - _grid.Width % 2)]);

			for (int index = 0; index < gridMap.Length; index++)
			{
				int x = index % _grid.Width;
				int y = index / _grid.Width;
				GridElement element = gridMap[y, x];

				if(element.Type == GridElementType.Start || element.Type == GridElementType.End)
					continue;

				element.Type = element.X%2 == 0 && element.Y%2 == 0 ? GridElementType.Empty : GridElementType.Solid;
			}

			foreach (var cellPath in paths)
			{
				int xVector = cellPath.CellB.X - cellPath.CellA.X;
				if (xVector != 0)
				{
					gridMap[cellPath.CellA.Y * 2, cellPath.CellA.X * 2 + xVector].Type = GridElementType.Empty;
					continue;
				}

				int yVector = cellPath.CellB.Y - cellPath.CellA.Y;
				if (yVector != 0)
				{
					gridMap[cellPath.CellA.Y * 2 + yVector, cellPath.CellA.X * 2].Type = GridElementType.Empty;
				}
			}
		}

		private void GenerateMazeEvent()
		{
			GenerateMaze();
			_grid.GetEventHandlers().ResetDisplayer?.Invoke();
		}
	}
}
