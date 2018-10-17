using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace clipdicator.Core.Ui {
  public partial class MainForm {
    private readonly Clipdicator _clipdicator;
    private NotifyIcon _notifyIcon;
    public string LastClipboardText = "";

    public MainForm(Clipdicator clipdicator) {
      _clipdicator = clipdicator;
      InitializeComponent();
    }

    protected override void OnFormClosing(FormClosingEventArgs arguments) {
      base.OnFormClosing(arguments);
      if (arguments.CloseReason == CloseReason.UserClosing) {
        arguments.Cancel = true;
      } else {
        _clipdicator.IsRunning = false;
        _notifyIcon.Visible = false;
      }
      Hide();
    }

    public void StartNotifyIcon() {
      _notifyIcon = new NotifyIcon();
      _notifyIcon.Icon = new Icon(Clipdicator.IconFile);
      ContextMenu notifyIconContextMenu = new ContextMenu();
      notifyIconContextMenu.MenuItems.Add("History", LaunchHistory);
      notifyIconContextMenu.MenuItems.Add("Settings", LaunchSettings);
      notifyIconContextMenu.MenuItems.Add("Exit", LaunchExit);
      _notifyIcon.ContextMenu = notifyIconContextMenu;
      _notifyIcon.Visible = true;
    }

    public void ListenForCopy() {
      Thread thread = new Thread(() => {
        LastClipboardText = Clipboard.GetText();
        while (_clipdicator.IsRunning) {
          Thread.Sleep(50);
          string clipboardText = Clipboard.GetText();
          if (clipboardText.Length != 0 && !clipboardText.Equals(LastClipboardText)) {
            LastClipboardText = clipboardText;
            _clipdicator.AddToHistory(LastClipboardText);
            _clipdicator.SaveHistory();
            if (_clipdicator.Notify) {
              _notifyIcon.ShowBalloonTip(_clipdicator.NotifyDuration, "clipdicator", "Clipboard saved.", ToolTipIcon.Info);
            }
          }
        }
      });
      thread.SetApartmentState(ApartmentState.STA);
      thread.IsBackground = true;
      thread.Start();
    }

    private void LaunchHistory(object sender, EventArgs arguments) {
      HistoryForm historyForm = new HistoryForm(_clipdicator, this);
      historyForm.Name = "clipdicator";
      historyForm.Icon = new Icon(Clipdicator.IconFile);
      historyForm.Text = "clipdicator";
      historyForm.Size = new Size(300, 225);
      historyForm.MinimizeBox = false;
      historyForm.MaximizeBox = false;
      historyForm.StartPosition = FormStartPosition.CenterScreen;
      historyForm.Show();
    }

    private void LaunchSettings(object sender, EventArgs arguments) {
      SettingsForm settingsForm = new SettingsForm(_clipdicator);
      settingsForm.Name = "clipdicator";
      settingsForm.Icon = new Icon(Clipdicator.IconFile);
      settingsForm.Text = "clipdicator";
      settingsForm.Size = new Size(200, 125);
      settingsForm.MinimizeBox = false;
      settingsForm.MaximizeBox = false;
      settingsForm.FormBorderStyle = FormBorderStyle.FixedSingle;
      settingsForm.StartPosition = FormStartPosition.CenterScreen;
      settingsForm.Show();
    }

    private void LaunchExit(object sender, EventArgs arguments) {
      _clipdicator.IsRunning = false;
      _notifyIcon.Visible = false;
      Hide();
      Process.GetCurrentProcess().Kill();
    }
  }
}