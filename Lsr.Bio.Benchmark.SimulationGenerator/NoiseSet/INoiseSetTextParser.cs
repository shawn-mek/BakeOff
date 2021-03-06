﻿using System.Collections.Generic;
using System.IO;

namespace Lsr.Bio.Benchmark.ScreenSimulator.NoiseSet {
    /// <summary>
    /// Interface defining the method(s) for loading noise set info from a text source
    /// </summary>
    internal interface INoiseSetTextParser {
        List<NoiseSetInfo> LoadNoiseSetInfo(TextReader reader);
    } //end INoiseSetTextParser
}
