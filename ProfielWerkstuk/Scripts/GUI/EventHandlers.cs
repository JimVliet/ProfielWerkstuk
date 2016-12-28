using ProfielWerkstuk.Scripts.GridManagement;

namespace ProfielWerkstuk.Scripts.GUI
{
	public class EventHandlers
	{
		public ButtonClicked CalculateDijkstra;
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

		public TextUpdate TextUpdate;
		public CalculateAlgorithm CalculateAlgorithm;
		public ChangeDiagonalOption ChangeDiagonalOption;
		public PlayPauseEvent PlayPauseEvent;
	}
	public delegate void TextUpdate(GridElement element, int explored);

	public delegate void CalculateAlgorithm(bool calculating, string algorithmName);

	public delegate void ChangeDiagonalOption(bool diagonal);

	public delegate void PlayPauseEvent(bool isPaused);

	public delegate void ButtonClicked();
}
