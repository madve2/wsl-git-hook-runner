using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitHookRunner
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("WSL Git hook runner started.");
			var script = args[1].Replace('\\', '/');
			var driveLetter = script[0];
			script = script.Replace(driveLetter + ":/", $"/mnt/{driveLetter.ToString().ToLowerInvariant()}/");

			var wsl = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = @"wsl.exe",
					Arguments = script,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};
			wsl.Start();
			while (!wsl.StandardOutput.EndOfStream)
			{
				Console.WriteLine(wsl.StandardOutput.ReadLine());
			}
			Console.WriteLine("WSL Git hook runner quit.");
			wsl.WaitForExit();
			Environment.Exit(wsl.ExitCode);
		}
	}
}
