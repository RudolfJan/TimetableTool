using DataAccess.Library.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Library.Logic;
using System.Data;

namespace TimetableTool.Desktop.ViewModels
	{
	public class DisplayTimetableViewModel: Screen
		{
		public TimetableMatrixModel TimetableMatrix { get; set; }= new TimetableMatrixModel();
		public DataView	SimpleTimetable { get; set; }
		public int TimetableId { get; set; }= -1;

		public DisplayTimetableViewModel()
			{

			}

		protected override void OnViewLoaded(object view)
			{
			base.OnViewLoaded(view);
			var matrix=TimetableMatrixDataAccess.ReadTimetableMatrix(TimetableId,false);
			SimpleTimetable= TimetableMatrixDataAccess.MatrixToDataTable(matrix.Matrix);
			NotifyOfPropertyChange(()=>SimpleTimetable );
			}

			}
	}
