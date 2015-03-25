using System.Collections.Generic;
using System.IO;

namespace Lsr.Bio.Benchmark.ScreenSimulator.Screen {
    /// <summary>
    /// Interface defining the method(s) for loading simulated screen info from a text source
    /// </summary>
    public interface IScreenTextParser {
        List<ScreenInfo> LoadScreenInfo(TextReader reader);
    } //end IScreenTextParser
}
