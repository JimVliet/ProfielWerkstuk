using Microsoft.Xna.Framework;

namespace ProfielWerkstuk.Scripts.Pathfinding
{
	public class ResultInfo
	{
		public ResultInfoType Type;
		public double Distance;
		public int X;
		public int Y;

		public ResultInfo(double distance, int x, int y, ResultInfoType type)
		{
			Distance = distance;
			X = x;
			Y = y;
			Type = type;
		}

		public Color GetColor()
		{
			switch (Type)
			{
				case ResultInfoType.Frontier:
					return Color.LightGreen;
				case ResultInfoType.Visited:
					return Color.LightBlue;
			}
			return Color.Black;
		}
	}

	public enum ResultInfoType
	{
		Visited, Frontier
	}
}
