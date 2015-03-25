using System.Collections.Generic;
using System.IO;

namespace Lsr.Bio.HtsAnalysis.Simulations {
    /// <summary>
    /// Interface defining the method(s) for loading simulated screen info from a text source
    /// </summary>
    internal interface IScreenTextParser {
        List<ScreenInfo> LoadScreenInfo(TextReader reader);
    } //end IScreenTextParser
}
