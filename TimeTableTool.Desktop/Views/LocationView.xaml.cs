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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimetableTool.Desktop.Views
	{
	/// <summary>
	/// Interaction logic for LocationView.xaml
	/// </summary>
	public partial class LocationView : UserControl
		{
		private int rowCount;
		public LocationView()
			{
			InitializeComponent();
			}

		//Scroll to last row if a row is added
		private void OnLoadingRow(object sender, DataGridRowEventArgs e)
			{
			if (LocationList.Items.Count > rowCount)
				{
				var lastRow = LocationList.Items[^1];
				LocationList.ScrollIntoView(lastRow);
				rowCount = LocationList.Items.Count;
				}
			}
		}
	}
