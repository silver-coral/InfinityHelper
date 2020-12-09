using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class Logger
    {
        private static readonly TraceSource _traceSource = new System.Diagnostics.TraceSource("MyLog");

        public static void Info(string msgFormat, params object[] args)
        {
            if (args.Count() > 0)
            {
                msgFormat = string.Format(msgFormat, args);
            }
            _traceSource.TraceEvent(TraceEventType.Information, 0, "[{0:yyyy-MM-dd HH:mm:ss:fff}]-{1}", DateTime.Now, msgFormat);
        }

        public static void Debug(string msgFormat, params object[] args)
        {
            if (args.Count() > 0)
            {
                msgFormat = string.Format(msgFormat, args);
            }
            _traceSource.TraceEvent(TraceEventType.Verbose, 0, "[{0:yyyy-MM-dd HH:mm:ss:fff}]-{1}", DateTime.Now, msgFormat);
        }

        public static void Error(Exception ex)
        {
            _traceSource.TraceEvent(TraceEventType.Error, 0, "[{0:yyyy-MM-dd HH:mm:ss:fff}]-{1}", DateTime.Now, ex.Message);
            LogFile(ex.ToString());
        }

        public static void Error(string msgFormat, params object[] args)
        {
            if (args.Count() > 0)
            {
                msgFormat = string.Format(msgFormat, args);
            }
            _traceSource.TraceEvent(TraceEventType.Error, 0, "[{0:yyyy-MM-dd HH:mm:ss:fff}]-{1}", DateTime.Now, msgFormat);
            LogFile(msgFormat);
        }

        private static void LogFile(string msg)
        {
            DateTime now = DateTime.Now;
            try
            {
                string fileDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!System.IO.Directory.Exists(fileDirectory))
                {
                    System.IO.Directory.CreateDirectory(fileDirectory);
                }
                string fileName = string.Format("{0:yyyy-MM-dd}.txt", now);
                string filePath = System.IO.Path.Combine(fileDirectory, fileName);
                string fileContent = string.Format("[{0:yyyy-MM-dd HH:mm:ss:fff}]-{1}\r\n", now, msg);
                System.IO.File.AppendAllLines(filePath, new List<string>() { fileContent }, System.Text.Encoding.UTF8);
            }
            catch
            {

            }
        }
    }
}
