﻿using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlickControls
{
	public class SlickStackedListControl<T> : SlickControl
	{
		private readonly List<DrawableItem<T>> _items;
		private int visibleItems;
		private bool scrollVisible;
		private int scrollIndex;
		private Rectangle scrollThumbRectangle;
		private int scrollMouseDown = -1;

		[Category("Data"), Browsable(false)]
		public IEnumerable<T> Items
		{
			get
			{
				lock (_items)
					foreach (var item in _items)
						yield return item.Item;
			}
		}

		[Category("Appearance"), DefaultValue(false)]
		public bool SeparateWithLines { get; set; }

		[Category("Appearance"), DefaultValue(false)]
		public bool HighlightOnHover { get; set; }

		[Category("Appearance"), DefaultValue(22)]
		public int ItemHeight { get; set; }

		[Category("Behavior"), DisplayName("Calculate Item Size")]
		public event Extensions.EventHandler<CanDrawItemEventArgs<T>> CanDrawItem;

		[Category("Appearance"), DisplayName("Paint Item")]
		public event Extensions.EventHandler<ItemPaintEventArgs<T>> PaintItem;

		[Category("Behavior"), DisplayName("Item Mouse Click")]
		public event Extensions.EventHandler<MouseEventArgs> ItemMouseClick;

		protected Point CursorLocation { get; set; }

		public SlickStackedListControl()
		{
			_items = new List<DrawableItem<T>>();
			ItemHeight = 22;
			AutoInvalidate = false;
			AutoScroll = true;
		}

		public virtual void Invalidate(T item)
		{
			lock (_items)
			{
				var selectedItem = _items.FirstOrDefault(x => x.Item.Equals(item));

				Invalidate(selectedItem.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom));
			}
		}

		public virtual void Add(T item)
		{
			lock (_items)
				_items.Add(new DrawableItem<T>(item));

			Invalidate();
		}

		public virtual void AddRange(IEnumerable<T> items)
		{
			lock (_items)
				_items.AddRange(items.Select(item => new DrawableItem<T>(item)));

			Invalidate();
		}

		public virtual void SetItems(IEnumerable<T> items)
		{
			lock (_items)
			{
				_items.Clear();
				_items.AddRange(items.Select(item => new DrawableItem<T>(item)));
			}

			Invalidate();
		}

		public virtual void Remove(T item)
		{
			lock (_items)
				_items.Remove(new DrawableItem<T>(item));

			Invalidate();
		}

		public virtual void RemoveAll(Predicate<T> predicate)
		{
			lock (_items)
				_items.RemoveAll(item => predicate(item.Item));

			Invalidate();
		}

		public virtual void Clear()
		{
			lock (_items)
				_items.Clear();

			Invalidate();
		}

		protected override void UIChanged()
		{
			if (Live)
			{
				ItemHeight = (int)(ItemHeight * UI.FontScale);

				Invalidate();
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (scrollMouseDown == -2)
				return;

			lock (_items)
				foreach (var item in _items)
					if (item.Bounds.Contains(e.Location))
						OnItemMouseClick(item, e);

			base.OnMouseClick(e);
		}

		protected virtual void OnItemMouseClick(DrawableItem<T> item, MouseEventArgs e)
		{
			ItemMouseClick?.Invoke(item.Item, e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			var itemActionHovered = false;

			lock (_items)
			{
				foreach (var item in _items)
				{
					if (item.Bounds.Contains(e.Location))
					{
						item.HoverState |= HoverState.Hovered;
						itemActionHovered |= IsItemActionHovered(item, e.Location);
						Invalidate(item.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom));
					}
					else if (item.HoverState.HasFlag(HoverState.Hovered))
					{
						item.HoverState &= ~HoverState.Hovered;
						Invalidate(item.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom));
					}
				}
			}

			if (scrollMouseDown >= 0)
			{
				scrollIndex = (_items.Count - visibleItems) * (e.Location.Y - scrollMouseDown) / (Height - scrollThumbRectangle.Height);
				Invalidate();
			}

			if (scrollVisible)
			{
				Invalidate(new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
			}

			Cursor = itemActionHovered || scrollMouseDown >= 0 || scrollThumbRectangle.Contains(e.Location) ? Cursors.Hand : Cursors.Default;
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			HoverState |= HoverState.Hovered;
			var mouse = PointToClient(Cursor.Position);

			lock (_items)
			{
				foreach (var item in _items)
				{
					if (item.Bounds.Contains(mouse))
					{
						item.HoverState |= HoverState.Hovered;
						Invalidate(item.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom));
					}
					else if (item.HoverState.HasFlag(HoverState.Hovered))
					{
						item.HoverState &= ~HoverState.Hovered;
						Invalidate(item.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom));
					}
				}
			}

			if (scrollVisible)
			{
				Invalidate(new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			HoverState &= ~HoverState.Hovered;

			lock (_items)
			{
				foreach (var item in _items)
				{
					if (item.HoverState.HasFlag(HoverState.Hovered))
					{
						item.HoverState &= ~HoverState.Hovered;
						Invalidate(item.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom));
					}
				}
			}

			if (scrollVisible)
			{
				Invalidate(new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			HoverState |= HoverState.Pressed;

			lock (_items)
			{
				foreach (var item in _items)
				{
					if (item.Bounds.Contains(e.Location))
						Invalidate(item.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom));
				}
			}

			if (scrollVisible && e.Location.X >= scrollThumbRectangle.X)
			{
				if (scrollThumbRectangle.Contains(e.Location))
				{
					scrollMouseDown = e.Location.Y - scrollThumbRectangle.Y;
				}
				else
				{
					if (e.Location.Y < scrollThumbRectangle.Y)
					{
						scrollIndex -= visibleItems;
					}
					else
					{
						scrollIndex += visibleItems;
					}

					scrollMouseDown = scrollThumbRectangle.Height / 2;
				}

				Invalidate(new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
			}
			else
			{
				scrollMouseDown = -1;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			HoverState &= ~HoverState.Pressed;

			lock (_items)
			{
				foreach (var item in _items)
				{
					if (item.Bounds.Contains(e.Location))
						Invalidate(item.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom));
				}
			}

			if (scrollMouseDown >= 0)
			{
				scrollMouseDown = -2;
				Invalidate();
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);

			scrollIndex -= e.Delta / ItemHeight;
			Invalidate();
		}

		protected virtual bool IsItemActionHovered(DrawableItem<T> item, Point location)
		{
			return false;
		}

		protected virtual IEnumerable<DrawableItem<T>> OrderItems(IEnumerable<DrawableItem<T>> items) => items;

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			CursorLocation = PointToClient(Cursor.Position);

			e.Graphics.Clear(BackColor);
		}

		protected virtual void OnPaintItem(ItemPaintEventArgs<T> e)
		{
			PaintItem?.Invoke(this, e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			var y = 0;

			var itemList = SafeGetItems();

			if (CanDrawItem != null)
			{
				itemList.RemoveAll(x =>
				{
					x.Bounds = Rectangle.Empty;

					var canDraw = new CanDrawItemEventArgs<T>(x.Item);

					CanDrawItem(this, canDraw);

					return canDraw.DoNotDraw;
				});
			}

			HandleScrolling(itemList);

			if (scrollVisible)
			{
				var isMouseDown = HoverState.HasFlag(HoverState.Pressed) && (scrollThumbRectangle.Contains(CursorLocation) || scrollMouseDown >= 0);

				if (isMouseDown || (HoverState.HasFlag(HoverState.Hovered) && CursorLocation.X >= scrollThumbRectangle.X))
				{
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(7, FormDesign.Design.Type == FormDesignType.Dark ? Color.White : Color.Black)), new Rectangle(scrollThumbRectangle.X, -1, scrollThumbRectangle.Width + 1, Height + 2));
				}

				e.Graphics.FillRoundedRectangle(scrollThumbRectangle.Gradient(isMouseDown ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), scrollThumbRectangle.Pad(2, 0, 2, 0), 3);
			}

			foreach (var item in OrderItems(itemList).Skip(scrollIndex))
			{
				item.Bounds = new Rectangle(0, y + Padding.Top, Width - (scrollVisible ? scrollThumbRectangle.Width : 0), ItemHeight);

				if (HighlightOnHover && item.HoverState.HasFlag(HoverState.Hovered))
				{
					e.Graphics.FillRectangle(item.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom).Gradient(Color.FromArgb(30, FormDesign.Design.ActiveColor)), item.Bounds.Pad(0, -Padding.Top, 0, -Padding.Bottom));
				}

				e.Graphics.SetClip(item.Bounds);

				OnPaintItem(new ItemPaintEventArgs<T>(
					item.Item,
					e.Graphics,
					item.Bounds,
					item.HoverState | (scrollMouseDown >= 0 ? HoverState.Normal : (HoverState & (HoverState.Pressed | HoverState.Focused)))));

				e.Graphics.ResetClip();

				y += ItemHeight + Padding.Vertical;

				if (SeparateWithLines)
				{
					e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor, (int)UI.FontScale), Padding.Left, y, Width - Padding.Right - (int)(scrollVisible ? 6 * UI.FontScale : 0), y);

					y += (int)UI.FontScale;
				}

				if (y > Height)
					break;
			}
		}

		private void HandleScrolling(List<DrawableItem<T>> itemList)
		{
			var totalHeight = itemList.Count * (ItemHeight + Padding.Vertical);

			if (SeparateWithLines)
			{
				totalHeight += (itemList.Count - 1) * (int)UI.FontScale;
			}

			if (totalHeight > Height)
			{
				visibleItems = (int)Math.Floor((float)Height / (ItemHeight + Padding.Vertical + (SeparateWithLines ? (int)UI.FontScale:0)));
				scrollVisible = true;
				scrollIndex = Math.Max(0, Math.Min(scrollIndex, itemList.Count - visibleItems));

				var thumbHeight = Math.Max(Height * visibleItems / itemList.Count, Height / 24);

				scrollThumbRectangle = new Rectangle(Width - (int)(10 * UI.FontScale), (Height - thumbHeight) * scrollIndex / (itemList.Count - visibleItems), (int)(10 * UI.FontScale), thumbHeight);
			}
			else
			{
				scrollVisible = false;
				scrollIndex = 0;
			}
		}

		private List<DrawableItem<T>> SafeGetItems()
		{
			lock (_items)
			{
				return _items.ToList();
			}
		}
	}
}