using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;

namespace ProfielWerkstuk.Scripts.GUI.Menus
{
	public class OptionMenu : UserInterfaceMenu
	{
		public OptionMenu(BaseUserInterfaceState state) : base(state, HorizontalAlignment.Right, VerticalAlignment.Bottom)
		{
			SetupContainers();
		}

		private void SetupContainers()
		{
			Margin = new Vector2(10, 10);

			MenuContainer textContainer = new MenuContainer(this);
			CalculatingText calcText = new CalculatingText(textContainer, new Vector2(), "-", State.UiManager.Font14);
			calcText.AddToContainer();
			textContainer.AddToMenu();

			MenuContainer buttonContainer = new MenuContainer(this);
			DiagonalButton diagonalButton = new DiagonalButton(buttonContainer, "Diagonal", State.UiManager.Font14)
			{
				Padding = new Vector2(10)
			};
			ShowArrowsButton showArrowsButton = new ShowArrowsButton(buttonContainer, "Show neither", State.UiManager.Font14)
			{
				Padding = new Vector2(10)
			};
			Vector2 maxSize = new Vector2(Math.Max(diagonalButton.Size.X, showArrowsButton.Size.X),
				Math.Max(diagonalButton.Size.Y, showArrowsButton.Size.Y));

			diagonalButton.Offset = new Vector2(-maxSize.X/2 - 5, 0);
			diagonalButton.PreferedSize = maxSize;
			showArrowsButton.Offset = new Vector2(maxSize.X/2 + 5, 0);
			showArrowsButton.PreferedSize = maxSize;

			diagonalButton.AddToContainer();
			showArrowsButton.AddToContainer();
			buttonContainer.AddToMenu();
		}
	}

	internal class CalculatingText : TextMenuElement
	{
		public CalculatingText(MenuContainer parentContainer, Vector2 offset, string text, SpriteFont font) : base(parentContainer, offset, text, font)
		{
			GetEventHandlers().CalculateAlgorithm += UpdateCalculatingText;
		}

		private void UpdateCalculatingText(bool calculating, string algorithmName)
		{
			Text = calculating ? "Calculating..." : algorithmName;
		}
	}

	internal class DiagonalButton : ButtonMenuElement
	{
		public DiagonalButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{
			GetEventHandlers().ChangeDiagonalOption += DiagonalOptionChanged;
			MatchToContainer = false;
			ButtonColor = Color.Red;
			ButtonHoverColor = new Color(255, 30, 30);
		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().DiagonalButtonClicked?.Invoke();
		}

		private void DiagonalOptionChanged(bool diagonal)
		{
			if (diagonal)
			{
				ButtonColor = Color.Green;
				ButtonHoverColor = new Color(40, 128, 40);
			}
			else
			{
				ButtonColor = Color.Red;
				ButtonHoverColor = new Color(255, 30, 30);
			}
		}
	}

	internal class ShowArrowsButton : ButtonMenuElement
	{
		public ShowArrowsButton(MenuContainer parentContainer, string text, SpriteFont font) : base(parentContainer, text, font)
		{
			ButtonColor = Color.Red;
			ButtonHoverColor = new Color(255, 30, 30);
			MatchToContainer = false;
			GetEventHandlers().ShowArrows += ShowArrowsOptionChanged;
			GetEventHandlers().ShowInfoText += ShowInfoTextOptionChanged;
		}

		public override void LeftClickEvent()
		{
			GetEventHandlers().ShowArrowsClicked?.Invoke();
		}

		private void ShowArrowsOptionChanged(bool showArrows)
		{
			if (showArrows)
			{
				Text = "Show arrows";
				ButtonColor = Color.Green;
				ButtonHoverColor = new Color(40, 128, 40);
				return;
			}

			Text = "Show neither";
			ButtonColor = Color.Red;
			ButtonHoverColor = new Color(255, 30, 30);
		}

		private void ShowInfoTextOptionChanged(bool showText)
		{
			if (showText)
			{
				Text = "Show info";
				ButtonColor = Color.Green;
				ButtonHoverColor = new Color(40, 128, 40);
				return;
			}

			Text = "Show neither";
			ButtonColor = Color.Red;
			ButtonHoverColor = new Color(255, 30, 30);
		}
	}
}
