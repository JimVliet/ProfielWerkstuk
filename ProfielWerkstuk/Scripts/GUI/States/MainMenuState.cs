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
			Game.EventHandlers.OpenMainMenu += EscapeButtonClick;
		}

		public override void Setup()
		{
			MainMenu menu = new MainMenu(this);
			menu.AddToState();
		}

		private void BackButtonClick()
		{
			UiManager.SwitchToState(UserInterfaceStates.GridMap);
		}

		private void ExitButtonClick()
		{
			Game.Exit();
		}

		private void EscapeButtonClick()
		{
			UiManager.SwitchToState(UserInterfaceStates.MainMenu);
		}
	}
}
