using Infragistics.Controls.Schedules;
using KB11362_XamScheduleViewScroll_WpfApp1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KB11362_XamScheduleViewScroll_WpfApp1;
internal class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<Resource> Resources { get; set; }
    public ObservableCollection<ResourceCalendar> Calendars { get; set; }
    public ObservableCollection<Appointment> Appointments { get; set; }
    public MainWindowViewModel()
    {
        Resources = new ObservableCollection<Resource>();
        Calendars = new ObservableCollection<ResourceCalendar>();
        Appointments = new ObservableCollection<Appointment>();

        Resources.Add(new Resource() { Id = "res1" });
        Resources.Add(new Resource() { Id = "res2" });
        Resources.Add(new Resource() { Id = "res3" });
        Calendars.Add(new ResourceCalendar() { Id = "cal1", OwningResourceId = "res1" });
        Calendars.Add(new ResourceCalendar() { Id = "cal2", OwningResourceId = "res2" });
        Calendars.Add(new ResourceCalendar() { Id = "cal3", OwningResourceId = "res3" });
    }

}
