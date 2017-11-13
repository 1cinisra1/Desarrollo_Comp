using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;

namespace HtmlHelpers
{
	/// <summary>
	/// Adapted from https://github.com/madskristensen/BundlerMinifier/wiki/Unbundling-scripts-for-debugging
	/// 
	/// Areas modified:
	///  - Modified it to make it work with aspnetcore.
	///  - Accept both scripts and styles.
	///  - Read config from wwwroot
	///  - Accept baseFolder since DI not suited for static methods
	///  - Style nitpicks
	/// </summary>
	public class Bundler
	{
		private const string VirtualFolder = "../wwwroot/";

		/// <summary>
		/// Unpacks the bundle (css/js) for debugging purposes. Makes the build faster by not bundling while debugging.
		/// </summary>
		/// <param name="baseFolder">The base folder for this application/</param>
		/// <param name="bundlePath">The bundled file to unpack.</param>
		/// <returns>Unpacked bundles</returns>
		public static HtmlString Unpack(string baseFolder, string bundlePath)
		{
			Console.WriteLine("basefolder: " + baseFolder);
			var configFile = Path.Combine(baseFolder, "bundleconfig.json");
			Console.WriteLine("file: " + configFile);
			var bundle = GetBundle(configFile, bundlePath);
			if (bundle == null)
			{
				Console.WriteLine("IS NULL");
				return null;
			}

			GPSMonitoreo.Libraries.Utils.ObjectJsonDumper.Dump(bundle.InputFiles, 1);

			// Clean up the bundle to remove the virtual folder that aspnetcore provides.
			//var inputFiles = bundle.InputFiles.Select(file => file.Substring(VirtualFolder.Length));
			//var inputFiles = bundle.InputFiles.Select(file => file.Substring(baseFolder.Length));
			var inputFiles = bundle.InputFiles;

			var outputString = bundlePath.EndsWith(".js") ?
				inputFiles.Select(inputFile => $"<script src='/{inputFile.Substring(1)}' type='text/javascript'></script>") :
				inputFiles.Select(inputFile => $"<link rel='stylesheet' href='/{inputFile.Substring(1)}' />");

			return new HtmlString(string.Join("\n", outputString));
		}

		private static Bundle GetBundle(string configFile, string bundlePath)
		{
			var file = new FileInfo(configFile);
			if (!file.Exists)
			{
				Console.WriteLine("not exists");
				return null;
			}

			var bundles = JsonConvert.DeserializeObject<IEnumerable<Bundle>>(File.ReadAllText(configFile));
			GPSMonitoreo.Libraries.Utils.ObjectJsonDumper.Dump(bundles, 1);
			Console.WriteLine(bundlePath);
			return (from b in bundles
					where b.OutputFileName.EndsWith(bundlePath, StringComparison.InvariantCultureIgnoreCase)
					select b).FirstOrDefault();
		}

		class Bundle
		{
			[JsonProperty("outputFileName")]
			public string OutputFileName { get; set; }

			[JsonProperty("inputFiles")]
			public List<string> InputFiles { get; set; } = new List<string>();
		}
	}
}