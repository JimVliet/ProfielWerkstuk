using Microsoft.Xna.Framework;
using ProfielWerkstuk.Scripts.GUI;

namespace ProfielWerkstuk.Scripts.Events
{
	public delegate void MenuActivated(Menu menu);

	public delegate void SizeUpdate(InfoTextElement element);

	public delegate void ClickEvent(IMenuItem item, Vector2 clickLocation);

	public delegate void CalculatingEvent(bool isCalculating);
}
