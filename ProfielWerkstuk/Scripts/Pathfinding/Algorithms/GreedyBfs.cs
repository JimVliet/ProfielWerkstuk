using System.Collections.Generic;
using System.Linq;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding.Algorithms
{
	public class GreedyBfs : IAlgorithm
	{
		private readonly GridElement[,] _gridElements;
		private readonly List<GridElement> _resultPath = new List<GridElement>();
		private readonly List<ResultInfo> _resultInfo = new List<ResultInfo>();
		private readonly GridElement _startElement;
		private readonly GridElement _endElement;
		private readonly bool _allowDiagonal;
		public AlgorithmType Type { get; set; }

		public GreedyBfs(GridElement[,] gridElements, GridElement start, GridElement end, bool allowDiag)
		{
			_gridElements = gridElements;
			_startElement = start;
			_endElement = end;
			_allowDiagonal = allowDiag;
			Type = AlgorithmType.GreedyBestFirstSearch;
		}

		public void CalculatePath()
		{
			Dictionary<GridElement, GridElement> previous = new Dictionary<GridElement, GridElement>();
			Dictionary<GridElement, double> distances = new Dictionary<GridElement, double>
			{
				[_startElement] = _startElement.GetDistance(_endElement)
			};

			List<GridElement> nodes = new List<GridElement>
			{
				_startElement
			};

			while (nodes.Count != 0)
			{
				nodes = nodes.OrderBy(x => distances[x]).ToList();

				GridElement closestElement = nodes[0];
				nodes.RemoveAt(0);
				_resultInfo.Add(new ResultInfo(closestElement, distances[closestElement], ResultInfoType.Visited,
					previous.ContainsKey(closestElement) ? previous[closestElement] : null, true));

				if (closestElement.Type == GridElementType.End)
				{
					while (previous.ContainsKey(closestElement))
					{
						_resultPath.Add(closestElement);
						closestElement = previous[closestElement];
					}
					_resultPath.Add(_startElement);
					break;
				}

				List<GridElement> neighbours = closestElement.GetNeighbourElements(_gridElements, _allowDiagonal);
				foreach (GridElement neighbour in neighbours)
				{
					if (!distances.ContainsKey(neighbour))
					{
						double distanceTotal = neighbour.GetDistance(_endElement);

						distances[neighbour] = distanceTotal;
						previous[neighbour] = closestElement;
						nodes.Add(neighbour);
						_resultInfo.Add(new ResultInfo(neighbour, distanceTotal, ResultInfoType.Frontier, closestElement, true));
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
			return "Greedy best-first search";
		}
	}
}
