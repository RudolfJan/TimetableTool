using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

		}
	}
