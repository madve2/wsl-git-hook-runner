using System;
using System.Diagnostics;

namespace GitHookRunner
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("WSL Git hook runner started.");
			var winScript = args[1].Replace('\\', '/').Replace("////", "/").Trim('\'');
			var driveLetter = winScript[0];
			var bashScript = winScript.Replace(driveLetter + ":/", $"/mnt/{driveLetter.ToString().ToLowerInvariant()}/");

			var wsl = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = @"wsl.exe",
					Arguments = $"bash -ic {bashScript}",
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
