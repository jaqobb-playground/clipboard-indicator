using System.Windows;
using clipdicator.Core;

namespace clipdicator.Bootstrap {
	public partial class Bootstrap {
		private void Launch(object sender, StartupEventArgs arguments) {
			Clipdicator clipdicator = new Clipdicator();
			clipdicator.Start();
		}
	}
}