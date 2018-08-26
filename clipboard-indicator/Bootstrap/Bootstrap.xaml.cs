using System.Windows;
using clipboard_indicator.Core;

namespace clipboard_indicator.Bootstrap
{
    public partial class Bootstrap
    {
        private void Launch(object sender, StartupEventArgs arguments)
        {
            ClipboardIndicator clipboardIndicator = new ClipboardIndicator();
            clipboardIndicator.Start();
        }
    }
}