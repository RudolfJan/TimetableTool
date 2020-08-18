using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TimetableTool.Desktop.ViewModels;

namespace TimetableTool.Desktop.Views
	{
	/// <summary>
	/// Interaction logic for StatisticsGraphForm.xaml
	/// </summary>
	public partial class StatisticsGraphForm
		{
		public StatisticsGraph Statistics { get; set; }

		public StatisticsGraphForm(StatisticsGraph statistics)
			{
			InitializeComponent();
			Statistics = statistics;
			DataContext = Statistics;
			}



		private void WindowLoaded(object sender, RoutedEventArgs e)
			{
			MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight * 0.9;
			MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
			//CreateSearchablePointsList();
			//RedrawPlot();
			}

		private void Exit_Click(object sender, RoutedEventArgs e)
			{
			Close();
			}



		}
	}
