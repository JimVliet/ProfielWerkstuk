using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GridManagement;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;

namespace ProfielWerkstuk.Scripts.GUI.Menus
{
	public class ToolTipsMenu : UserInterfaceMenu
	{
		public ToolTipsMenu(BaseUserInterfaceState state, Vector2 pos) : base(state, pos)
		{
			SetupTextElements();
		}

		public ToolTipsMenu(BaseUserInterfaceState state) : base(state, HorizontalAlignment.Left, VerticalAlignment.Bottom)
		{
			SetupTextElements();
		}

		private void SetupTextElements()
		{
			Margin = new Vector2(10, 10);
			BaseButtonDistance = 0;

			MenuContainer menuContainer = new MenuContainer(this);
			DistanceInfo distanceInfo = new DistanceInfo(menuContainer, new Vector2(-130, -48), 
				"Distance to start: -", State.UiManager.Font16);
			distanceInfo.AddToContainer();

			PosInfo posInfo = new PosInfo(menuContainer, new Vector2(-130, -16),
				"Position: -", State.UiManager.Font16);
			posInfo.AddToContainer();

			NodesExploredInfo nodesExplored = new NodesExploredInfo(menuContainer, 
				new Vector2(-130, 16), "Nodes explored: -", State.UiManager.Font16);
			nodesExplored.AddToContainer();

			PercentExploredInfo percentExplored = new PercentExploredInfo(menuContainer, new Vector2(-130, 48), 
				"Nodes explored: -", State.UiManager.Font16, State.Game.Grid);
			percentExplored.AddToContainer();
			
			menuContainer.AddToMenu();
		}
	}

	internal class DistanceInfo : TextMenuElement
	{
		public DistanceInfo(MenuContainer parentContainer, Vector2 offset, string text, SpriteFont font) : base(parentContainer, offset, text, font)
		{
			GetEventHandlers().TextUpdate += UpdateDistance;
			Alignment = TextAlignment.Left;
		}

		private void UpdateDistance(GridElement element, int explored)
		{
			Text = "Distance to start: " + (element?.GetResultInfo() == null ? "-"
					: Math.Round(element.GetResultInfo().Distance, 2).ToString(CultureInfo.CurrentCulture));
		}
	}

	internal class PosInfo : TextMenuElement
	{
		public PosInfo(MenuContainer parentContainer, Vector2 offset, string text, SpriteFont font) : base(parentContainer, offset, text, font)
		{
			GetEventHandlers().TextUpdate += UpdatePosText;
			Alignment = TextAlignment.Left;
		}

		private void UpdatePosText(GridElement element, int explored)
		{
			Text = "Position: " + (element == null ? "-" : "(" + element.X + ", " + element.Y + ")");
		}
	}

	internal class NodesExploredInfo : TextMenuElement
	{
		public NodesExploredInfo(MenuContainer parentContainer, Vector2 offset, string text, SpriteFont font) : base(parentContainer, offset, text, font)
		{
			GetEventHandlers().TextUpdate += UpdateNodesExplored;
			Alignment = TextAlignment.Left;
		}

		private void UpdateNodesExplored(GridElement element, int explored)
		{
			Text = "Nodes explored: " + (explored == 0 ? "-" : explored.ToString());
		}
	}

	internal class PercentExploredInfo : TextMenuElement
	{
		private readonly Grid _grid;

		public PercentExploredInfo(MenuContainer parentContainer, Vector2 offset, string text, 
			SpriteFont font, Grid grid) : base(parentContainer, offset, text, font)
		{
			_grid = grid;
			GetEventHandlers().TextUpdate += UpdatePercentExplored;
			Alignment = TextAlignment.Left;
		}

		private void UpdatePercentExplored(GridElement element, int explored)
		{
			Text = "Percent explored: " + (explored == 0 ? "-"
					: Math.Round((float)explored / GetGridArea() * 100, 1).ToString(CultureInfo.CurrentCulture) + "%");
		}

		private int GetGridArea()
		{
			return _grid.HalfWidth*_grid.HalfHeight*4;
		}
	}
}
