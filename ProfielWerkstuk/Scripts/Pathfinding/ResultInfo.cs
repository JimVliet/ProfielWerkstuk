using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.Pathfinding
{
	public class ResultInfo
	{
		private readonly ResultInfoType _type;
		public readonly int X;
		public readonly int Y;
		public readonly GridElement PreviousElement;
		private readonly string _text;
		private readonly string _infoText;

		public ResultInfo(GridElement element, double number, ResultInfoType type, GridElement previous, bool numberIsHeuristic)
		{
			_type = type;
			X = element.X;
			Y = element.Y;
			PreviousElement = previous;

			if (numberIsHeuristic)
				_text = "Heuristic: " + RoundDoubleToString(number, 2);
			else
				_text = "Distance to start: " + RoundDoubleToString(number, 2);

			_infoText = RoundDoubleToString(number, 1);
		}

		public ResultInfo(GridElement element, double distanceSoFar, double heuristic, ResultInfoType type, GridElement previous)
		{
			_type = type;
			X = element.X;
			Y = element.Y;
			PreviousElement = previous;

			_text = "F = " + RoundDoubleToString(distanceSoFar + heuristic, 2) + " = (G) " + 
				RoundDoubleToString(distanceSoFar, 2) + " + (H) " + RoundDoubleToString(heuristic, 2);
			_infoText = RoundDoubleToString(distanceSoFar + heuristic, 1);
		}

		public string GetInfoText()
		{
			return _text;
		}

		public string GetStringForDisplay()
		{
			return _infoText;
		}

		private string RoundDoubleToString(double number, int decimals)
		{
			return Math.Round(number, decimals).ToString(CultureInfo.CurrentCulture);
		}

		public Color GetColor()
		{
			switch (_type)
			{
				case ResultInfoType.Frontier:
					return new Color(255, 196, 107);
				case ResultInfoType.Visited:
					return new Color(140, 188, 219);
				default:
					return Color.Black;
			}
		}

		public void DrawArrow(SpriteBatch spriteBatch, Grid grid, int lineLength, int arrowSize, Color arrowColor)
		{
			if(!HasPrevious())
				return;

			Vector2 gridHalfSize = new Vector2(grid.GridSize/2f);
			Vector2 center = grid.GetGridVector2(X, Y) + gridHalfSize;
			Vector2 deltaLine;

			switch (DirectionToPrevious())
			{
				case ArrowDirection.Top:
					deltaLine = new Vector2(0, -lineLength/2f);
					DrawArrowLine(spriteBatch, center, deltaLine, arrowColor);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(arrowSize), center + deltaLine, arrowColor, 3f);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(-arrowSize, arrowSize), center + deltaLine, arrowColor, 3f);
					return;
				case ArrowDirection.TopRight:
					deltaLine = new Vector2(lineLength / 2f, -lineLength / 2f);
					DrawArrowLine(spriteBatch, center, deltaLine, arrowColor);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(-arrowSize, 0), center + deltaLine, arrowColor, 3f);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(0, arrowSize), center + deltaLine, arrowColor, 3f);
					return;
				case ArrowDirection.Right:
					deltaLine = new Vector2(lineLength / 2f, 0);
					DrawArrowLine(spriteBatch, center, deltaLine, arrowColor);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(-arrowSize), center + deltaLine, arrowColor, 3f);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(-arrowSize, arrowSize), center + deltaLine, arrowColor, 3f);
					return;
				case ArrowDirection.BottomRight:
					deltaLine = new Vector2(lineLength / 2f, lineLength / 2f);
					DrawArrowLine(spriteBatch, center, deltaLine, arrowColor);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(0, -arrowSize), center + deltaLine, arrowColor, 3f);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(-arrowSize, 0), center + deltaLine, arrowColor, 3f);
					return;
				case ArrowDirection.Bottom:
					deltaLine = new Vector2(0, lineLength / 2f);
					DrawArrowLine(spriteBatch, center, deltaLine, arrowColor);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(-arrowSize), center + deltaLine, arrowColor, 3f);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(arrowSize, -arrowSize), center + deltaLine, arrowColor, 3f);
					return;
				case ArrowDirection.BottomLeft:
					deltaLine = new Vector2(-lineLength / 2f, lineLength / 2f);
					DrawArrowLine(spriteBatch, center, deltaLine, arrowColor);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(0, -arrowSize), center + deltaLine, arrowColor, 3f);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(arrowSize, 0), center + deltaLine, arrowColor, 3f);
					return;
				case ArrowDirection.Left:
					deltaLine = new Vector2(-lineLength / 2f, 0);
					DrawArrowLine(spriteBatch, center, deltaLine, arrowColor);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(arrowSize, -arrowSize), center + deltaLine, arrowColor, 3f);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(arrowSize), center + deltaLine, arrowColor, 3f);
					return;
				case ArrowDirection.TopLeft:
					deltaLine = new Vector2(-lineLength / 2f, -lineLength / 2f);
					DrawArrowLine(spriteBatch, center, deltaLine, arrowColor);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(arrowSize, 0), center + deltaLine, arrowColor, 3f);
					spriteBatch.DrawLine(center + deltaLine + new Vector2(0, arrowSize), center + deltaLine, arrowColor, 3f);
					return;
				default:
					return;
			}
		}

		private void DrawArrowLine(SpriteBatch spriteBatch, Vector2 center, Vector2 deltaLine, Color color)
		{
			spriteBatch.DrawLine(center - deltaLine, center + deltaLine, color, 4f);
		}

		private bool HasPrevious()
		{
			return PreviousElement != null;
		}

		/// <summary>
		/// Returns the direction to the previous resultinfo, make sure the previouselement isn't null
		/// </summary>
		/// <returns></returns>
		private ArrowDirection DirectionToPrevious()
		{
			if (PreviousElement.X < X)
			{
				if(PreviousElement.Y < Y)
					return ArrowDirection.TopLeft;

				return PreviousElement.Y > Y ? ArrowDirection.BottomLeft : ArrowDirection.Left;
			}
			if (PreviousElement.X > X)
			{
				if(PreviousElement.Y < Y)
					return ArrowDirection.TopRight;

				return PreviousElement.Y > Y ? ArrowDirection.BottomRight : ArrowDirection.Right;
			}

			return PreviousElement.Y < Y ? ArrowDirection.Top : ArrowDirection.Bottom;
		}
	}

	public enum ResultInfoType
	{
		Visited, Frontier
	}

	public enum ArrowDirection
	{
		Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, TopLeft
	}
}
