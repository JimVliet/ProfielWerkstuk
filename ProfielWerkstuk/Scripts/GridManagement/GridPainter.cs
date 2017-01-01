using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProfielWerkstuk.Scripts.GUI;

namespace ProfielWerkstuk.Scripts.GridManagement
{
	public class GridPainter
	{
		private readonly Grid _grid;
		private GridElementType _type;
		private readonly List<GridElementType> _gridTypes;
		private int _index;
		private readonly EventHandlers _eventHandlers;

		public GridPainter(Grid grid, EventHandlers eventHandlers)
		{
			_grid = grid;
			_gridTypes = new List<GridElementType>
			{
				GridElementType.Solid, GridElementType.Road, GridElementType.Forest, GridElementType.River
			};

			_type = _gridTypes[_index];

			eventHandlers.GridPreviewClicked += SwitchType;
			eventHandlers.GetPreviewType += GetPreviewType;
			eventHandlers.PreviewTypeSwitched += PreviewTypeSwitched;

			_eventHandlers = eventHandlers;
		}

		private void SwitchType()
		{
			_eventHandlers.PreviewTypeSwitched?.Invoke();
		}

		private void PreviewTypeSwitched()
		{
			_index = (_index + 1) % _gridTypes.Count;
			_type = _gridTypes[_index];
		}

		private GridElementType GetPreviewType()
		{
			if(_eventHandlers.ShiftHeld != null)
				return _eventHandlers.ShiftHeld.Invoke() ? GridElementType.Empty : _type;
			return _type;
		}

		public void GridClicked(Vector2 clickLocation)
		{
			if (_grid.AlgorithmActive)
				return;

			GridElement element = _grid.GetGridElement(clickLocation);
			if (element == null)
				return;
			element.Type = GetPreviewType();
		}

		public void GridHoldClick(Vector2 mouseLocation)
		{
			if (_grid.AlgorithmActive)
				return;

			GridElement element = _grid.GetGridElement(mouseLocation);
			if (element == null || element.Type == GridElementType.Start || element.Type == GridElementType.End)
				return;
			if (_grid.GridHoldType == GridElementType.Null)
			{
				_grid.GridHoldType = GetPreviewType();
				return;
			}

			element.Type = _grid.GridHoldType;
		}
	}
}
