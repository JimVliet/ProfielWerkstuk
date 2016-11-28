﻿using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GridManagement;
using ProfielWerkstuk.Scripts.Pathfinding.AlgorithmDisplayers;
using ProfielWerkstuk.Scripts.Utility;

namespace ProfielWerkstuk.Scripts.Pathfinding
{
	public class AlgorithmManager
	{
		public Thread AlgorithmThread;
		public Game1 Game;
		public IAlgorithm CurrentAlgorithm;
		public IDisplayer Displayer;
		public bool IsCalculating;
		public bool AllowDiagonal;

		public AlgorithmManager(Game1 game)
		{
			Game = game;
		}

		public void Calculate()
		{
			if(AlgorithmThread != null && AlgorithmThread.IsAlive)
				return;

			CurrentAlgorithm = new Dijkstra(Game.Grid.GridElements, Game.Grid.StartElement, AllowDiagonal);
			Displayer = new DijkstraDisplayer(Game.Grid);
			AlgorithmThread = new Thread(CurrentAlgorithm.CalculatePath);

			AlgorithmThread.Start();
			IsCalculating = true;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (IsCalculating && !AlgorithmThread.IsAlive)
			{
				CurrentAlgorithm.Callback(this);
				IsCalculating = false;
			}

			Displayer?.Draw(spriteBatch);
		}

		public List<Vector2> GetPathDrawingPoints(List<GridElement> path)
		{
			if (path == null || path.Count <= 1)
				return null;

			Vector2 gridHalf = new Vector2(Game.Grid.GridSize / 2f, Game.Grid.GridSize / 2f);

			List<Vector2> pathDrawingPoint = new List<Vector2>
			{
				Game.Grid.GetGridVector2(path[0].X, path[0].Y) + gridHalf
			};

			for (int i = 2; i < path.Count; i++)
			{
				GridElement element3 = path[i - 2];
				GridElement element2 = path[i - 1];
				GridElement element1 = path[i];
				//pathDrawingPoint.Add(Game.Grid.GetGridVector2(element1.X, element1.Y) + gridHalf);

				if (Utilities.IsCollinear(element1.X, element1.Y, element2.X, element2.Y, element3.X, element3.Y))
					continue;

				pathDrawingPoint.Add(Game.Grid.GetGridVector2(element2.X, element2.Y) + gridHalf);
				pathDrawingPoint.Add(Game.Grid.GetGridVector2(element1.X, element1.Y) + gridHalf);
				i++;
			}
			pathDrawingPoint.Add(Game.Grid.GetGridVector2(path[path.Count-1].X, path[path.Count-1].Y) + gridHalf);
			return pathDrawingPoint;
		}
	}

	public enum AlgorithmType
	{
		Dijkstra, AStar
	}
}
