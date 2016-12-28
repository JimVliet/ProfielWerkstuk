using ProfielWerkstuk.Scripts.GUI.Menus;

namespace ProfielWerkstuk.Scripts.GUI.States
{
	public class MainMenuState : BaseUserInterfaceState
	{
		public MainMenuState(ProfielWerkstuk game, UserInterfaceManager manager) : base(game, manager, UserInterfaceStates.MainMenu)
		{
			AllowClicking = false;
			Game.EventHandlers.BackButtonClicked += BackButtonClick;
			Game.EventHandlers.ExitButtonClicked += ExitButtonClick;
		}

		public override void Setup()
		{
			MainMenu menu = new MainMenu(this);
			menu.AddToState();
		}

		private void BackButtonClick()
		{
			Game.UserInterface.SwitchToState(UserInterfaceStates.GridMap);
		}

		private void ExitButtonClick()
		{
			Game.Exit();
		}
	}
}
