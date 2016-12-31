using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
}
