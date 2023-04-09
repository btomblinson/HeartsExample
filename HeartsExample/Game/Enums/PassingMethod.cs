using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartsExample.Game.Enums
{
    public enum PassingMethod
    {
        [Description("0")] None,

        [Description("1")] Left,

        [Description("2")] Right,

        [Description("3")] Across
    }
}