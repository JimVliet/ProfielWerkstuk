using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding.AlgorithmDisplayers
{
	public class AlgorithmDisplayer : DisplayerBase
	{
		private int _infoAddSpeed = 80;
		private int _addSpeedMultiplier = 1;
		private double _updateIndex = -1;

		public AlgorithmDisplayer(ProfielWerkstuk game) : base(game)
		{

		}

		public override void Update(GameTime gameTime)
		{
			if (ResultInfo == null || (Game.AlgorithmManager.Paused && _addSpeedMultiplier == 1) || Game.AlgorithmManager.DisplayerEndedAnimating)
				return;

			double newIndex = Math.Min(Math.Max(GetNewIndex(gameTime), -1), ResultInfo.Count-1);

			UpdateResults(newIndex);

			_updateIndex = newIndex;
			if ((int) _updateIndex >= ResultInfo.Count - 1)
				Game.AlgorithmManager.DisplayerEndedAnimating = true;
		}

		private double GetNewIndex(GameTime gameTime)
		{
			return _updateIndex + gameTime.ElapsedGameTime.Milliseconds/1000d*_infoAddSpeed*_addSpeedMultiplier;
		}

		private void UpdateResults(double newIndexPos)
		{
			int oldIndex = (int) _updateIndex;
			int newIndex = (int) newIndexPos;

			if(oldIndex == newIndex || newIndex >= ResultInfo.Count || newIndex < -1)
				return;

			GridElement[,] gridMap = Grid.GetGridMap();

			if (gridMap == null)
				return;

			if (oldIndex < newIndex)
			{
				for (int i = oldIndex+1; i <= newIndex; i++)
				{
					ResultInfo info = ResultInfo[i];

					GridElement element = gridMap[info.Y, info.X];

					if (!element.HasResultInfo())
						Explored++;

					element.AttachResultInfo(info);
				}
				return;
			}

			for (int i = oldIndex; i > newIndex; i--)
			{
				ResultInfo info = ResultInfo[i];

				GridElement element = gridMap[info.Y, info.X];

				if (element.RemoveLastInfo() && !element.HasResultInfo())
					Explored--;
			}
		}

		public override void UpdateDisplayer(List<Vector2> pathDrawingPoints, List<ResultInfo> resultInfo)
		{
			PathDrawingPoints = pathDrawingPoints;
			ResultInfo = resultInfo;
			_updateIndex = -1;
			Explored = 0;
		}

		protected override void FastForwardStart()
		{
			if (!Game.AlgorithmManager.DisplayerEndedAnimating)
				_addSpeedMultiplier = 5;
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
			_updateIndex = -1;
			Explored = 0;
			Game.AlgorithmManager.DisplayerEndedAnimating = false;
			Game.EventHandlers.PlayPauseEvent?.Invoke(false);
		}

		protected override void SkipToEnd()
		{
			UpdateResults(ResultInfo.Count-1);
			_updateIndex = ResultInfo.Count - 1;
			Game.AlgorithmManager.DisplayerEndedAnimating = true;
		}
	}
}
