using System.Collections.Generic;
using System.Linq;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding.Algorithms
{
	public class AStar : IAlgorithm
	{
		private readonly GridElement[,] _gridElements;
		private readonly List<GridElement> _resultPath = new List<GridElement>();
		private readonly List<ResultInfo> _resultInfo = new List<ResultInfo>();
		private readonly GridElement _startElement;
		private readonly GridElement _endElement;
		private readonly bool _allowDiagonal;
		public AlgorithmType Type { get; set; }

		public AStar(GridElement[,] gridElements, GridElement start, GridElement end, bool allowDiag)
		{
			_gridElements = gridElements;
			_startElement = start;
			_endElement = end;
			_allowDiagonal = allowDiag;
			Type = AlgorithmType.AStar;
		}

		public void CalculatePath()
		{
			//TO-DO: Finish this
			Dictionary<GridElement, GridElement> previous = new Dictionary<GridElement, GridElement>();
			Dictionary<GridElement, double> costSoFar = new Dictionary<GridElement, double>
			{
				[_startElement] = 0
			};

			Dictionary<GridElement, double> priority = new Dictionary<GridElement, double>
			{
				[_startElement] = 0
			};

			List<GridElement> nodes = new List<GridElement>
			{
				_startElement
			};

			while (nodes.Count != 0)
			{
				//F = G + H
				nodes = nodes.OrderBy(x => priority[x]).ToList();

				GridElement bestElement = nodes[0];
				nodes.RemoveAt(0);
				_resultInfo.Add(new ResultInfo(bestElement, costSoFar[bestElement], Heuristic(bestElement), ResultInfoType.Visited,
					previous.ContainsKey(bestElement) ? previous[bestElement] : null));

				if (bestElement.Type == GridElementType.End)
				{
					while (previous.ContainsKey(bestElement))
					{
						_resultPath.Add(bestElement);
						bestElement = previous[bestElement];
					}
					_resultPath.Add(_startElement);
					break;
				}

				List<GridElement> neighbours = bestElement.GetNeighbourElements(_gridElements, _allowDiagonal);
				foreach (GridElement neighbour in neighbours)
				{
					double newCost = costSoFar[bestElement] + bestElement.GetDistance(neighbour)*neighbour.GetTravelCost();

					if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
					{
						double heuristic = Heuristic(neighbour);

						costSoFar[neighbour] = newCost;
						priority[neighbour] = newCost + heuristic;
						nodes.Add(neighbour);
						previous[neighbour] = bestElement;
						_resultInfo.Add(new ResultInfo(neighbour, newCost, heuristic, ResultInfoType.Frontier, bestElement));
					}
				}
			}
		}

		private double Heuristic(GridElement element)
		{
			return element.GetDistance(_endElement);
		}

		public void Callback(AlgorithmManager manager)
		{
			manager.Displayer.UpdateDisplayer(manager.GetPathDrawingPoints(_resultPath), _resultInfo);
		}

		public string GetName()
		{
			return "A*";
		}
	}
}
