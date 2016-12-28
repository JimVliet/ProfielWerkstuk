using System.Collections.Generic;
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
		public readonly IDisplayer Displayer;
		private readonly ProfielWerkstuk _game;
		private Thread _algorithmThread;
		private IAlgorithm _currentAlgorithm;
		private bool _allowDiagonal;

		private bool _paused;
		public bool Paused
		{
			get { return _paused; }
			private set
			{
				_paused = value;

				if (!value && DisplayerEndedAnimating)
				{
					_game.EventHandlers.CalculateDijkstra?.Invoke();
				}
			}
		}

		private bool _displayerEndedAnimating;
		public bool DisplayerEndedAnimating
		{
			get{ return _displayerEndedAnimating; }
			set
			{
				_displayerEndedAnimating = value;
				if(value)
					_game.EventHandlers.PlayPauseEvent?.Invoke(true);
			}
		}

		private bool _isCalculating;
		private bool IsCalculating
		{
			get { return _isCalculating; }
			set
			{
				_isCalculating = value;
				_game.Grid.AlgorithmActive = value;
				_game.EventHandlers.CalculateAlgorithm?.Invoke(value, _currentAlgorithm.GetName());
			}
		}

		public AlgorithmManager(ProfielWerkstuk game)
		{
			_game = game;
			Displayer = new DijkstraDisplayer(_game);
			_game.EventHandlers.CalculateDijkstra += Calculate;
			_game.EventHandlers.DiagonalButtonClicked += DiagonalButtonClicked;
			_game.EventHandlers.PlayPauseButtonClicked += PlayPauseButtonClicked;
			_game.EventHandlers.PlayPauseEvent += PlayPauseEvent;
		}

		public int GetExplored()
		{
			return Displayer.GetExplored();
		}

		private void Calculate()
		{
			if(_algorithmThread != null && _algorithmThread.IsAlive)
				return;

			_currentAlgorithm = new Dijkstra(_game.Grid.GetGridMap(), _game.Grid.GetStartElement(), _allowDiagonal);
			_algorithmThread = new Thread(_currentAlgorithm.CalculatePath);

			IsCalculating = true;
			_algorithmThread.Start();
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			Displayer?.Draw(spriteBatch, gameTime);
		}

		public void Update(GameTime gameTime)
		{
			if (IsCalculating && !_algorithmThread.IsAlive)
			{
				_currentAlgorithm.Callback(this);
				IsCalculating = false;
				Displayer.CleanUp();

				DisplayerEndedAnimating = false;
				_game.EventHandlers.PlayPauseEvent?.Invoke(false);
			}

			Displayer?.Update(gameTime);
		}

		public List<Vector2> GetPathDrawingPoints(List<GridElement> path)
		{
			if (path == null || path.Count <= 1)
				return null;

			Vector2 gridHalf = new Vector2(_game.Grid.GridSize / 2f, _game.Grid.GridSize / 2f);

			List<Vector2> pathDrawingPoint = new List<Vector2>
			{
				_game.Grid.GetGridVector2(path[0].X, path[0].Y) + gridHalf
			};

			for (int i = 2; i < path.Count; i++)
			{
				GridElement element3 = path[i - 2];
				GridElement element2 = path[i - 1];
				GridElement element1 = path[i];

				if (Utilities.IsCollinear(element1.X, element1.Y, element2.X, element2.Y, element3.X, element3.Y))
					continue;

				pathDrawingPoint.Add(_game.Grid.GetGridVector2(element2.X, element2.Y) + gridHalf);
				pathDrawingPoint.Add(_game.Grid.GetGridVector2(element1.X, element1.Y) + gridHalf);
				i++;
			}
			pathDrawingPoint.Add(_game.Grid.GetGridVector2(path[path.Count-1].X, path[path.Count-1].Y) + gridHalf);
			return pathDrawingPoint;
		}

		private void DiagonalButtonClicked()
		{
			_allowDiagonal = !_allowDiagonal;
			_game.EventHandlers.ChangeDiagonalOption?.Invoke(_allowDiagonal);
		}

		private void PlayPauseButtonClicked()
		{
			_game.EventHandlers.PlayPauseEvent?.Invoke(!Paused);
		}

		private void PlayPauseEvent(bool paused)
		{
			Paused = paused;
		}
	}

	public enum AlgorithmType
	{
		Dijkstra, AStar
	}
}
