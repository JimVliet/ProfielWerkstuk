using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;

namespace ProfielWerkstuk.Scripts.GUI.Menus
{
	public class AlgorithmMenu : UserInterfaceMenu
	{
		public AlgorithmMenu(BaseUserInterfaceState state) : base(state, HorizontalAlignment.Right, VerticalAlignment.Top)
		{
			SetupMenu();
		}

		private void SetupMenu()
		{
			MenuContainer dijkstraContainer = new MenuContainer(this);

			DijkstraButton dijkstra = new DijkstraButton(dijkstraContainer, "Dijkstra", State.UiManager.Font16)
			{
				PreferedSize = new Vector2(160, 20),
				Padding = new Vector2(10, 10)
			};

			dijkstra.AddToContainer();
			dijkstraContainer.AddToMenu();

			MenuContainer bfsContainer = new MenuContainer(this);

			BfsButton bfs = new BfsButton(bfsContainer, "Breadth-first search", State.UiManager.Font16)
			{
				Padding = new Vector2(10, 10)
			};

			bfs.AddToContainer();
			bfsContainer.AddToMenu();

			MenuContainer dfsContainer = new MenuContainer(this);

			DfsButton dfs = new DfsButton(dfsContainer, "Depth-first search", State.UiManager.Font16)
			{
				Padding = new Vector2(10, 10)
			};

			dfs.AddToContainer();
			dfsContainer.AddToMenu();

			MenuContainer greedyBfsContainer = new MenuContainer(this);
			GreedyBfsButton greedyBfs = new GreedyBfsButton(greedyBfsContainer, "Greedy best-first search", State.UiManager.Font16)
			{
				Padding = new Vector2(10, 10)
			};

			greedyBfs.AddToContainer();
			greedyBfsContainer.AddToMenu();

			MenuContainer aStarContainer = new MenuContainer(this);
			AStarButton aStarButton = new AStarButton(aStarContainer, "A*", State.UiManager.Font16)
			{
				Padding = new Vector2(10, 10)
			};

			aStarButton.AddToContainer();
			aStarContainer.AddToMenu();
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

	public class BfsButton : ButtonMenuElement
	{
		public BfsButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().CalculateBfs?.Invoke();
		}
	}

	public class DfsButton : ButtonMenuElement
	{
		public DfsButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().CalculateDfs?.Invoke();
		}
	}

	public class GreedyBfsButton : ButtonMenuElement
	{
		public GreedyBfsButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().CalculateGreedyBfs?.Invoke();
		}
	}

	public class AStarButton : ButtonMenuElement
	{
		public AStarButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().CalculateAStar?.Invoke();
		}
	}
}
