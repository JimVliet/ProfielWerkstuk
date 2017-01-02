using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;

namespace ProfielWerkstuk.Scripts.GUI.Menus
{
	public class KeybindingsMenu : UserInterfaceMenu
	{
		public KeybindingsMenu(BaseUserInterfaceState state) : base(state, HorizontalAlignment.Center, VerticalAlignment.Center)
		{
			SetupText();
		}

		private void SetupText()
		{
			AddTextElement("Escape: open/close menus");
			AddTextElement("Tab: switch painting mode");
			AddTextElement("(Hold) Right mouse button: paint on the grid");
			AddTextElement("(Hold) Left mouse button: drag camera around");
			AddTextElement("Left mouse button: paint one gridelement");
			AddTextElement("(Hold) Shift: Clear gridelements");

			MenuContainer backContainer = new MenuContainer(this);
			BackToMainMenuButton backButton = new BackToMainMenuButton(backContainer, "Back", State.UiManager.Font24)
			{
				Padding = new Vector2(20, 10)
			};
			backButton.AddToContainer();
			backContainer.AddToMenu();
		}

		private void AddTextElement(string text)
		{
			MenuContainer container = new MenuContainer(this);
			TextMenuElement textElement = new TextMenuElement(container, new Vector2(-250, 0), text, State.UiManager.Font16)
			{
				Alignment = TextAlignment.Left
			};
			textElement.AddToContainer();
			container.AddToMenu();
		}
	}

	internal class BackToMainMenuButton : ButtonMenuElement
	{
		public BackToMainMenuButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{
			
		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().BackToMainMenuClicked?.Invoke();
		}
	}
}
