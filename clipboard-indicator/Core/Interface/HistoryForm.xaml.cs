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

using System.Drawing;
using System.Windows.Forms;
using Clipboard = System.Windows.Clipboard;

namespace clipboard_indicator.Core.Interface
{
	public partial class HistoryForm
	{
		private readonly ClipboardIndicator _clipboardIndicator;
		private readonly MainForm _mainForm;
		private readonly ListBox _historyBox;

		public HistoryForm(ClipboardIndicator clipboardIndicator, MainForm mainForm)
		{
			_clipboardIndicator = clipboardIndicator;
			_mainForm = mainForm;
			_historyBox = new ListBox();
			_historyBox.Text = "Notify on clipboard save";
			_historyBox.Location = new Point(0, 0);
			_historyBox.Size = new Size(285, 190);
			foreach (string line in _clipboardIndicator.History)
			{
				_historyBox.Items.Add(line);
			}
			_historyBox.MouseDoubleClick += HandleHistory;
			Controls.Add(_historyBox);
			InitializeComponent();
		}

		private void HandleHistory(object sender, MouseEventArgs arguments)
		{
			if (arguments.Button != MouseButtons.Left)
			{
				return;
			}
			int index = _historyBox.IndexFromPoint(arguments.Location);
			if (index != ListBox.NoMatches)
			{
				string line = _clipboardIndicator.History[index];
				_mainForm.LastClipboardText = line;
				Clipboard.SetText(line);
				Hide();
			}
		}
	}
}