﻿using Extensions;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SlickControls.Controls.Form
{
	public abstract class SlickSelectionDropDown<T> : SlickControl
	{
		private SlickForm _form;
		private T[] _items;
		private T selectedItem;
		private CustomStackedListControl listDropDown;

		public event EventHandler SelectedItemChanged;

		[Category("Data")]
		public T[] Items { get => _items; set { _items = value; if (_items?.Length > 0) { selectedItem = _items[0]; } } }

		[Category("Data"), DefaultValue(null)]
		public T SelectedItem { get => selectedItem; set { selectedItem = value; SelectedItemChanged?.Invoke(this, EventArgs.Empty); } }

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Bindable(true)]
		public override string Text { get => base.Text; set { base.Text = value; UIChanged(); } }

		public SlickSelectionDropDown()
		{
			Cursor = Cursors.Hand;
		}

		public void ShowDropdown()
		{
			if (listDropDown == null)
			{
				if (Items != null && _form != null)
				{
					listDropDown = new CustomStackedListControl()
					{
						BackColor = FormDesign.Design.ButtonColor,
						Padding = UI.Scale(new Padding(5), UI.FontScale),
						Location = _form.PointToClient(PointToScreen(new Point(0, Height - 3))),
						Font = Font,
						Cursor = Cursor,
						SeparateWithLines = true,
						MaximumSize = new Size(Width, 9999),
						MinimumSize = new Size(Width, 0),
						Size = new Size(Width, 0)
					};

					listDropDown.PaintItem += ListDropDown_PaintItem;
					listDropDown.ItemMouseClick += DropDownItems_ItemMouseClick;
					listDropDown.Parent = _form;
					listDropDown.BringToFront();
					listDropDown.SetItems(Items);

					new AnimationHandler(listDropDown, new Size(Width, Math.Min((listDropDown.ItemHeight + listDropDown.Padding.Vertical + (int)UI.FontScale) * Math.Min(10, Items.Length), _form.Height - listDropDown.Top - 15)), 2).StartAnimation();
				}
				else
				{
					SystemSounds.Exclamation.Play();
				}
			}
			else
			{
				CloseDropDown();
			}
		}

		protected override void UIChanged()
		{
			Padding = UI.Scale(new Padding(5), UI.FontScale);
			Height = Font.Height + Padding.Vertical;
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			if (_form == null && FindForm() is SlickForm slickForm)
			{
				_form = slickForm;
				_form.OnWndProc += Frm_OnWndProc;
				Disposed += (s, _) => _form.OnWndProc -= Frm_OnWndProc;

				LocationChanged += (s, ea) =>
				{
					if (listDropDown != null)
					{
						listDropDown.Location = _form.PointToClient(PointToScreen(new Point(0, Height - 2)));
					}
				};

				_form.LocationChanged += (s, ea) =>
				{
					if (listDropDown != null)
					{
						listDropDown.Location = _form.PointToClient(PointToScreen(new Point(0, Height - 2)));
					}
				};

				_form.Resize += (s, ea) =>
				{
					if (listDropDown != null && listDropDown.Visible)
					{
						if (_form.WindowState == FormWindowState.Minimized)
						{
							CloseDropDown();
						}
						else
						{
							listDropDown.Location = _form.PointToClient(PointToScreen(new Point(0, Height - 2)));
							listDropDown.MaximumSize = new Size(Width, 9999);
							listDropDown.MinimumSize = new Size(Width, 0);
						}
					}
				};
			}
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);

			CloseDropDown();
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.Button == MouseButtons.Left)
			{
				ShowDropdown();
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (Keys.Down == keyData && listDropDown == null)
			{
				ShowDropdown();
				return true;
			}

			if (Keys.Up == keyData && listDropDown != null)
			{
				CloseDropDown();
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void CloseDropDown()
		{
			if (listDropDown != null)
			{
				var ctrl = listDropDown;
				new AnimationHandler(ctrl, new Size(Width, 0), 2).StartAnimation(ctrl.Dispose);

				listDropDown = null;

				Invalidate();
			}
		}

		private void DropDownItems_ItemMouseClick(object sender, MouseEventArgs e)
		{
			SelectedItem = (T)sender;

			CloseDropDown();
		}

		private bool Frm_OnWndProc(Message arg)
		{
			if (Visible
				&& listDropDown != null
				&& arg.Msg == 0x21
				&& !new Rectangle(PointToScreen(Point.Empty), Size).Contains(Cursor.Position)
				&& !new Rectangle(listDropDown.PointToScreen(Point.Empty), listDropDown.Size).Contains(Cursor.Position))
			{
				CloseDropDown();
			}

			return false;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			SlickButton.GetColors(out var fore, out var back, listDropDown != null ? HoverState.Pressed : HoverState);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			using (var brush = ClientRectangle.Gradient(back))
			{
				e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(0, 0, 1, 1), Padding.Left, true, true, listDropDown == null, listDropDown == null);
			}

			PaintItem(e, ClientRectangle.Pad(Padding), fore, listDropDown != null ? HoverState.Pressed : HoverState, SelectedItem);
		}

		private void ListDropDown_PaintItem(object sender, ItemPaintEventArgs<T> e)
		{
			SlickButton.GetColors(out var fore, out var back, e.HoverState);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (e.HoverState.HasFlag(HoverState.Hovered) || e.HoverState.HasFlag(HoverState.Pressed))
			{
				using (var brush = new SolidBrush(back))
				{
					e.Graphics.SetClip(e.ClipRectangle.Pad(0, -Padding.Top, 0, -Padding.Bottom));
					e.Graphics.FillRectangle(brush, e.Graphics.ClipBounds);
				}
			}

			PaintItem(e, e.ClipRectangle.Pad(Padding.Left, 0, Padding.Right, 0), fore, e.HoverState, e.Item);
		}

		protected abstract void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, T item);

		private class CustomStackedListControl : SlickStackedListControl<T> 
		{
			protected override bool IsItemActionHovered(DrawableItem<T> item, Point location)
			{
				return true;
			}
		}
	}
}
