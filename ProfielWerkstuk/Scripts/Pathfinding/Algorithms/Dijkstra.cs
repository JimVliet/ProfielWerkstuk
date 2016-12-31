using System.Collections.Generic;
using System.Linq;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding.Algorithms
{
	public class Dijkstra : IAlgorithm
	{
		private readonly GridElement[,] _gridElements;
		private readonly List<GridElement> _resultPath = new List<GridElement>();
		private readonly List<ResultInfo> _resultInfo = new List<ResultInfo>();
		private readonly GridElement _startElement;
		private readonly bool _allowDiagonal;
		public AlgorithmType Type { get; set; }

		public Dijkstra(GridElement[,] gridElements, GridElement start, bool allowDiag)
		{
			_gridElements = gridElements;
			_startElement = start;
			_allowDiagonal = allowDiag;
			Type = AlgorithmType.Dijkstra;
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
				//sorteer de nodesList
				nodes = nodes.OrderBy(x => distances[x]).ToList();

				GridElement smallest = nodes[0];
				nodes.RemoveAt(0);
				_resultInfo.Add(new ResultInfo(smallest, distances[smallest], ResultInfoType.Visited, 
					previous.ContainsKey(smallest) ? previous[smallest] : null));

				if (smallest.Type == GridElementType.End)
				{
					while (previous.ContainsKey(smallest))
					{
						_resultPath.Add(smallest);
						smallest = previous[smallest];
					}
					_resultPath.Add(_startElement);
					break;
				}

				List<GridElement> neighbours = smallest.GetNeighbourElements(_gridElements, _allowDiagonal);
				foreach (GridElement neighbour in neighbours)
				{ 
					double distanceTotal = distances[smallest] + smallest.GetDistance(neighbour);

					if (!distances.ContainsKey(neighbour) || distanceTotal < distances[neighbour])
					{
						distances[neighbour] = distanceTotal;
						previous[neighbour] = smallest;
						nodes.Add(neighbour);
						_resultInfo.Add(new ResultInfo(neighbour, distanceTotal, ResultInfoType.Frontier, smallest));
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
			return "Dijkstra";
		}
	}
}
  