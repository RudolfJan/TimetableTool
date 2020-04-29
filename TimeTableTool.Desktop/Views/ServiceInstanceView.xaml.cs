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
	/// Interaction logic for ServiceInstanceView.xaml
	/// </summary>
	
	public partial class ServiceInstanceView : UserControl
		{
		private int rowCount;
		public ServiceInstanceView()
			{
			InitializeComponent();
			}

	private void OnLoadingRow(object sender, DataGridRowEventArgs e)
			{
			if (ServiceInstanceListDataGrid.Items.Count > rowCount)
				{
				var lastRow = ServiceInstanceListDataGrid.Items[^1];
				ServiceInstanceListDataGrid.ScrollIntoView(lastRow);
				rowCount = ServiceInstanceListDataGrid.Items.Count;
				}
			}
		}
	}
