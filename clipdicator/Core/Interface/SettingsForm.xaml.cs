using System;
using System.Drawing;
using System.Windows.Forms;

namespace clipdicator.Core.Interface
{
	public partial class SettingsForm
	{
		private readonly Clipdicator _clipdicator;
		private readonly NumericUpDown _historySizeBox;
		private readonly CheckBox _notifyBox;
		private readonly Label _historySizeInfoBox;
		private readonly NumericUpDown _notifyDurationBox;
		private readonly Label _notifyDurationInfoBox;

		public SettingsForm(Clipdicator clipdicator)
		{
			_clipdicator = clipdicator;
			_historySizeBox = new NumericUpDown();
			_historySizeBox.Minimum = 10;
			_historySizeBox.Maximum = 150;
			_historySizeBox.Value = _clipdicator.HistorySize;
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
			_notifyBox.Checked = _clipdicator.Notify;
			_notifyBox.MouseClick += HandleNotify;
			_notifyDurationBox = new NumericUpDown();
			_notifyDurationBox.Minimum = 100;
			_notifyDurationBox.Maximum = 10000;
			_notifyDurationBox.Value = _clipdicator.NotifyDuration;
			_notifyDurationBox.Location = new Point(30, 55);
			_notifyDurationBox.Size = new Size(60, 25);
			_notifyDurationBox.Enabled = _clipdicator.Notify;
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
			_clipdicator.HistorySize = (int) _historySizeBox.Value;
			_clipdicator.SaveConfiguration();
		}

		private void HandleNotify(object sender, MouseEventArgs arguments)
		{
			if (arguments.Button == MouseButtons.Left)
			{
				_clipdicator.Notify = !_clipdicator.Notify;
				_clipdicator.SaveConfiguration();
				_notifyBox.Checked = _clipdicator.Notify;
				_notifyDurationBox.Enabled = _clipdicator.Notify;
			}
		}

		private void HandleNotifyDuration(object sender, EventArgs arguments)
		{
			_clipdicator.NotifyDuration = (int) _notifyDurationBox.Value;
			_clipdicator.SaveConfiguration();
		}
	}
}