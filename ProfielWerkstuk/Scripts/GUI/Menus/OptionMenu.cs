﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProfielWerkstuk.Scripts.GUI.BaseClasses;
using ProfielWerkstuk.Scripts.GUI.States;

namespace ProfielWerkstuk.Scripts.GUI.Menus
{
	public class OptionMenu : UserInterfaceMenu
	{
		public OptionMenu(BaseUserInterfaceState state, Vector2 pos) : base(state, pos)
		{
			SetupContainers();
		}

		public OptionMenu(BaseUserInterfaceState state) : base(state, HorizontalAlignment.Right, VerticalAlignment.Bottom)
		{
			SetupContainers();
		}

		private void SetupContainers()
		{
			Margin = new Vector2(10, 10);

			MenuContainer textContainer = new MenuContainer(this);
			CalculatingText calcText = new CalculatingText(textContainer, new Vector2(), "-", State.UiManager.Font16);
			calcText.AddToContainer();
			textContainer.AddToMenu();

			MenuContainer buttonContainer = new MenuContainer(this);
			DiagonalButton diagonalButton = new DiagonalButton(buttonContainer, "Diagonal", State.UiManager.Font16)
			{
				Padding = new Vector2(10, 10)
			};
			ShowArrowsButton showArrowsButton = new ShowArrowsButton(buttonContainer, "Show arrows", State.UiManager.Font16)
			{
				Padding = new Vector2(10, 10)
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
			ButtonColor = Color.Green;
			ButtonHoverColor = new Color(40, 128, 40);
			MatchToContainer = false;
		}
	}
}