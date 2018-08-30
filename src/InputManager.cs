using System;
using System.Windows;
using System.Windows.Controls;
using HDT.Plugins.Graveyard;
using Hearthstone_Deck_Tracker;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace MulliganWinrate
{
	public class InputManager
	{
		private User32.MouseInput _mouseInput;
		private StackPanel _player;

		private String _selected;

		public InputManager (StackPanel player)
		{
			_player = player;
		}

		public bool Toggle ()
		{
			if (_mouseInput == null)
			{
				_mouseInput = new User32.MouseInput();
				_mouseInput.LmbDown += MouseInputOnLmbDown;
				_mouseInput.LmbUp += MouseInputOnLmbUp;
				_mouseInput.MouseMoved += MouseInputOnMouseMoved;
				return true;
			} else
			{
				_mouseInput.Dispose();
				_mouseInput = null;
				return false;
			}
		}

		public void Dispose () {
			_mouseInput.Dispose();
		}

		private void MouseInputOnLmbDown(object sender, EventArgs eventArgs)
		{
			var pos = User32.GetMousePos();
			var _mousePos = new Point(pos.X, pos.Y);
			if (PointInsideControl(_mousePos, _player))
			{
				_selected = "player";
			} 
		}

		private void MouseInputOnLmbUp(object sender, EventArgs eventArgs)
		{
			var pos = User32.GetMousePos();
			var p = Core.OverlayCanvas.PointFromScreen(new Point(pos.X, pos.Y));
			if (_selected == "player")
			{
				Settings.Default.PlayerTop = p.Y;
				Settings.Default.PlayerLeft = p.X;
			} 

			_selected = null;
		}

		private void MouseInputOnMouseMoved(object sender, EventArgs eventArgs)
		{
			if (_selected == null)
			{
				return;
			}

			var pos = User32.GetMousePos();
			var p = Core.OverlayCanvas.PointFromScreen(new Point(pos.X, pos.Y));
			if (_selected == "player")
			{
				Canvas.SetTop(_player, p.Y);
				Canvas.SetLeft(_player, p.X);
			} 
		}

		private bool PointInsideControl(Point p, FrameworkElement control) {
			var pos = control.PointFromScreen(p);
			return pos.X > 0 && pos.X < control.ActualWidth && pos.Y > 0 && pos.Y < control.ActualHeight;
		}
	}
}
