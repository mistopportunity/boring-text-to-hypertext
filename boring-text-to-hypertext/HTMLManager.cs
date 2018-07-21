using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace BoringTextToHypertext {
    internal static class HTMLManager {

		private const string SchemaFile = "Schema.html";
		private const string SchemaTitle = "{TITLE}";
		private const string SchemaBody = "{BODY}";
		private const string HTMLFolder = "HTML";

		private static string schema = string.Empty;

		internal static bool Initialize() {
			if(File.Exists(SchemaFile)) {
				try {

					schema = File.ReadAllText(SchemaFile);

					return true;

				} catch(IOException exception) {

					Console.WriteLine(exception.Message);

					return false;
				}
			} else {
				Console.WriteLine($"Could not find a schema file in executing directory ({SchemaFile})");
				return false;
			}
		}

		internal static void GetHTMLForLine(string line,ref StringBuilder builder) {

			line.Trim();

			line = line.Replace("<","&lt;");

			line = line.Replace(">","&gt;");
			
			builder.AppendLine($"		<p>{line}<p>");

		}

		internal static string GetFilePath(string originalPath) {
			return Path.Combine(
				HTMLFolder,
				Path.ChangeExtension(originalPath,".html")
			);
		}

		internal static void WriteHTMLFile(string path,string HTMLBody) {

			string file = schema;
			string HTMLTitle = Path.GetFileNameWithoutExtension(path);

			file = file.Replace(SchemaTitle,HTMLTitle);
			file = file.Replace(SchemaBody,HTMLBody);

			Directory.CreateDirectory(HTMLFolder);

			File.WriteAllText(path,file);

		}
	}
}
