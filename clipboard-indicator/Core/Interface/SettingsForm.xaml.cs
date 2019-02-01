// This file is a part of clipboard-indicator, licensed under the MIT License.
//
// Copyright (c) Jakub Zagórski (jaqobb)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardIndicator.Core.Interface
{
	public partial class SettingsForm
	{
		private readonly ClipboardIndicatorMain _clipboardIndicator;
		private readonly NumericUpDown _historySizeBox;
		private readonly CheckBox _notifyBox;
		private readonly Label _historySizeInfoBox;
		private readonly NumericUpDown _notifyDurationBox;
		private readonly Label _notifyDurationInfoBox;

		public SettingsForm(ClipboardIndicatorMain clipboardIndicator)
		{
			_clipboardIndicator = clipboardIndicator;
			_historySizeBox = new NumericUpDown();
			_historySizeBox.Minimum = 10;
			_historySizeBox.Maximum = 150;
			_historySizeBox.Value = _clipboardIndicator.HistorySize;
			_historySizeBox.Location = new Point(15, 5);
			_historySizeBox.Size = new Size(45, 25);
			_historySizeBox.ValueChanged += HandleHistorySize;
			_historySizeInfoBox = new Label();
			_historySizeInfoBox.Text = "History size";
			_historySizeInfoBox.Location = new Point(62, 8);
			_historySizeInfoBox.AutoSize = true;
			_notifyBox = new CheckBox();
			_notifyBox.Text = "Notify on clipboard save";
			_notifyBox.Location = new Point(15, 29);
			_notifyBox.Size = new Size(145, 25);
			_notifyBox.Checked = _clipboardIndicator.Notify;
			_notifyBox.MouseClick += HandleNotify;
			_notifyDurationBox = new NumericUpDown();
			_notifyDurationBox.Minimum = 100;
			_notifyDurationBox.Maximum = 10000;
			_notifyDurationBox.Value = _clipboardIndicator.NotifyDuration;
			_notifyDurationBox.Location = new Point(30, 55);
			_notifyDurationBox.Size = new Size(60, 25);
			_notifyDurationBox.Enabled = _clipboardIndicator.Notify;
			_notifyDurationBox.ValueChanged += HandleNotifyDuration;
			_notifyDurationInfoBox = new Label();
			_notifyDurationInfoBox.Text = "Notify duration";
			_notifyDurationInfoBox.Location = new Point(90, 58);
			_notifyDurationInfoBox.AutoSize = true;
			Controls.Add(_historySizeBox);
			Controls.Add(_historySizeInfoBox);
			Controls.Add(_notifyBox);
			Controls.Add(_notifyDurationBox);
			Controls.Add(_notifyDurationInfoBox);
			InitializeComponent();
		}

		private void HandleHistorySize(object sender, EventArgs arguments)
		{
			_clipboardIndicator.HistorySize = (int) _historySizeBox.Value;
			_clipboardIndicator.SaveConfiguration();
		}

		private void HandleNotify(object sender, MouseEventArgs arguments)
		{
			if (arguments.Button == MouseButtons.Left)
			{
				_clipboardIndicator.Notify = !_clipboardIndicator.Notify;
				_clipboardIndicator.SaveConfiguration();
				_notifyBox.Checked = _clipboardIndicator.Notify;
				_notifyDurationBox.Enabled = _clipboardIndicator.Notify;
			}
		}

		private void HandleNotifyDuration(object sender, EventArgs arguments)
		{
			_clipboardIndicator.NotifyDuration = (int) _notifyDurationBox.Value;
			_clipboardIndicator.SaveConfiguration();
		}
	}
}