using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;

namespace ProfielWerkstuk.Scripts.GUI.Menus
{
	public class AlgorithmMenu : UserInterfaceMenu
	{
		public AlgorithmMenu(BaseUserInterfaceState state, Vector2 pos) : base(state, pos)
		{
			SetupMenu();
		}

		public AlgorithmMenu(BaseUserInterfaceState state) : base(state, HorizontalAlignment.Right, VerticalAlignment.Top)
		{
			SetupMenu();
		}

		private void SetupMenu()
		{
			MenuContainer container = new MenuContainer(this);

			DijkstraButton dijkstra = new DijkstraButton(container, "Dijkstra", State.UiManager.Font16)
			{
				PreferedSize = new Vector2(160, 20),
				Padding = new Vector2(10, 10)
			};

			dijkstra.AddToContainer();
			container.AddToMenu();
		}
	}

	public class DijkstraButton : ButtonMenuElement
	{
		public DijkstraButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().CalculateDijkstra?.Invoke();
		}
	}
}
