using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;

namespace ProfielWerkstuk.Scripts.GUI.Menus
{
	public class MainMenu : UserInterfaceMenu
	{
		public MainMenu(BaseUserInterfaceState state, Vector2 pos) : base(state, pos)
		{
			SetupButtons();
		}

		public MainMenu(BaseUserInterfaceState state) : base(state, HorizontalAlignment.Center, VerticalAlignment.Center)
		{
			SetupButtons();
		}

		private void SetupButtons()
		{
			MenuContainer containerExit = new MenuContainer(this);
			ExitButton exit = new ExitButton(containerExit, "Exit", State.UiManager.Font24)
			{
				PreferedSize = new Vector2(200, 30)
			};

			exit.AddToContainer();
			containerExit.AddToMenu();

			MenuContainer containerBack = new MenuContainer(this);
			BackButton back = new BackButton(containerBack, "Back", State.UiManager.Font24);

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
}
