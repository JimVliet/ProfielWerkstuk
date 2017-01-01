using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding.AlgorithmDisplayers
{
	public class DisplayerBase
	{
		protected readonly Grid Grid;
		protected readonly ProfielWerkstuk Game;
		protected List<ResultInfo> ResultInfo;
		protected List<Vector2> PathDrawingPoints;

		protected int Explored;

		protected DisplayerBase(ProfielWerkstuk game)
		{
			Grid = game.Grid;
			Game = game;
			PathDrawingPoints = null;
			ResultInfo = null;

			game.EventHandlers.FastForwardStart += FastForwardStart;
			game.EventHandlers.FastForwardEnd += FastForwardEnd;
			game.EventHandlers.FastBackwardStart += FastBackwardStart;
			game.EventHandlers.FastBackwardEnd += FastBackwardEnd;
			game.EventHandlers.SkipToStartClicked += SkipToStart;
			game.EventHandlers.SkipToEndClicked += SkipToEnd;
		}

		public int GetExplored()
		{
			return Explored;
		}

		public virtual void Update(GameTime gameTime)
		{
			
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if(ResultInfo == null)
				return;

			if (PathDrawingPoints != null && PathDrawingPoints.Count > 1 
				&& Game.AlgorithmManager.DisplayerEndedAnimating)
				DrawPath(spriteBatch);

			DrawPathToStart(spriteBatch);
		}

		private void DrawPath(SpriteBatch spriteBatch)
		{
			for (int i = 1; i < PathDrawingPoints.Count; i++)
			{
				spriteBatch.DrawLine(PathDrawingPoints[i - 1], PathDrawingPoints[i], Color.Red, 4f);
			}
		}

		public void Dispose()
		{
			Game.EventHandlers.FastForwardStart -= FastForwardStart;
			Game.EventHandlers.FastForwardEnd -= FastForwardEnd;
			Game.EventHandlers.FastBackwardStart -= FastBackwardStart;
			Game.EventHandlers.FastBackwardEnd -= FastBackwardEnd;
			Game.EventHandlers.SkipToStartClicked -= SkipToStart;
			Game.EventHandlers.SkipToEndClicked -= SkipToEnd;
		}

		private void DrawPathToStart(SpriteBatch spriteBatch)
		{
			if (Game.UserInterface.GetMenu(Game.InputManager.GetMouseLocation()) != null || !Game.UserInterface.AllowClicking())
				return;

			GridElement element = Game.Grid.GetGridElement(Game.InputManager.GetMouseLocation());
			List<GridElement> gridElements = element?.GetPathToStart();
			if(gridElements == null)
				return;

			List<Vector2> drawingPath = Game.AlgorithmManager.GetPathDrawingPoints(gridElements);
			if(drawingPath == null)
				return;

			for (int i = 1; i < drawingPath.Count; i++)
			{
				spriteBatch.DrawLine(drawingPath[i - 1], drawingPath[i], Color.Red * 0.8f, 4f);
			}
		}

		public virtual void UpdateDisplayer(List<Vector2> pathDrawingPoints, List<ResultInfo> resultInfo)
		{

		}

		public void CleanUp()
		{
			foreach (GridElement element in Grid.GetGridMap())
			{
				element.ClearResultInfo();
			}
		}

		protected virtual void FastForwardStart()
		{
			
		}

		protected virtual void FastForwardEnd()
		{
			
		}

		protected virtual void FastBackwardStart()
		{
			
		}

		protected virtual void FastBackwardEnd()
		{
			
		}

		protected virtual void SkipToStart()
		{
			
		}

		protected virtual void SkipToEnd()
		{
			
		}
	}
}
