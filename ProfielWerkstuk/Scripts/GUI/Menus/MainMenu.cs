using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GridManagement;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;

namespace ProfielWerkstuk.Scripts.GUI.Menus
{
	public class MainMenu : UserInterfaceMenu
	{
		public MainMenu(BaseUserInterfaceState state) : base(state, HorizontalAlignment.Center, VerticalAlignment.Center)
		{
			SetupButtons();
		}

		private void SetupButtons()
		{
			MenuContainer containerExit = new MenuContainer(this);
			ExitButton exit = new ExitButton(containerExit, "Exit", State.UiManager.Font24)
			{
				PreferedSize = new Vector2(200, 0),
				Padding = new Vector2(20, 10)
			};

			exit.AddToContainer();
			containerExit.AddToMenu();

			MenuContainer containerClearGrid = new MenuContainer(this);
			ClearGrid clearGridButton = new ClearGrid(containerClearGrid, "Clear grid", State.UiManager.Font24)
			{
				Padding = new Vector2(20, 10)
			};

			clearGridButton.AddToContainer();
			containerClearGrid.AddToMenu();

			MenuContainer mazeContainer = new MenuContainer(this);
			GenerateMazeButton generateMazeButton = new GenerateMazeButton(mazeContainer, "Generate maze", State.UiManager.Font24)
			{
				Padding = new Vector2(20, 10)
			};

			generateMazeButton.AddToContainer();
			mazeContainer.AddToMenu();

			MenuContainer mapSizeContainer = new MenuContainer(this);
			ChangeMapSizeButton mapSizeButton = new ChangeMapSizeButton(mapSizeContainer, "Mapsize: Medium", State.UiManager.Font24)
			{
				Padding = new Vector2(20, 10),
				PreferedSize = new Vector2(320, 0)
			};

			mapSizeButton.AddToContainer();
			mapSizeContainer.AddToMenu();

			MenuContainer keybindingContainer = new MenuContainer(this);
			KeybindingsButton keybindings = new KeybindingsButton(keybindingContainer, "Keybindings", State.UiManager.Font24)
			{
				Padding = new Vector2(20, 10)
			};

			keybindings.AddToContainer();
			keybindingContainer.AddToMenu();

			MenuContainer containerBack = new MenuContainer(this);
			BackButton back = new BackButton(containerBack, "Back", State.UiManager.Font24)
			{
				Padding = new Vector2(20, 10)
			};

			back.AddToContainer();
			containerBack.AddToMenu();
		}
	}

	internal class BackButton : ButtonMenuElement
	{
		public BackButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().BackButtonClicked?.Invoke();
		}
	}

	internal class ExitButton : ButtonMenuElement
	{
		public ExitButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().ExitButtonClicked?.Invoke();
		}
	}

	internal class ClearGrid : ButtonMenuElement
	{
		public ClearGrid(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().ClearGridClicked?.Invoke();
		}
	}

	internal class KeybindingsButton : ButtonMenuElement
	{
		public KeybindingsButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().OpenKeybindings?.Invoke();
		}
	}

	internal class GenerateMazeButton : ButtonMenuElement
	{
		public GenerateMazeButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().GenerateMaze?.Invoke();
		}
	}

	internal class ChangeMapSizeButton : ButtonMenuElement
	{
		public ChangeMapSizeButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{
			GetEventHandlers().MapsizeChanged += MapSizeChanged;
		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().ChangeMapSizeClicked?.Invoke();
		}

		private void MapSizeChanged(MapSize mapSize)
		{
			Text = "Mapsize: " + mapSize;
		}
	}
}
