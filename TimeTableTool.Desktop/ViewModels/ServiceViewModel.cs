using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System.Linq;
using System.Threading.Tasks;
using TimetableTool.DataAccessLibrary.Logic;

namespace TimetableTool.Desktop.ViewModels
  {
  public class ServiceViewModel : Screen
    {
    private IWindowManager _windowManager;

    #region Properties

    private string _startTimeText;

    private string _endTimeText;
    private ServiceModel _selectedService;
    private string _serviceName;
    private string _serviceAbbreviation;
    private ServiceTemplateModel _serviceTemplate;
    private ServiceTemplateModel _selectedServiceTemplate;

    public int RouteId { get; set; }
    public int ServiceId { get; set; }
    public string RouteName { get; set; }
    public string TimetableName { get; set; }

    public string ServiceName
      {
      get
        {
        return _serviceName;
        }
      set
        {
        _serviceName = value;
        NotifyOfPropertyChange(()=> ServiceName);
        NotifyOfPropertyChange(() => CanSaveService);
        }
      }

    public string ServiceAbbreviation
      {
      get
        {
        return _serviceAbbreviation;
        }
      set
        {
        _serviceAbbreviation = value;
        NotifyOfPropertyChange(() => ServiceAbbreviation);
        NotifyOfPropertyChange(() => CanSaveService);
        }
      }

    public BindableCollection<ServiceTemplateModel> ServiceTemplateList { get; set; }
    public BindableCollection<ServiceModel> ServiceList { get; set; }

    public ServiceModel SelectedService
      {
      get
        {
        return _selectedService;
        }
      set
        {
        _selectedService = value;
        ServiceId = 0;
        NotifyOfPropertyChange(() => CanEditService);
        NotifyOfPropertyChange(() => CanDeleteService);
        NotifyOfPropertyChange(() => CanRepeat);
        }
      }

    public ServiceTemplateModel SelectedServiceTemplate
      {
      get
        {
        return _selectedServiceTemplate;
        }
      set
        {
        _selectedServiceTemplate = value;
        NotifyOfPropertyChange(()=>CanSelectServiceTemplate);
        }
      }

    public ServiceTemplateModel ServiceTemplate
      {
      get
        {
        return _serviceTemplate;
        }
      set
        {
        _serviceTemplate = value;
        NotifyOfPropertyChange(() => ServiceTemplate);
        NotifyOfPropertyChange(() => CanSaveService);
        NotifyOfPropertyChange(() => CanAutoFill);
        }
      }

    public string StartTimeText
      {
      get { return _startTimeText; }
      set
        {
        _startTimeText = value;
        NotifyOfPropertyChange(() => StartTimeText);
        NotifyOfPropertyChange(() => CanSaveService);
        NotifyOfPropertyChange(() => CanAutoFill);
        }
      }

    public string EndTimeText
      {
      get { return _endTimeText; }
      set
        {
        _endTimeText = value;
        NotifyOfPropertyChange(() => EndTimeText);
        NotifyOfPropertyChange(() => CanSaveService);
        }
      }

    public bool CanAutoFill
      {
      get
        {
        return ServiceTemplate!=null && StartTimeText?.Length==5;
        }
      }

    private int _digits=3;
    public int Digits
      {
      get { return _digits; }
      set
        {
        _digits = value;
        NotifyOfPropertyChange(()=>Digits);
        NotifyOfPropertyChange(()=>CanRepeat);
        }
      }
     
    private string _separator="-";

    public string Separator
      {
      get { return _separator; }
      set
        {
        _separator = value;
        NotifyOfPropertyChange(()=> Separator);
        NotifyOfPropertyChange(()=>CanRepeat);
        }
      }

    private int _numberStart=1;

    public int NumberStart
      {
      get { return _numberStart; }
      set
        {
        _numberStart = value;
        NotifyOfPropertyChange(()=> NumberStart);
        NotifyOfPropertyChange(()=>CanRepeat);

        }
      }

    private int _numberOffset=1;
    public int NumberOffset
      {
      get { return _numberOffset; }
      set
        {
        _numberOffset = value;
        NotifyOfPropertyChange(()=>NumberOffset);
        NotifyOfPropertyChange(()=>CanRepeat);
        }
      }

    private int _timeOffset;

    public int TimeOffset
      {
      get { return _timeOffset; }
      set
        {
        _timeOffset = value; 
        NotifyOfPropertyChange(()=>TimeOffset);
        NotifyOfPropertyChange(()=>CanRepeat);
        }
      }

    private int _repeatCount=1;

    public int RepeatCount
      {
      get { return _repeatCount; }
      set
        {
        _repeatCount = value;
        NotifyOfPropertyChange(()=>RepeatCount);
        NotifyOfPropertyChange(()=>CanRepeat);
        }
      }

    public bool CanRepeat {
      get
        {
        return TimeOffset>0 && RepeatCount>0 && SelectedService!=null;
        }
      }

    #endregion
    #region Initialize

    public ServiceViewModel(IWindowManager windowManager)
      {
      _windowManager = windowManager;
      }
    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      RouteModel rm = RouteDataAccess.GetRouteById(RouteId);
      RouteName = rm.RouteName;
      ServiceTemplateList = new BindableCollection<ServiceTemplateModel>(ServiceTemplateDataAccess.GetServiceTemplatesPerRoute(RouteId));
      ServiceList = new BindableCollection<ServiceModel>(ServicesDataAccess.GetServicesPerRoute(RouteId).OrderBy(x=>x.StartTime));
      NotifyOfPropertyChange(() => ServiceTemplateList);
      NotifyOfPropertyChange(() => ServiceList);
      NotifyOfPropertyChange(() => RouteName);
      NotifyOfPropertyChange(() => TimetableName);
      }

    #endregion

    public bool CanEditService
      {
      get { return SelectedService != null && ServiceId <= 0; }
      }

    public bool CanDeleteService
      {
      get { return SelectedService != null  && Settings.DatabaseVersion>=2; }
      }

    public void DeleteService()
      {
      ServicesDataAccess.DeleteService(SelectedService.Id);
      ServiceList.Remove(SelectedService);
      ServiceId = 0;
      }

    public void SelectServiceTemplate()
      {
      ServiceTemplate = SelectedServiceTemplate;
      }

    public void AutoFill()
      {
      ServiceName = ServiceTemplate.ServiceTemplateName;
      ServiceAbbreviation = ServiceTemplate.ServiceTemplateAbbreviation;
      int StartTime = TimeConverters.TimeToMinutes(StartTimeText);
      EndTimeText = TimeConverters.MinutesToString(StartTime + ServiceTemplate.CalculatedDuration);
      NotifyOfPropertyChange(()=> CanSaveService);
      }

    public async Task Repeat()
      {
      ServiceId = SelectedService.Id;
      ServiceTemplate = ServiceTemplateDataAccess.GetServiceTemplateById(SelectedService.ServiceTemplateId);
      ReviewServicesViewModel reviewVM = IoC.Get<ReviewServicesViewModel>();

      for (int i = 0; i < RepeatCount; i++)
        {
        var newService=new ServiceModel();
        newService.ServiceTemplateId = ServiceTemplate.Id;
        newService.ServiceName = SelectedService.ServiceName;
        newService.StartTime = SelectedService.StartTime + (i+1) * TimeOffset;
        newService.EndTime=SelectedService.EndTime+  (i+1) * TimeOffset;
        int serviceNumber = NumberStart+ i * NumberOffset;
        string offset = serviceNumber.ToString($"D{Digits}");
        newService.ServiceAbbreviation= $"{ServiceTemplate.ServiceTemplateAbbreviation}{Separator}{offset}";
        reviewVM.CreatedServices.Add(newService);
        }

      reviewVM.CreatedServices = new BindableCollection<ServiceModel>(reviewVM.CreatedServices.OrderBy(x => x.StartTime));
      await _windowManager.ShowDialogAsync(reviewVM);
      if (reviewVM.IsSaved)
        {
        ServiceList = new BindableCollection<ServiceModel>(ServicesDataAccess
          .GetServicesPerRoute(RouteId).OrderBy(x => x.StartTime));
        NotifyOfPropertyChange(() => ServiceList);
        }
      NotifyOfPropertyChange(() => CanRepeat);
      }

    public void EditService()
      {
      ServiceName = SelectedService.ServiceName;
      ServiceAbbreviation = SelectedService.ServiceAbbreviation;
      ServiceTemplate = ServiceTemplateDataAccess.GetServiceTemplateById(SelectedService.ServiceTemplateId);
      ServiceId = SelectedService.Id;
      StartTimeText =TimeConverters.MinutesToString(SelectedService.StartTime);
      EndTimeText = TimeConverters.MinutesToString(SelectedService.EndTime);
      NotifyOfPropertyChange(() => CanEditService);
      NotifyOfPropertyChange(() => CanSaveService);
      NotifyOfPropertyChange(() => CanRepeat);
      }

    public bool CanSelectServiceTemplate
      {
      get { return SelectedServiceTemplate != null; }
      }

    public bool CanSaveService
      {
      get
        {
        return ServiceTemplate != null
               && ServiceName?.Length > 0
               && ServiceAbbreviation?.Length > 0;
        }
      }



    public void SaveService()
      {
      ServiceModel newService = new ServiceModel();
      newService.ServiceName = ServiceName;
      newService.ServiceAbbreviation = ServiceAbbreviation;
      newService.ServiceTemplateId = ServiceTemplate.Id;
      newService.StartTime = TimeConverters.TimeToMinutes(StartTimeText);
      newService.EndTime = TimeConverters.TimeToMinutes(EndTimeText);
      if (ServiceId <= 0)
        {
        ServicesDataAccess.InsertService(newService);
        }
      else
        {
        newService.Id = ServiceId;
        ServicesDataAccess.UpdateService(newService);
        }
      ServiceList = new BindableCollection<ServiceModel>(ServicesDataAccess.GetServicesPerRoute(RouteId).OrderBy(x=>x.StartTime));
      ClearService();
      NotifyOfPropertyChange(() => ServiceList);
      }


    public void ClearService()
      {
      ServiceTemplate = null;
      StartTimeText = "";
      EndTimeText = "";
      ServiceName = "";
      ServiceAbbreviation = "";
      ServiceId = 0;
      NotifyOfPropertyChange(() => CanRepeat);
      }
    }
  }
