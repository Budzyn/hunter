using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace Hunter.DataAccess.Entities
{
    public enum Stage
    {
        [Description("Long Listed")]
        LongListed,
        [Description("Test Sent")]
        TestSent,
        [Description("Test Done")]
        TestDone,
        Interview,
        [Description("Test Failed")]
        TestFailed,
        [Description("Interview Failed")]
        InterviewFailed,
        Passed
    }
}