using System.IO;
using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.RAnalysis {
    public abstract class RnaitherScriptRunner<T>: RscriptRunner {
        protected readonly string DATASET_LOAD_SCRIPT_TEMPLATE = @"suppressPackageStartupMessages(library(""RNAither""))
header <- readLines(""{0}"", 3)
dataset <- read.table(""{0}"", skip=3, colClasses=c(NA, NA, NA, NA, ""factor"", NA, NA, NA, NA, NA, NA, NA, NA), stringsAsFactors=FALSE)
";
        protected string DatasetFilePath = @"c:\amanda\temprnaitherdataset.txt";
        protected RnaitherDatasetGenerator DatasetGenerator;
		protected KeyedPlatesetInfos<T> PlatesetsToOutput;

        protected RnaitherScriptRunner (RnaitherDatasetGenerator datasetGenerator,
			KeyedPlatesetInfos<T> platesetsToOutput) {

            this.DatasetGenerator = datasetGenerator;
            this.PlatesetsToOutput = platesetsToOutput;
        } //end constructor

        protected override TextReader Run() {
            TextReader result;
            string datasetText = this.DatasetGenerator.GenerateDatasetText(this.PlatesetsToOutput);
            File.WriteAllText(this.DatasetFilePath, datasetText); //overwrites any previous versions of file
            result = base.Run();
            return result;
        } //end Run override
    } //end class
}
