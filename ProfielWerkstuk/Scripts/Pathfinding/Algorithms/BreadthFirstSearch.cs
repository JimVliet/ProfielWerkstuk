using System.Collections.Generic;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding.Algorithms
{
	public class BreadthFirstSearch : IAlgorithm
	{
		public AlgorithmType Type { get; set; }
		private readonly GridElement[,] _gridElements;
		private readonly List<ResultInfo> _resultInfo = new List<ResultInfo>();
		private readonly List<GridElement> _resultPath = new List<GridElement>();
		private readonly GridElement _startElement;
		private readonly bool _allowDiagonal;

		public BreadthFirstSearch(GridElement[,] gridElements, GridElement start, bool allowDiag)
		{
			Type = AlgorithmType.BreadthFirstSearch;
			_gridElements = gridElements;
			_startElement = start;
			_allowDiagonal = allowDiag;
		}

		public void CalculatePath()
		{
			Dictionary<GridElement, GridElement> previous = new Dictionary<GridElement, GridElement>();
			Dictionary<GridElement, double> distances = new Dictionary<GridElement, double>
			{
				[_startElement] = 0
			};

			List<GridElement> nodes = new List<GridElement>
			{
				_startElement
			};

			while (nodes.Count != 0)
			{
				GridElement current = nodes[0];
				nodes.RemoveAt(0);
				_resultInfo.Add(new ResultInfo(current, distances[current], ResultInfoType.Visited, 
					previous.ContainsKey(current) ? previous[current] : null));

				if (current.Type == GridElementType.End)
				{
					while (previous.ContainsKey(current))
					{
						_resultPath.Add(current);
						current = previous[current];
					}
					_resultPath.Add(_startElement);
					break;
				}

				List<GridElement> neighBours = current.GetNeighbourElements(_gridElements, _allowDiagonal);
				foreach (var neighbour in neighBours)
				{
					if (!distances.ContainsKey(neighbour))
					{
						distances[neighbour] = distances[current] + 1;
						previous[neighbour] = current;
						nodes.Add(neighbour);
						_resultInfo.Add(new ResultInfo(neighbour, distances[current] + 1, ResultInfoType.Frontier, current));
					}
				}
			}
		}

		public void Callback(AlgorithmManager manager)
		{
			manager.Displayer.UpdateDisplayer(manager.GetPathDrawingPoints(_resultPath), _resultInfo);
		}

		public string GetName()
		{
			return "BreadthFirstSearch";
		}
	}
}
