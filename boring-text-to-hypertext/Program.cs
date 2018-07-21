using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BoringTextToHypertext {
	internal sealed class Program {

		private static void Main(string[] args) {

			if(!HTMLManager.Initialize()) {

				Console.WriteLine("There was a failure initializing HTMLManager");
				Console.ReadKey(true);

				return;
			}

			if(args != null && args.Length > 0) {

				foreach(string arg in args) {

					ProcessFile(arg);

				}

				Console.WriteLine("Batch processing complete");

				Program.Main(null);

			} else {

				HTMLManager.Initialize();

				while(true) {

					Console.Write("File path: ");
					ProcessFile(Console.ReadLine().Trim());

				}
			}
		}

		private static void ProcessFile(string path) {


			if(File.Exists(path)) {

				string HTMLBody = null;

				try {

					using(StreamReader reader = File.OpenText(path)) {
						HTMLBody = GenerateHTMLBody(reader);
					}

				} catch (IOException exception) {


					Console.WriteLine($"Failure opening \"{path}\"");

					Console.WriteLine(exception.Message);

					return;
				}


				var exportPath = HTMLManager.GetFilePath(path);

				if(File.Exists(exportPath)) {

					bool @continue = false;
					Console.WriteLine("Do you want to overwrite the already existing file? (Y/N)");

					while(!@continue) {
						switch(Console.ReadKey(true).Key) {
							case ConsoleKey.Y:
								@continue = true;
								break;
							case ConsoleKey.N:
								Console.WriteLine($"Stopped {path} conversion");
								return;
						}
					}
				}

				try {

					HTMLManager.WriteHTMLFile(exportPath,HTMLBody);

				} catch(IOException exception) {

					Console.WriteLine($"Failure writing {path} to {exportPath}");

					Console.WriteLine(exception.Message);

					return;

				}

				Console.WriteLine($"Saved {path} to {exportPath}!");

			} else {

				Console.WriteLine($"{path} (as a file) does not exist");

			}
		}


		private static string GenerateHTMLBody(StreamReader reader) {
			var stringBuilder = new StringBuilder();

			while(!reader.EndOfStream) {

				string line = reader.ReadLine();

				HTMLManager.GetHTMLForLine(line,ref stringBuilder);


			}

			return stringBuilder.ToString();


		}


	}
}
