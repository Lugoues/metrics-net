using System;

namespace metrics.Reporting
{
    public class ConsoleReporter : TextWriterReporter
    {
        public ConsoleReporter() : base(Console.Out)
        {
            
        }

        public ConsoleReporter(IReportFormatter formatter) : base(Console.Out, formatter)
        {

        }
    }
}
