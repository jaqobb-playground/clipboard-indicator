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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ClipboardIndicator.Core.Interface;

namespace ClipboardIndicator.Core
{
	public class ClipboardIndicator
	{
		public static readonly string ApplicationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Clipboard Indicator");
		public static readonly string ConfigurationFile = Path.Combine(ApplicationFolder, "configuration.properties");
		public static readonly string HistoryFile = Path.Combine(ApplicationFolder, "history.log");
		public static readonly string IconFile = Path.Combine(ApplicationFolder, "icon.ico");

		public bool IsRunning;
		public int HistorySize;
		public bool Notify;
		public int NotifyDuration;
		public List<string> History;

		public void Start()
		{
			if (!Directory.Exists(ApplicationFolder))
			{
				Directory.CreateDirectory(ApplicationFolder);
			}
			if (!File.Exists(ConfigurationFile))
			{
				File.AppendAllText(ConfigurationFile, "history-size=50");
				File.AppendAllText(ConfigurationFile, Environment.NewLine);
				File.AppendAllText(ConfigurationFile, "notify=True");
				File.AppendAllText(ConfigurationFile, "notify-duration=250");
			}
			foreach (string line in File.ReadAllLines(ConfigurationFile))
			{
				if (line.Length == 0)
				{
					continue;
				}
				string[] data = line.Split('=');
				if (data.Length != 2)
				{
					continue;
				}
				if (data[0].Equals("history-size"))
				{
					if (!int.TryParse(data[1], out HistorySize))
					{
						HistorySize = 50;
						continue;
					}
					if (HistorySize < 10)
					{
						HistorySize = 10;
						continue;
					}
					if (HistorySize > 150)
					{
						HistorySize = 150;
						continue;
					}
				}
				if (data[0].Equals("notify"))
				{
					if (!bool.TryParse(data[1], out Notify))
					{
						Notify = true;
						continue;
					}
				}
				if (data[0].Equals("notify-duration"))
				{
					if (!int.TryParse(data[1], out NotifyDuration))
					{
						NotifyDuration = 250;
						continue;
					}
					if (NotifyDuration < 100)
					{
						NotifyDuration = 100;
					}
					if (NotifyDuration > 10000)
					{
						NotifyDuration = 10000;
					}
				}
			}
			SaveConfiguration();
			History = new List<string>(HistorySize);
			if (!File.Exists(HistoryFile))
			{
				File.Create(HistoryFile).Close();
			}
			else
			{
				foreach (string line in File.ReadAllLines(HistoryFile))
				{
					if (History.Count < HistorySize)
					{
						History.Add(line);
					}
				}

				SaveHistory();
			}
			IsRunning = true;
			MainForm form = new MainForm(this);
			form.Name = "Clipboard Indicator";
			form.Size = new Size(0, 0);
			form.StartNotifyIcon();
			form.ListenForCopy();
		}

		public void AddToHistory(string line)
		{
			History.Insert(0, line);
			if (History.Count > HistorySize)
			{
				History.RemoveAt(HistorySize - 1);
			}
		}

		public void SaveConfiguration()
		{
			File.Delete(ConfigurationFile);
			File.AppendAllText(ConfigurationFile, "history-size=" + HistorySize);
			File.AppendAllText(ConfigurationFile, Environment.NewLine);
			File.AppendAllText(ConfigurationFile, "notify=" + Notify);
			File.AppendAllText(ConfigurationFile, "notify-duration=" + NotifyDuration);
		}

		public void SaveHistory()
		{
			File.Delete(HistoryFile);
			for (int index = 0; index < History.Count; index++)
			{
				File.AppendAllText(HistoryFile, History[index]);
				if (index != History.Count - 1)
				{
					File.AppendAllText(HistoryFile, Environment.NewLine);
				}
			}
		}
	}
}
