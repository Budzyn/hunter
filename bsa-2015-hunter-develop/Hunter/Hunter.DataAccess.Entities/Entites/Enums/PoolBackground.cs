using System;
using System.Collections.Generic;

namespace Hunter.DataAccess.Entities.Enums
{
    public sealed class PoolBackground
    {
        public string color { get; private set; }
        public string code { get; private set; }

        public static readonly List<PoolBackground> BgColors = new List<PoolBackground>
        {
            new PoolBackground{color = "Green", code = "rgb(44,201,99)"},
            new PoolBackground{color = "Yellow", code = "rgb(293,250,85)"},
            new PoolBackground{color = "Red", code = "rgb(240,88,88)"},
            new PoolBackground{color = "Orange", code = "rgb(245,122,14)"},
            new PoolBackground{color = "Aquamarine", code = "rgb(76,222,181)"},
            new PoolBackground{color = "Purple", code = "rgb(181,60,181)"},
            new PoolBackground{color = "Blue", code = "rgb(87,166,235)"}
        };
    }
}
