using ProfielWerkstuk.Scripts.GUI.Menus;

namespace ProfielWerkstuk.Scripts.GUI.States
{
	public class KeybindingsState : BaseUserInterfaceState
	{
		public KeybindingsState(ProfielWerkstuk game, UserInterfaceManager uiManager) : base(game, uiManager, UserInterfaceStates.Keybindings)
		{
			AllowClicking = false;
			GetEventHandlers().BackToMainMenuClicked += BackToMainMenu;
			GetEventHandlers().OpenKeybindings += OpenKeybindings;
		}

		public override void Setup()
		{
			KeybindingsMenu keybindings = new KeybindingsMenu(this);
			keybindings.AddToState();
		}

		private void OpenKeybindings()
		{
			UiManager.SwitchToState(UserInterfaceStates.Keybindings);
		}

		private void BackToMainMenu()
		{
			UiManager.SwitchToState(UserInterfaceStates.MainMenu);
		}
	}
}
