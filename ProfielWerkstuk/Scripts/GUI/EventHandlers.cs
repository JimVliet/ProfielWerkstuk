﻿using System.Windows.Forms;
using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.GUI
{
	public class EventHandlers
	{
		public ButtonClicked CalculateDijkstra;
		public ButtonClicked CalculateBfs;
		public ButtonClicked CalculateDfs;
		public ButtonClicked CalculateGreedyBfs;
		public ButtonClicked CalculateAStar;

		public ButtonClicked OpenMainMenu;
		public ButtonClicked DiagonalButtonClicked;
		public ButtonClicked PlayPauseButtonClicked;
		public ButtonClicked BackButtonClicked;
		public ButtonClicked ExitButtonClicked;
		public ButtonClicked FastForwardStart;
		public ButtonClicked FastBackwardStart;
		public ButtonClicked FastForwardEnd;
		public ButtonClicked FastBackwardEnd;
		public ButtonClicked SkipToStartClicked;
		public ButtonClicked SkipToEndClicked;
		public ButtonClicked ShowArrowsClicked;
		public ButtonClicked ClearGridClicked;
		public ButtonClicked GridPreviewClicked;
		public ButtonClicked BackToMainMenuClicked;
		public ButtonClicked OpenKeybindings;
		public ButtonClicked ChangeMapSizeClicked;

		public TextUpdate TextUpdate;
		public CalculateAlgorithm CalculateAlgorithm;
		public ChangeDiagonalOption ChangeDiagonalOption;
		public PlayPauseEvent PlayPauseEvent;
		public ShowArrows ShowArrows;
		public ShowInfoText ShowInfoText;
		public GetPreviewType GetPreviewType;
		public PreviewTypeSwitched PreviewTypeSwitched;
		public ShiftHeld ShiftHeld;
		public GenerateMaze GenerateMaze;
		public ResetDisplayer ResetDisplayer;
		public MapSizeChanged MapsizeChanged;
	}
	public delegate void TextUpdate(GridElement element, int explored);

	public delegate void CalculateAlgorithm(bool calculating, string algorithmName);

	public delegate void ChangeDiagonalOption(bool diagonal);

	public delegate void PlayPauseEvent(bool isPaused);

	public delegate void ButtonClicked();

	public delegate void PreviewTypeSwitched();

	public delegate void GenerateMaze();

	public delegate void ResetDisplayer();

	public delegate void ShowArrows(bool showArrows);

	public delegate void ShowInfoText(bool showText);

	public delegate void MapSizeChanged(MapSize mapSize);

	public delegate GridElementType GetPreviewType();

	public delegate bool ShiftHeld();
}
