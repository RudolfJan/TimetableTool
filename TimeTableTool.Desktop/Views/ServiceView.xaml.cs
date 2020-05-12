using System.Windows.Controls;

namespace TimetableTool.Desktop.Views
	{
	/// <summary>
	/// Interaction logic for ServiceView.xaml
	/// </summary>

	public partial class ServiceView

		{
		private int rowCount;
		public ServiceView()
			{
			InitializeComponent();
			}

	private void OnLoadingRow(object sender, DataGridRowEventArgs e)
			{
			if (ServiceListDataGrid.Items.Count > rowCount)
				{
				var lastRow = ServiceListDataGrid.Items[^1];
				ServiceListDataGrid.ScrollIntoView(lastRow);
				rowCount = ServiceListDataGrid.Items.Count;
				}
			}
		}
	}
