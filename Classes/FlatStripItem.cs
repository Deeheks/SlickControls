﻿using System;
using System.Drawing;

namespace SlickControls
{
	public class SlickStripItem
	{
		public delegate void action();

		public string Text { get; set; }
		public Bitmap Image { get; set; }
		public DynamicIcon ImageName { get; set; }
		public Action<SlickStripItem> Action { get; set; }
		public bool Fade { get; set; }
		public bool Show { get; set; }
		public bool CloseOnClick { get; set; }
		public int Tab { get; set; }
		public bool IsEmpty => string.IsNullOrWhiteSpace(Text) && Image == null;

		internal bool IsOpenable { get; set; }
		internal bool IsOpened { get; set; }
		internal bool IsContent { get; set; }
		internal bool IsFocused { get; set; }
		internal bool IsVisible { get; set; }
		internal Rectangle DrawRectangle { get; set; }

		public SlickStripItem(string text, action action = null, Bitmap image = null, bool show = true, bool fade = false, int tab = 0, bool closeOnClick = true)
			: this(text, action == null ? (Action<SlickStripItem >)null : (x) => action(), image, show, fade, tab, closeOnClick) { }
	
		
		public SlickStripItem(string text, Action<SlickStripItem> action, Bitmap image = null, bool show = true, bool fade = false, int tab = 0, bool closeOnClick = true)
		{
			Text = text;
			Image = image;
			Action = action;
			Fade = fade;
			Show = show;
			Tab = tab;
			CloseOnClick = closeOnClick;
		}

		public SlickStripItem(string text, DynamicIcon icon, bool show = true, bool fade = false, int tab = 0, bool closeOnClick = true, action action = null)
			: this(text, icon, x => action(), show, fade, tab, closeOnClick) { }

		public SlickStripItem(string text, DynamicIcon icon, Action<SlickStripItem> action, bool show = true, bool fade = false, int tab = 0, bool closeOnClick = true)
		{
			Text = text;
			ImageName = icon;
			Action = action;
			Fade = fade;
			Show = show;
			Tab = tab;
			CloseOnClick = closeOnClick;
		}

		public static SlickStripItem Empty => new SlickStripItem(string.Empty);
	}
}