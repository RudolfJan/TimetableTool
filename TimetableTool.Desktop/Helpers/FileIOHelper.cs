using Logging.Library;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TimetableTool.Desktop.Models;

namespace TimetableTool.Desktop.Helpers
	{
	public class FileIOHelper
		{
		public static string GetSaveFileName(SaveFileModel saveFileParams)
			{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.CheckFileExists=saveFileParams.CheckFileExists;
			dialog.CheckPathExists=saveFileParams.CheckPathExists;
			dialog.CreatePrompt=saveFileParams.CreatePrompt;
			dialog.CustomPlaces=saveFileParams.CustomPlaces;
			dialog.DefaultExt=saveFileParams.DefaultExt;
			dialog.DereferenceLinks=saveFileParams.DereferenceLinks;
			dialog.FileName=saveFileParams.FileName;
			// FileNames not supported!
			dialog.Filter= saveFileParams.Filter;
			dialog.FilterIndex= saveFileParams.FilterIndex;
			dialog.InitialDirectory = saveFileParams.InitialDirectory;
			dialog.OverwritePrompt=saveFileParams.OverWriteprompt;
			dialog.Title=saveFileParams.Title;
			if(dialog.ShowDialog()==true)
				{
				saveFileParams.FileName=dialog.FileName;
				saveFileParams.SafeFileName=dialog.SafeFileName;
				saveFileParams.SafeFileNames=dialog.SafeFileNames;
				return dialog.FileName;
				}
			return "";
			}

		public static void OpenFileWithShell(string FilePath)
			{
			 try
        {
        if (File.Exists(FilePath))
          {
          using (var OpenFileProcess = new Process())
            {
            OpenFileProcess.StartInfo.FileName = "explorer.exe";
            OpenFileProcess.StartInfo.Arguments = QuoteFilename(FilePath);
            OpenFileProcess.StartInfo.UseShellExecute=true;
            OpenFileProcess.StartInfo.RedirectStandardOutput = false;
            OpenFileProcess.Start();
            }
          }
        }
      catch (Exception E)
        {
        Log.Trace("Cannot open file " + FilePath + " reason: " + E.Message,
          LogEventType.Error);
        }

      Log.Trace("Cannot find file " + FilePath +
                        " \r\nMake sure to install it at the correct location");
			}

		    // Add quotes to a filename in case it contains spaces. If the filepath is already quoted, don't do it again 
    public static string QuoteFilename(string s)
      {
      if(s.StartsWith("\"") && s.EndsWith("\""))
        {
        return s; // already quoted
        }
      else
        return $"\"{s}\"";
      }
		}
	}
