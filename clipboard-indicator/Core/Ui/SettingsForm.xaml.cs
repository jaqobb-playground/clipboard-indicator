using System;
using System.Windows.Forms;
using System.Drawing;

namespace clipboard_indicator.Core.Ui
{
    public partial class SettingsForm
    {
        private readonly ClipboardIndicator _clipboardIndicator;
        private readonly NumericUpDown _historySizeBox;
        private readonly CheckBox _notifyBox;
        private readonly NumericUpDown _notifyDurationBox;

        public SettingsForm(ClipboardIndicator clipboardIndicator)
        {
            _clipboardIndicator = clipboardIndicator;
            _historySizeBox = new NumericUpDown();
            _historySizeBox.Minimum = 10;
            _historySizeBox.Maximum = 150;
            _historySizeBox.Value = _clipboardIndicator.HistorySize;
            _historySizeBox.Location = new Point(15, 5);
            _historySizeBox.Size = new Size(45, 25);
            _historySizeBox.ValueChanged += HandleHistorySize;
            Label historySizeInfoBox = new Label();
            historySizeInfoBox.Text = "History size";
            historySizeInfoBox.Location = new Point(62, 8);
            historySizeInfoBox.AutoSize = true;
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
            Label notifyDurationInfoBox = new Label();
            notifyDurationInfoBox.Text = "Notify duration";
            notifyDurationInfoBox.Location = new Point(90, 58);
            notifyDurationInfoBox.AutoSize = true;
            Controls.Add(_historySizeBox);
            Controls.Add(historySizeInfoBox);
            Controls.Add(_notifyBox);
            Controls.Add(_notifyDurationBox);
            Controls.Add(notifyDurationInfoBox);
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