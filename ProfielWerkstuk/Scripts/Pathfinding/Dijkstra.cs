using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding
{
	public class Dijkstra : IAlgorithm
	{
		public GridElement[,] GridElements;
		public List<GridElement> ResultPath;
		public List<ResultInfo> ResultInfo = new List<ResultInfo>();
		public GridElement StartElement;
		public bool AllowDiagonal;
		public AlgorithmType Type { get; set; }

		public Dijkstra(GridElement[,] gridElements, GridElement start, bool allowDiag)
		{
			GridElements = gridElements;
			StartElement = start;
			AllowDiagonal = allowDiag;
			Type = AlgorithmType.Dijkstra;
		}

		public void CalculatePath()
		{
			Dictionary<GridElement, GridElement> previous = new Dictionary<GridElement, GridElement>();
			Dictionary<GridElement, double> distances = new Dictionary<GridElement, double>
			{
				[StartElement] = 0
			};

			List<GridElement> nodes = new List<GridElement>
			{
				StartElement
			};

			while (nodes.Count != 0)
			{
				//sorteer de nodesList
				nodes = nodes.OrderBy(x => distances[x]).ToList();

				GridElement smallest = nodes[0];
				nodes.RemoveAt(0);

				if (smallest.Type == GridElementType.End)
				{
					ResultPath = new List<GridElement>();

					while (previous.ContainsKey(smallest))
					{
						ResultPath.Add(smallest);
						smallest = previous[smallest];
					}
					ResultPath.Add(StartElement);
					break;
				}

				ResultInfo.Add(new ResultInfo(distances[smallest], smallest.X, smallest.Y, ResultInfoType.Visited));
				List<GridElement> neighbours = smallest.GetNeighbourElements(GridElements, AllowDiagonal);
				foreach (GridElement neighbour in neighbours)
				{ 
					double distanceTotal = distances[smallest] + smallest.GetDistance(neighbour);
					if (!distances.ContainsKey(neighbour) || distanceTotal < distances[neighbour])
					{
						distances[neighbour] = distanceTotal;
						previous[neighbour] = smallest;
						nodes.Add(neighbour);
						ResultInfo.Add(new ResultInfo(distanceTotal, neighbour.X, neighbour.Y, ResultInfoType.Frontier));
					}
				}
			}
		}

		public void Callback(AlgorithmManager manager)
		{
			manager.Displayer.UpdateDisplayer(manager.GetPathDrawingPoints(ResultPath), ResultInfo);
		}

		public string GetName()
		{
			return "Dijkstra";
		}
	}
}
  