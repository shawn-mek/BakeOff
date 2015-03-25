using System;
using Lsr.Bio.Benchmark.ScreenScorer;
using Lsr.Bio.Benchmark.ScreenSimulator;

namespace Lsr.Bio.App.Benchmark {
    class Program {
        static void Main(string[] args) {
			if (args.Length < 1) {
				Program._PrintInstructions();
				return;
			} //end if incorrect number of args

            try {
            	string action = args[0];
            	switch (action) {
					case "simulate":
						if (args.Length != 6) {
							Program._PrintInstructions();
							return;
						}

						SimulationManager manager = new SimulationManager();
						manager.GenerateNoisyScreens(args);
            			break;
					case "score":
						if (args.Length != 5) {
							Program._PrintInstructions();
							return;							
						}

						ScoringManager scoreManager = new ScoringManager();
						//figure out--where are we getting the input args?
						scoreManager._Score(args);

            			break;
					case "analyze":
						//go over the scoring results and apply decision criteria to pick best scoring per scenario
						//not yet implemented ... not yet really planned out
            			break;
            	} //end switch
            } catch (Exception ex) {
                Console.WriteLine(string.Format("Error:{0}",
                    ex.Message));
            } //end try/catch
        } //end Main

		private static void _PrintInstructions() {
			System.Console.WriteLine(@"
Expected command line parameters:
simulate <working directory> <plate map file name> <screen infos file name> <hit set infos file name> <noise set infos file name>
or
analyze <simulation file path> <number of plates> <number of rows> <number of columns>
			");			
		} //end _PrintInstructions
    }
}
