using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;

namespace ProfielWerkstuk.Scripts.GUI.Menus
{
	public class EscapeButtonMenu : UserInterfaceMenu
	{
		public EscapeButtonMenu(BaseUserInterfaceState state, Vector2 pos) : base(state, pos)
		{
			SetupButton();
		}

		public EscapeButtonMenu(BaseUserInterfaceState state) : base(state, HorizontalAlignment.Left, VerticalAlignment.Top)
		{
			SetupButton();
		}

		private void SetupButton()
		{
			Margin = new Vector2(5,5);
			MenuContainer container = new MenuContainer(this);
			MenuButton menuButton = new MenuButton(container, "Menu", State.UiManager.Font16)
			{
				Padding = new Vector2(5, 5)
			};

			menuButton.AddToContainer();
			container.AddToMenu();
		}
	}

	public class MenuButton : ButtonMenuElement
	{
		public MenuButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{

		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().OpenMainMenu?.Invoke();
		}
	}
}
