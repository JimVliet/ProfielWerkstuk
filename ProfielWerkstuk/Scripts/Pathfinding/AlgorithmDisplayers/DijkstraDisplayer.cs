using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding.AlgorithmDisplayers
{
	public class DijkstraDisplayer : IDisplayer
	{
		private readonly Grid _grid;
		private readonly ProfielWerkstuk _game;
		private List<ResultInfo> _resultInfo;
		private List<Vector2> _pathDrawingPoints;

		private double _distanceCounter;
		private int _infoAddSpeed = 8;
		private int _addSpeedMultiplier = 1;
		private int _updateIndex;
		private int _explored;

		public DijkstraDisplayer(ProfielWerkstuk game)
		{
			_grid = game.Grid;
			_game = game;
			_pathDrawingPoints = null;
			_resultInfo = null;

			game.EventHandlers.FastForwardStart += FastForwardStart;
			game.EventHandlers.FastForwardEnd += FastForwardEnd;
			game.EventHandlers.FastBackwardStart += FastBackwardStart;
			game.EventHandlers.FastBackwardEnd += FastBackwardEnd;
			game.EventHandlers.SkipToStartClicked += SkipToStart;
			game.EventHandlers.SkipToEndClicked += SkipToEnd;
		}

		public int GetExplored()
		{
			return _explored;
		}

		public void Update(GameTime gameTime)
		{
			if (_resultInfo == null || (_game.AlgorithmManager.Paused && _addSpeedMultiplier == 1) || _game.AlgorithmManager.DisplayerEndedAnimating)
				return;

			double newDistanceCounter = Math.Max(GetNewDistance(gameTime), 0);

			UpdateNodes(newDistanceCounter);

			_distanceCounter = newDistanceCounter;

			if (_distanceCounter > _resultInfo[_resultInfo.Count - 1].Distance + 1)
				_game.AlgorithmManager.DisplayerEndedAnimating = true;
		}

		private void UpdateNodes(double newDistanceCounter)
		{
			int oldDistance = (int) _distanceCounter;
			int newDistance = (int) newDistanceCounter;

			if (oldDistance == newDistance)
				return;

			GridElement[,] gridMap = _grid.GetGridMap();

			if (gridMap == null)
				return;

			ResultInfo info = _resultInfo[_updateIndex];

			if (oldDistance < newDistance)
			{
				//Make sure the last element isn't being added multiple times
				if (_updateIndex == _resultInfo.Count - 1 && gridMap[info.Y, info.X].HasResultInfo())
					return;

				while (info.Distance - info.GetExtraDistance() < newDistance)
				{
					GridElement element = gridMap[info.Y, info.X];

					if (!element.HasResultInfo())
						_explored++;

					element.AttachResultInfo(info);

					if (_updateIndex + 1 == _resultInfo.Count)
						break;

					_updateIndex++;
					info = _resultInfo[_updateIndex];
				}
			}
			else
			{
				while ((int)info.Distance - (int)info.GetExtraDistance() >= newDistance - 1)
				{
					GridElement element = gridMap[info.Y, info.X];

					if (element.RemoveLastInfo() && !element.HasResultInfo())
						_explored--;

					if (_updateIndex == 0)
						break;

					_updateIndex--;
					info = _resultInfo[_updateIndex];
				}
				if (_updateIndex != 0)
					_updateIndex++;
			}
		}

		private double GetNewDistance(GameTime gameTime)
		{
			return _distanceCounter + gameTime.ElapsedGameTime.Milliseconds/1000d * _infoAddSpeed * _addSpeedMultiplier;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			if (_pathDrawingPoints == null || _pathDrawingPoints.Count <= 1 || _resultInfo == null)
				return;

			if((int)_distanceCounter == (int)_resultInfo[_resultInfo.Count - 1].Distance + 1)
				DrawPath(spriteBatch);

			DrawPathToStart(spriteBatch);
		}

		private void DrawPath(SpriteBatch spriteBatch)
		{
			for (int i = 1; i < _pathDrawingPoints.Count; i++)
			{
				spriteBatch.DrawLine(_pathDrawingPoints[i - 1], _pathDrawingPoints[i], Color.Orange, 4f);
			}
		}

		private void DrawPathToStart(SpriteBatch spriteBatch)
		{
			if (_game.UserInterface.GetMenu(_game.InputManager.GetMouseLocation()) != null || !_game.UserInterface.AllowClicking())
				return;

			GridElement element = _game.Grid.GetGridElement(_game.InputManager.GetMouseLocation());
			List<GridElement> gridElements = element?.GetPathToStart();
			if(gridElements == null)
				return;

			List<Vector2> drawingPath = _game.AlgorithmManager.GetPathDrawingPoints(gridElements);
			if(drawingPath == null)
				return;

			for (int i = 1; i < drawingPath.Count; i++)
			{
				spriteBatch.DrawLine(drawingPath[i - 1], drawingPath[i], Color.Red * 0.8f, 4f);
			}
		}

		public void UpdateDisplayer(List<Vector2> pathDrawingPoints, List<ResultInfo> resultInfo)
		{
			resultInfo = resultInfo.OrderBy(info => info.Distance - info.GetExtraDistance()).ToList();

			_pathDrawingPoints = pathDrawingPoints;
			_resultInfo = resultInfo;
			_updateIndex = 0;
			_distanceCounter = 0;
			_explored = 0;
		}

		public void CleanUp()
		{
			foreach (GridElement element in _grid.GetGridMap())
			{
				element.ClearResultInfo();
			}
		}

		private void FastForwardStart()
		{
			if(!_game.AlgorithmManager.DisplayerEndedAnimating)
				_addSpeedMultiplier = 3;
		}

		private void FastForwardEnd()
		{
			_addSpeedMultiplier = 1;
		}

		private void FastBackwardStart()
		{
			_addSpeedMultiplier = -3;
			_game.AlgorithmManager.DisplayerEndedAnimating = false;
		}

		private void FastBackwardEnd()
		{
			_addSpeedMultiplier = 1;
		}

		private void SkipToStart()
		{
			CleanUp();
			_updateIndex = 0;
			_distanceCounter = 0;
			_explored = 0;
			_game.AlgorithmManager.DisplayerEndedAnimating = false;
			_game.EventHandlers.PlayPauseEvent?.Invoke(false);
		}

		private void SkipToEnd()
		{
			UpdateNodes(_resultInfo[_resultInfo.Count - 1].Distance + 1);
			_distanceCounter = _resultInfo[_resultInfo.Count - 1].Distance + 1;
			_game.AlgorithmManager.DisplayerEndedAnimating = true;
		}
	}
}
