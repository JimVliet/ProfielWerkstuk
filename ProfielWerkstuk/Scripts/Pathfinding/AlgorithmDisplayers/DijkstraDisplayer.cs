using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding.AlgorithmDisplayers
{
	public class DijkstraDisplayer : IDisplayer
	{
		private List<Vector2> _pathDrawingPoints;
		public Grid Grid;
		private List<ResultInfo> _resultInfo;
		private double _infoCounter;
		public int InfoAddSpeed = 150;

		public DijkstraDisplayer(Grid grid)
		{
			Grid = grid;
			_pathDrawingPoints = null;
			_resultInfo = null;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			if (_pathDrawingPoints == null || _pathDrawingPoints.Count <= 1 || _resultInfo == null)
				return;

			_infoCounter += gameTime.ElapsedGameTime.Milliseconds/1000d * InfoAddSpeed + Math.Sqrt(0.5 * _infoCounter);
			bool maxReached = _infoCounter > _resultInfo.Count;
			_infoCounter = maxReached ? _resultInfo.Count : _infoCounter;

			for (int i = 0; i < (int)_infoCounter; i++)
			{
				ResultInfo info = _resultInfo[i];
				if (Grid.GetGridMap()?[info.Y, info.X].Type == GridElementType.Empty)
					spriteBatch.FillRectangle(Grid.GetGridVector2(info.X, info.Y), new Vector2(Grid.GridSize, Grid.GridSize), info.GetColor());
			}

			if(maxReached)
				DrawPath(spriteBatch);
		}

		private void DrawPath(SpriteBatch spriteBatch)
		{
			for (int i = 1; i < _pathDrawingPoints.Count; i++)
			{
				spriteBatch.DrawLine(_pathDrawingPoints[i - 1], _pathDrawingPoints[i], Color.Orange, 4f);
			}
		}

		public void UpdateDisplayer(List<Vector2> pathDrawingPoints, List<ResultInfo> resultInfo)
		{
			_pathDrawingPoints = pathDrawingPoints;
			_resultInfo = resultInfo;
			_infoCounter = 0;
		}
	}
}
