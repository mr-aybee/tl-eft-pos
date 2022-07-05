using System;
using System.IO;
using System.Web;

namespace FTPPOSService
{

	public static class Logger
	{
		public static Logging Logging { get; set; } = new Logging();

		public static void Log(string Text)
		{
			Logging.WriteLogToFile($"{DateTime.Now}[][]{Text}");
		}


		public static void LogQuery(string Source, string Query, string Param, int ParamCount, string IP)
		{
			Logging.WriteLogToFile($"{Source}[]---------------\r\n" +
				$"{DateTime.Now}[]\r\n" +
				$"Query[]{Query}\r\n" +
				$"Param[{ParamCount}]{Param}\r\n" +
				$"IP[]{IP}");
		}

		public static void Error(string Message)
		{
			Logging.WriteErrorToFile($"{DateTime.Now}[][]{Message}");
		}

		public static void Error(Exception Ex)
		{
			Logging.WriteErrorToFile(
				$"{DateTime.Now}[]\r\n" +
				$"Message: {Ex.Message}\r\n" +
				$"StackTrace: {Ex.StackTrace}\r\n"
			);
		}

		public static void ExecutionEntry(string Text)
		{
			Logging.WriteExecutionEntryToFile($"{DateTime.Now}[][]{Text}\r\n");
		}

	}


	public class Logging
	{
		public Logging() { }

		private static readonly object locker = new object();



		public void WriteLogToFile(string message)
		{
			lock (locker)
			{
				//var dirLog = HttpContext.Current.Server.MapPath($"{HttpContext.Current.Request.ApplicationPath}/Logs");

				StreamWriter SW;
				SW = File.AppendText($"Logs {DateTime.Now.ToString("yyyyMM")}.txt");
				SW.WriteLine(message);
				SW.Close();
				if (SW != null)
					SW.Dispose();
			}
		}

		public void WriteExecutionEntryToFile(string message)
		{
			lock (locker)
			{
				StreamWriter SW;
				SW = File.AppendText($"Executions {DateTime.Now.ToString("yyyyMM")}.txt");
				SW.WriteLine(message);
				SW.Close();
				if (SW != null)
					SW.Dispose();
			}
		}

		public void WriteErrorToFile(string message)
		{
			lock (locker)
			{
				StreamWriter SW;
				SW = File.AppendText($"ErrorLogs {DateTime.Now.ToString("yyyyMM")}.txt");
				SW.WriteLine(message);
				SW.Close();
				if (SW != null)
					SW.Dispose();
			}
		}
	}
}
