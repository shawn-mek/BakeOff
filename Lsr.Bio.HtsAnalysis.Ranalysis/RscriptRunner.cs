using System.IO; //for File
using System.Diagnostics; //for Process

namespace Lsr.Bio.HtsAnalysis.RAnalysis {
    public abstract class RscriptRunner {
        protected string OutputFilePath = @"c:\amanda\temprscript_output.txt";
        protected string ScriptFilePath = @"c:\amanda\temprscript.R";
        protected string RscriptExePath = @"c:\Program Files\R\R-2.13.0\bin\Rscript.exe";

        protected abstract string GenerateRscript();

        protected virtual TextReader Run() {
            StreamReader result;
            string rScript = this.GenerateRscript();
            rScript = rScript.Replace('\\', '/'); //R uses the other direction of slashes for file paths than dos
            File.WriteAllText(this.ScriptFilePath, rScript); //overwrites any previous versions of file

            this.RunRscript(this.ScriptFilePath);

            result = File.OpenText(this.OutputFilePath);
            return result;
        } //end Run

        protected void RunRscript(string scriptFilePath) {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = this.RscriptExePath;
            p.StartInfo.Arguments = scriptFilePath;
            p.Start();
            p.WaitForExit();
        } //end RunRscript
    } //end class
}
