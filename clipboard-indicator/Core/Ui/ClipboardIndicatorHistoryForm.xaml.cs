using System.Drawing;
using System.Windows.Forms;
using Clipboard = System.Windows.Clipboard;

namespace clipboard_indicator.Core.Ui
{
    public partial class ClipboardIndicatorHistoryForm
    {
        private readonly ClipboardIndicator _clipboardIndicator;
        private readonly ClipboardIndicatorForm _clipboardIndicatorForm;
        private readonly ListBox _historyBox;

        public ClipboardIndicatorHistoryForm(ClipboardIndicator clipboardIndicator, ClipboardIndicatorForm clipboardIndicatorForm)
        {
            _clipboardIndicator = clipboardIndicator;
            _clipboardIndicatorForm = clipboardIndicatorForm;

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

                _clipboardIndicatorForm.LastClipboardText = line;
                
                Clipboard.SetText(line);
                
                Hide();
            }
        }
    }
}