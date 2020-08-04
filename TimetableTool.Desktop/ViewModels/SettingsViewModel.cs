using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TimetableTool.Desktop.ViewModels
	{
	public class SettingsViewModel: Screen
		{
		public int ScottPlotWidth { get; set; }
		public int ScottPlotHeight { get; set; }
		protected override void OnViewLoaded(object view)
			{
			base.OnViewLoaded(view);
			ScottPlotWidth = Settings.ScottPlotWidth;
			ScottPlotHeight = Settings.ScottPlotHeight;
			NotifyOfPropertyChange(()=>ScottPlotHeight);
			NotifyOfPropertyChange(()=>ScottPlotWidth);
			}


		public async Task Save()
			{
			Settings.ScottPlotWidth = ScottPlotWidth;
			Settings.ScottPlotHeight = ScottPlotHeight;
			Settings.WriteToRegistry();
			await TryCloseAsync();
			}
		public async Task Cancel()
			{
			await TryCloseAsync();
			}
		}
	}
