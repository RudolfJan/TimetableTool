using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimetableTool.Desktop.Views
	{
	/// <summary>
	/// Interaction logic for TimeEventView.xaml
	/// </summary>
	public partial class TimeEventView : UserControl
		{
		private int rowCount;
		public TimeEventView()
			{
			InitializeComponent();
			}

		//Scroll to last row if a row is added
		private void OnLoadingRow(object sender, DataGridRowEventArgs e)
			{
			if (EventTimeEventDataGrid.Items.Count > rowCount)
				{
				var lastRow = EventTimeEventDataGrid.Items[^1];
				EventTimeEventDataGrid.ScrollIntoView(lastRow);
				rowCount = EventTimeEventDataGrid.Items.Count;
				}

			}
		}
	}
