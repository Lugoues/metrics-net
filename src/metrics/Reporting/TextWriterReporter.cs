using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace metrics.Reporting
{
    public class TextWriterReporter : ReporterBase
    {
        protected TextWriter Out;
        private readonly IReportFormatter _formatter;

        protected TextWriterReporter(TextWriter writer)
            : this(writer, new HumanReadableReportFormatter())
        {
            Out = writer;
        }

        protected TextWriterReporter(TextWriter writer, IReportFormatter formatter)
        {
            Out = writer;
            _formatter = formatter;
        }

        public override void Run()
        {
            try
            {
                Out.Write(_formatter.GetSample());

                Out.Flush();
            }
            catch (Exception e)
            {
                Out.WriteLine(e.StackTrace);
            }
        }

        public override void Dispose()
        {

            if (Out != null)
            {
                Out.Close();
            }

            base.Dispose();
        }
    }
}