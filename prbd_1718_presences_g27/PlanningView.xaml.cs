using Microsoft.Win32;
using PRBD_Framework;

using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Input;

using System;
using System.Collections.Generic;
using System.Data;

using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace prbd_1718_presences_g27
{

    public partial class PlanningView : UserControlBase
    {
        public ICommand DisplayPresence { get; set; }
        public ICommand DisplayPlanningMonday { get; set; }
        public ICommand DisplayPlanningTuesday { get; set; }
        public ICommand DisplayPlanningWednesday { get; set; }
        public ICommand DisplayPlanningThursday { get; set; }
        public ICommand DisplayPlanningFriday { get; set; }
        public ICommand DisplayPlanningSaturday { get; set; }
        public ICommand NextWeek { get; set; }
        public ICommand PreviousWeek { get; set; }      
        private DateTime dbegin = DateTime.Today;
        private DateTime datesBeginPlanning;
        public DateTime DatesBeginPlanning
        {
            get
            {

                return datesBeginPlanning;
            }
            set
            {

                datesBeginPlanning = (DateTime)value;
                RaisePropertyChanged(nameof(DatesBeginPlanning));
            }
        }
        private ObservableCollection<Course> mondayPlanning;
        public ObservableCollection<Course> MondayPlanning
        {
            get { return mondayPlanning; }
            set
            {
                mondayPlanning = value;
                RaisePropertyChanged(nameof(MondayPlanning));
            }
        }
        private ObservableCollection<Course> tuesdayPlanning;
        public ObservableCollection<Course> TuesdayPlanning
        {
            get { return tuesdayPlanning; }
            set
            {
                tuesdayPlanning = value;
                RaisePropertyChanged(nameof(TuesdayPlanning));
            }
        }
        private ObservableCollection<Course> wednesdayPlanning;
        public ObservableCollection<Course> WednesdayPlanning
        {
            get { return wednesdayPlanning; }
            set
            {
                wednesdayPlanning = value;
                RaisePropertyChanged(nameof(WednesdayPlanning));
            }
        }
        private ObservableCollection<Course> thursdayPlanning;
        public ObservableCollection<Course> ThursdayPlanning
        {
            get { return thursdayPlanning; }
            set
            {
                thursdayPlanning = value;
                RaisePropertyChanged(nameof(ThursdayPlanning));
            }
        }
        private ObservableCollection<Course> fridayPlanning;
        public ObservableCollection<Course> FridayPlanning
        {
            get { return fridayPlanning; }
            set
            {
                fridayPlanning = value;
                RaisePropertyChanged(nameof(FridayPlanning));
            }
        }
        private ObservableCollection<Course> saturdayPlanning;
        public ObservableCollection<Course> SaturdayPlanning
        {
            get { return saturdayPlanning; }
            set
            {
                saturdayPlanning = value;
                RaisePropertyChanged(nameof(SaturdayPlanning));
            }
        }      
        public List<String> listDay = new List<String>();
        public PlanningView()
        {
            InitializeComponent();
            DataContext = this;
            DateTime input = dbegin;
            int delta = DayOfWeek.Monday - input.DayOfWeek;
            DateTime monday = input.AddDays(delta);
            DatesBeginPlanning = monday;
            DisplayPlanningMonday = new RelayCommand<Course>(course => {showPresence(course,0);});
            DisplayPlanningTuesday = new RelayCommand<Course>(course => { showPresence(course,1);});
            DisplayPlanningWednesday = new RelayCommand<Course>(course => {showPresence(course,2);});
            DisplayPlanningThursday = new RelayCommand<Course>(course => {showPresence(course,3);});
            DisplayPlanningFriday = new RelayCommand<Course>(course => {showPresence(course,4);});
            DisplayPlanningSaturday = new RelayCommand<Course>(course => {showPresence(course,5);});
            NextWeek = new RelayCommand(NextWeekAction);
            PreviousWeek = new RelayCommand(PreviousWeekAction);
            listDay.Add("lundi"); listDay.Add("mardi"); listDay.Add("mercredi"); listDay.Add("jeudi"); listDay.Add("vendredi"); listDay.Add("samedi");

            DisplayPlanning(DatesBeginPlanning);
            


        }
        private void showPresence(Course course,int toDay)
        {
            
            int idOccurence = 0;
            DateTime theDay = DatesBeginPlanning;
            theDay = theDay.AddDays(toDay);
            foreach (var val in App.Model.courseoccurrence)
            {
                if (val.Course.Code == course.Code && val.Date.ToString("dd-MM-yyyy") == theDay.ToString("dd-MM-yyyy"))
                    idOccurence = val.Id;
            }
            App.Messenger.NotifyColleagues(App.MSG_DISPLAY_PRESENCE, idOccurence);
        }
        private void DisplayPlanning( DateTime DatesBeginPlanning)
        {
            App.DatesBeginPlanning = DatesBeginPlanning;
            var theday = from c in App.Model.course
                         where c.User.Fullname.Equals(App.CurrentUser.Fullname) 
                         select c.Dayofweek;
            foreach(var realDay in theday)
            {
                var q = from c in App.Model.course
                        where c.User.Fullname.Equals(App.CurrentUser.Fullname)
                        && c.Startdate <= DatesBeginPlanning && c.Finishdate >= DatesBeginPlanning && c.Dayofweek== realDay

                        select c;
                    if (realDay == 0)
                    {
                        MondayPlanning = new ObservableCollection<Course>(q);
                    }
                    else if (realDay == 1)
                    {
                        TuesdayPlanning = new ObservableCollection<Course>(q);
                    }
                    else if (realDay == 2)
                    {
                        WednesdayPlanning = new ObservableCollection<Course>(q);
                    }
                    else if (realDay == 3)
                    {
                        ThursdayPlanning = new ObservableCollection<Course>(q);
                    }
                    else if (realDay == 4)
                    {
                        FridayPlanning = new ObservableCollection<Course>(q);
                    }
                    else if (realDay == 5)
                    {
                        SaturdayPlanning = new ObservableCollection<Course>(q);
                    }

            }
            var theCourse = from c in App.Model.course
                         where c.User.Fullname.Equals(App.CurrentUser.Fullname)
                         select c;
            foreach( var cur in theCourse)
            {
                if(cur.Startdate> DatesBeginPlanning || cur.Finishdate<DatesBeginPlanning)
                {
                    if(cur.Dayofweek==0)
                        MondayPlanning = new ObservableCollection<Course>(); 
                    else if (cur.Dayofweek == 1)
                        TuesdayPlanning = new ObservableCollection<Course>();
                    else if(cur.Dayofweek == 2)
                        WednesdayPlanning = new ObservableCollection<Course>();
                    else if(cur.Dayofweek == 3)
                        ThursdayPlanning = new ObservableCollection<Course>();
                    else if(cur.Dayofweek == 4)
                        FridayPlanning = new ObservableCollection<Course>();
                    else if(cur.Dayofweek == 5)
                        SaturdayPlanning = new ObservableCollection<Course>();
                }
            }
      


        }
        private void NextWeekAction()
        {      
            DatesBeginPlanning = DatesBeginPlanning.AddDays(7);
            DisplayPlanning(DatesBeginPlanning);
        }
        private void PreviousWeekAction()
        {
            DatesBeginPlanning = DatesBeginPlanning.AddDays(-7);
            DisplayPlanning(DatesBeginPlanning);
        }
        


    }


}
