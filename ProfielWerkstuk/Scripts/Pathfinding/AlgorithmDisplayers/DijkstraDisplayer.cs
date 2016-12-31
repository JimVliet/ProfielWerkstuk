using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding.AlgorithmDisplayers
{
	public class DijkstraDisplayer : DisplayerBase
	{
		private double _distanceCounter;
		private int _infoAddSpeed = 8;
		private int _addSpeedMultiplier = 1;
		private int _updateIndex;

		public DijkstraDisplayer(ProfielWerkstuk game) : base(game)
		{

		}

		public override void Update(GameTime gameTime)
		{
			if (ResultInfo == null || (Game.AlgorithmManager.Paused && _addSpeedMultiplier == 1) || Game.AlgorithmManager.DisplayerEndedAnimating)
				return;

			double newDistanceCounter = Math.Max(GetNewDistance(gameTime), 0);

			UpdateNodes(newDistanceCounter);

			_distanceCounter = newDistanceCounter;

			if (_distanceCounter > ResultInfo[ResultInfo.Count - 1].Distance + 1)
				Game.AlgorithmManager.DisplayerEndedAnimating = true;
		}

		private void UpdateNodes(double newDistanceCounter)
		{
			int oldDistance = (int) _distanceCounter;
			int newDistance = (int) newDistanceCounter;

			if (oldDistance == newDistance)
				return;

			GridElement[,] gridMap = Grid.GetGridMap();

			if (gridMap == null)
				return;

			ResultInfo info = ResultInfo[_updateIndex];

			if (oldDistance < newDistance)
			{
				//Make sure the last element isn't being added multiple times
				if (_updateIndex == ResultInfo.Count - 1 && gridMap[info.Y, info.X].HasResultInfo())
					return;

				while (info.Distance - info.GetExtraDistance() < newDistance)
				{
					GridElement element = gridMap[info.Y, info.X];

					if (!element.HasResultInfo())
						Explored++;

					element.AttachResultInfo(info);

					if (_updateIndex + 1 == ResultInfo.Count)
						break;

					_updateIndex++;
					info = ResultInfo[_updateIndex];
				}
			}
			else
			{
				while ((int)info.Distance - (int)info.GetExtraDistance() >= newDistance - 1)
				{
					GridElement element = gridMap[info.Y, info.X];

					if (element.RemoveLastInfo() && !element.HasResultInfo())
						Explored--;

					if (_updateIndex == 0)
						break;

					_updateIndex--;
					info = ResultInfo[_updateIndex];
				}
				if (_updateIndex != 0)
					_updateIndex++;
			}
		}

		private double GetNewDistance(GameTime gameTime)
		{
			return _distanceCounter + gameTime.ElapsedGameTime.Milliseconds/1000d * _infoAddSpeed * _addSpeedMultiplier;
		}

		public override void UpdateDisplayer(List<Vector2> pathDrawingPoints, List<ResultInfo> resultInfo)
		{
			resultInfo = resultInfo.OrderBy(info => info.Distance - info.GetExtraDistance()).ToList();

			PathDrawingPoints = pathDrawingPoints;
			ResultInfo = resultInfo;
			_updateIndex = 0;
			_distanceCounter = 0;
			Explored = 0;
		}

		protected override void FastForwardStart()
		{
			if(!Game.AlgorithmManager.DisplayerEndedAnimating)
				_addSpeedMultiplier = 3;
		}

		protected override void FastForwardEnd()
		{
			_addSpeedMultiplier = 1;
		}

		protected override void FastBackwardStart()
		{
			_addSpeedMultiplier = -3;
			Game.AlgorithmManager.DisplayerEndedAnimating = false;
		}

		protected override void FastBackwardEnd()
		{
			_addSpeedMultiplier = 1;
		}

		protected override void SkipToStart()
		{
			CleanUp();
			_updateIndex = 0;
			_distanceCounter = 0;
			Explored = 0;
			Game.AlgorithmManager.DisplayerEndedAnimating = false;
			Game.EventHandlers.PlayPauseEvent?.Invoke(false);
		}

		protected override void SkipToEnd()
		{
			UpdateNodes(ResultInfo[ResultInfo.Count - 1].Distance + 1);
			_distanceCounter = ResultInfo[ResultInfo.Count - 1].Distance + 1;
			Game.AlgorithmManager.DisplayerEndedAnimating = true;
		}
	}
}
