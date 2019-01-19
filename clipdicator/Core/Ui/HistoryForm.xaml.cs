using System.Drawing;
using System.Windows.Forms;
using Clipboard = System.Windows.Clipboard;

namespace clipdicator.Core.Ui {
	public partial class HistoryForm {
		private readonly Clipdicator _clipdicator;
		private readonly MainForm _mainForm;
		private readonly ListBox _historyBox;

		public HistoryForm(Clipdicator clipdicator, MainForm mainForm) {
			_clipdicator = clipdicator;
			_mainForm = mainForm;
			_historyBox = new ListBox();
			_historyBox.Text = "Notify on clipboard save";
			_historyBox.Location = new Point(0, 0);
			_historyBox.Size = new Size(285, 190);
			foreach (string line in _clipdicator.History) {
				_historyBox.Items.Add(line);
			}
			_historyBox.MouseDoubleClick += HandleHistory;
			Controls.Add(_historyBox);
			InitializeComponent();
		}

		private void HandleHistory(object sender, MouseEventArgs arguments) {
			if (arguments.Button != MouseButtons.Left) {
				return;
			}
			int index = _historyBox.IndexFromPoint(arguments.Location);
			if (index != ListBox.NoMatches) {
				string line = _clipdicator.History[index];
				_mainForm.LastClipboardText = line;
				Clipboard.SetText(line);
				Hide();
			}
		}
	}
}