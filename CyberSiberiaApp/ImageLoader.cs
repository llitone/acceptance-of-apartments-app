using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.IO;

namespace CyberSiberiaApp
{
	public class ImageLoader
	{
		private dynamic _python_get_file_name_function;
        private ScriptEngine _engine;
		private ScriptScope _scope;
        private string _filePath = "";

        public ImageLoader()
		{

            this._engine = Python.CreateEngine();
            this._scope = this._engine.CreateScope();
            this._engine.ExecuteFile("worker.py", this._scope);
            this._python_get_file_name_function = this._scope.GetVariable("get_file_name");
        }

		public string convert_image(string filename)
		{
			string name = this._python_get_file_name_function(filename);
            if (!Directory.Exists("/_images"))
			{
				Directory.CreateDirectory("/_images");
			}
			File.Copy(filename, string.Format("/_images/{0}", name));
            return name;
		}
	}
	public static class MainClass
	{
        public static void Main()
        {
			ImageLoader loader = new ImageLoader();
			loader.convert_image("IMG_4149.heic");
        }
    }
}

