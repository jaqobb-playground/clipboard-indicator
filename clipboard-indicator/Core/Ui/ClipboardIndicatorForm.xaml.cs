using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace clipboard_indicator.Core.Ui
{
    public partial class ClipboardIndicatorForm
    {
        private readonly ClipboardIndicator _clipboardIndicator;
        private NotifyIcon _notifyIcon;
        public string LastClipboardText = "";

        public ClipboardIndicatorForm(ClipboardIndicator clipboardIndicator)
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

                Hide();
            }
            else
            {
                _clipboardIndicator.IsRunning = false;
                _notifyIcon.Visible = false;

                Hide();
            }
        }

        public void StartNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new Icon("icon.ico");

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
            ClipboardIndicatorHistoryForm historyForm = new ClipboardIndicatorHistoryForm(_clipboardIndicator, this);
            historyForm.Name = "Clipboard Indicator";
            historyForm.Icon = new Icon("icon.ico");
            historyForm.Text = "Clipboard Indicator";
            historyForm.Size = new Size(300, 225);
            historyForm.MinimizeBox = false;
            historyForm.MaximizeBox = false;
            historyForm.StartPosition = FormStartPosition.CenterScreen;
            
            historyForm.Show();
        }

        private void LaunchSettings(object sender, EventArgs arguments)
        {
            ClipboardIndicatorSettingsForm settingsForm = new ClipboardIndicatorSettingsForm(_clipboardIndicator);
            settingsForm.Name = "Clipboard Indicator";
            settingsForm.Icon = new Icon("icon.ico");
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