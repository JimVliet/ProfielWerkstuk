using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding.AlgorithmDisplayers
{
	public class DijkstraDisplayer : IDisplayer
	{
		public List<Vector2> PathDrawingPoints { get; set; }
		public Grid Grid;

		public DijkstraDisplayer(Grid grid)
		{
			Grid = grid;
			PathDrawingPoints = null;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (PathDrawingPoints != null && PathDrawingPoints.Count > 1)
			{
				for (int i = 1; i < PathDrawingPoints.Count; i++)
				{
					spriteBatch.DrawLine(PathDrawingPoints[i - 1], PathDrawingPoints[i], Color.Orange, 2f);
				}
			}
		}
	}
}
