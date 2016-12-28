using Microsoft.Xna.Framework;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding
{
	public class ResultInfo
	{
		private readonly ResultInfoType _type;
		public readonly double Distance;
		public readonly int X;
		public readonly int Y;
		public readonly GridElement PreviousElement;

		public ResultInfo(GridElement element, double distance, ResultInfoType type, GridElement previous)
		{
			_type = type;
			Distance = distance;
			X = element.X;
			Y = element.Y;
			PreviousElement = previous;
		}

		public Color GetColor()
		{
			switch (_type)
			{
				case ResultInfoType.Frontier:
					return Color.LightGreen;
				case ResultInfoType.Visited:
					return Color.LightBlue;
			}
			return Color.Black;
		}

		public double GetExtraDistance()
		{
			switch (_type)
			{
				case ResultInfoType.Frontier:
					return 1d;
				default:
					return 0d;
			}
		}
	}

	public enum ResultInfoType
	{
		Visited, Frontier, Cleared
	}
}
