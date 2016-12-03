using Microsoft.Xna.Framework;
using ProfielWerkstuk.Scripts.Utility;

namespace ProfielWerkstuk.Scripts.GUI.States
{
	public class MainMenuState : BaseUserInterfaceState
	{
		public MainMenuState(Game1 game) : base(game, UserInterfaceStates.MainMenu)
		{
			AllowClicking = false;
		}

		public new void Setup()
		{
			SetupMainMenu();
		}

		private void SetupMainMenu()
		{
			float windowWidth = Utilities.GetWindowWidth(Game);
			float windowHeight = Utilities.GetWindowHeight(Game);

			//Setup mainmenu
			Menu mainMenu = CreateMenu(new Vector2(windowWidth / 2f, windowHeight / 2f));
			Game.InputManager.EscapeTriggerList.Add(mainMenu);

			Button exitButton = new Button(Game.UserInterface.Font28, new Vector2(), "Exit")
			{
				ClickedEvent = ExitButtonClick
			};
			exitButton.Size = new Vector2(250f, exitButton.Size.Y);

			Button backButton = new Button(Game.UserInterface.Font28, new Vector2(), "Back")
			{
				ClickedEvent = BackButtonClick
			};

			mainMenu.AddMenuItem(exitButton);
			mainMenu.AddMenuItem(backButton);
		}

		private void BackButtonClick(IMenuItem item, Vector2 location)
		{
			Game.UserInterface.SwitchToState(UserInterfaceStates.GridMap);
		}

		private void ExitButtonClick(IMenuItem item, Vector2 location)
		{
			Game.Exit();
		}
	}
}
