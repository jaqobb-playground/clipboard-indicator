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
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ClipboardIndicator.Core.Interface
{
	public partial class MainForm
	{
		private readonly ClipboardIndicatorMain _clipboardIndicator;
		private NotifyIcon _notifyIcon;
		public string LastClipboardText = "";

		public MainForm(ClipboardIndicatorMain clipboardIndicator)
		{
			_clipboardIndicator = clipboardIndicator;
			InitializeComponent();
		}

		protected override void OnFormClosing(FormClosingEventArgs arguments)
		{
			base.OnFormClosing(arguments);
			if (arguments.CloseReason == CloseReason.UserClosing)
			{
				arguments.Cancel = true;
			}
			else
			{
				_clipboardIndicator.IsRunning = false;
				_notifyIcon.Visible = false;
			}
			Hide();
		}

		public void StartNotifyIcon()
		{
			_notifyIcon = new NotifyIcon();
			_notifyIcon.Icon = new Icon(ClipboardIndicatorMain.IconFile);
			ContextMenu notifyIconContextMenu = new ContextMenu();
			notifyIconContextMenu.MenuItems.Add("History", LaunchHistory);
			notifyIconContextMenu.MenuItems.Add("Settings", LaunchSettings);
			notifyIconContextMenu.MenuItems.Add("Exit", LaunchExit);
			_notifyIcon.ContextMenu = notifyIconContextMenu;
			_notifyIcon.Visible = true;
		}

		public void ListenForCopy()
		{
			Thread thread = new Thread(() =>
			{
				LastClipboardText = Clipboard.GetText();
				while (_clipboardIndicator.IsRunning)
				{
					Thread.Sleep(50);
					string clipboardText = Clipboard.GetText();
					if (clipboardText.Length != 0 && !clipboardText.Equals(LastClipboardText))
					{
						LastClipboardText = clipboardText;
						_clipboardIndicator.AddToHistory(LastClipboardText);
						_clipboardIndicator.SaveHistory();
						if (_clipboardIndicator.Notify)
						{
							_notifyIcon.ShowBalloonTip(_clipboardIndicator.NotifyDuration, "Clipboard Indicator", "Clipboard saved.", ToolTipIcon.Info);
						}
					}
				}
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.IsBackground = true;
			thread.Start();
		}

		private void LaunchHistory(object sender, EventArgs arguments)
		{
			HistoryForm historyForm = new HistoryForm(_clipboardIndicator, this);
			historyForm.Name = "Clipboard Indicator";
			historyForm.Icon = new Icon(ClipboardIndicatorMain.IconFile);
			historyForm.Text = "Clipboard Indicator";
			historyForm.Size = new Size(300, 225);
			historyForm.MinimizeBox = false;
			historyForm.MaximizeBox = false;
			historyForm.StartPosition = FormStartPosition.CenterScreen;
			historyForm.Show();
		}

		private void LaunchSettings(object sender, EventArgs arguments)
		{
			SettingsForm settingsForm = new SettingsForm(_clipboardIndicator);
			settingsForm.Name = "Clipboard Indicator";
			settingsForm.Icon = new Icon(ClipboardIndicatorMain.IconFile);
			settingsForm.Text = "Clipboard Indicator";
			settingsForm.Size = new Size(200, 125);
			settingsForm.MinimizeBox = false;
			settingsForm.MaximizeBox = false;
			settingsForm.FormBorderStyle = FormBorderStyle.FixedSingle;
			settingsForm.StartPosition = FormStartPosition.CenterScreen;
			settingsForm.Show();
		}

		private void LaunchExit(object sender, EventArgs arguments)
		{
			_clipboardIndicator.IsRunning = false;
			_notifyIcon.Visible = false;
			Hide();
			Process.GetCurrentProcess().Kill();
		}
	}
}