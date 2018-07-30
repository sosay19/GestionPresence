using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;

namespace prbd_1718_presences_g27
{
    public partial class CoursesView : UserControlBase
    {
        private ObservableCollection<Course> courses;
        private ObservableCollection<User> users;
        public ObservableCollection<Course> Courses
        {
            get
            {
                return courses;
            }
            set
            {
                courses = value;
                RaisePropertyChanged(nameof(Courses));
            }
        }
        public ObservableCollection<User> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                RaisePropertyChanged(nameof(Users));
            }
        }
        private string filter;
        private string filterUsers;
        private bool? filterRegisters;
        private DateTime? filterDatesBegin;
        private DateTime? filterDatesEnd;
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                ApplyFilterAction();
                RaisePropertyChanged(nameof(Filter));
            }
        }
        public string FilterUsers
        {
            get { return filterUsers; }
            set
            {
                filterUsers = value;
                ApplyFilterAction();
                RaisePropertyChanged(nameof(FilterUsers));
            }
        }
        public bool? FilterRegisters
        {
            get
            {
                return filterRegisters;
            }
            set
            {
                filterRegisters = value;
                ApplyFilterAction();
                RaisePropertyChanged(nameof(FilterRegisters));
            }
        }
        public DateTime? FilterDatesBegin
        {
            get { return filterDatesBegin; }
            set
            {
                filterDatesBegin = value;
                ApplyFilterAction();
                RaisePropertyChanged(nameof(FilterDatesBegin));
            }
        }
        public DateTime? FilterDatesEnd
        {
            get { return filterDatesEnd; }
            set
            {
                filterDatesEnd = value;
                ApplyFilterAction();
                RaisePropertyChanged(nameof(FilterDatesEnd));
            }
        }
        private bool allSelected;
        public bool AllSelected
        {
            get { return allSelected; }
            set
            {
                allSelected = value;
                ApplyFilterAction();
            }
        }
        public ICommand ClearFilter { get; set; }
        public ICommand NewCourse { get; set; }
        public ICommand DisplayCourseDetails { get; set; }
        public CoursesView()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            DataContext = this;
            Courses = new ObservableCollection<Course>(App.Model.course);
            App.Messenger.Register<Course>(App.MSG_COURSE_CHANGED, course => { ApplyFilterAction(); });
            ClearFilter = new RelayCommand(() => {
                Filter = "";
                FilterUsers = "";
                FilterRegisters = null;
                FilterDatesBegin = null;
                FilterDatesEnd = null;
            });
            App.Messenger.Register<string>(App.MSG_SAVE_ACTION, (s) =>
            {
                Courses = new ObservableCollection<Course>(App.Model.course);

            });
            NewCourse = new RelayCommand(() => { App.Messenger.NotifyColleagues(App.MSG_NEW_COURSE); });

            DisplayCourseDetails = new RelayCommand<Course>(m => {
                App.Messenger.NotifyColleagues(App.MSG_DISPLAY_COURSE, m); });

            IList<ProfessorEntry> list = new List<ProfessorEntry>();
            if (App.CurrentUser.Role == "admin")
            {
                foreach (var p in App.Model.user)
                {
                    if (p.Role != "admin")
                    {
                        list.Add(new ProfessorEntry(p.Fullname));
                    }
                }


            }
            else
            {

                list.Add(new ProfessorEntry(App.CurrentUser.Fullname));
                FilterUsers = App.CurrentUser.Fullname;
                cmb.IsEnabled = false;
                cmb.SelectedIndex = 0;
                btnNew.IsEnabled = false;

            }
            _professorEntries = new CollectionView(list);
        }
        private readonly CollectionView _professorEntries;
        private string _professorEntry;
        public CollectionView ProfessorEntries
        {
            get { return _professorEntries; }
        }
        public string ProfessorEntry
        {
            get { return _professorEntry; }
            set
            {
                if (_professorEntry == value) return;
                _professorEntry = value;
                OnPropertyChanged("ProfessorEntry");
            }
        }    
        private void ApplyFilterAction()
        {
            if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter)
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*1*/

            else if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.User.Fullname.Equals(FilterUsers)
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Student.Count > 0 && m.User.Fullname.Equals(FilterUsers)
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Student.Count == 0 && m.User.Fullname.Equals(FilterUsers)
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*2*/

            else if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
               && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Finishdate >= FilterDatesEnd
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Student.Count > 0 && m.Finishdate >= FilterDatesEnd
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Student.Count == 0 && m.Finishdate >= FilterDatesEnd
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*1*/

            else if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Finishdate >= FilterDatesEnd && m.User.Fullname.Equals(FilterUsers)
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Finishdate >= FilterDatesEnd && m.User.Fullname.Equals(FilterUsers) && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Finishdate >= FilterDatesEnd && m.User.Fullname.Equals(FilterUsers) && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*2*/

            /*****************/
            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Startdate >= FilterDatesBegin
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Startdate >= FilterDatesBegin && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Startdate >= FilterDatesBegin && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*1*/

            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.User.Fullname.Equals(FilterUsers) && m.Startdate >= FilterDatesBegin
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.User.Fullname.Equals(FilterUsers) && m.Student.Count > 0 && m.Startdate >= FilterDatesBegin
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.User.Fullname.Equals(FilterUsers) && m.Student.Count == 0 && m.Startdate >= FilterDatesBegin
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*2*/

            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
               && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Startdate >= FilterDatesBegin && m.Finishdate >= FilterDatesEnd
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Startdate >= FilterDatesBegin && m.Finishdate >= FilterDatesEnd && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Startdate >= FilterDatesBegin && m.Finishdate >= FilterDatesEnd && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*1*/

            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Startdate >= FilterDatesBegin && m.Finishdate >= FilterDatesEnd && m.User.Fullname.Equals(FilterUsers)
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Startdate >= FilterDatesBegin && m.Finishdate >= FilterDatesEnd
                               && m.User.Fullname.Equals(FilterUsers) && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Title.Contains(Filter) && m.Startdate >= FilterDatesBegin && m.Finishdate >= FilterDatesEnd
                               && m.User.Fullname.Equals(FilterUsers) && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*2*/
            /*********************************ff***/
            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*1*/

            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.User.Fullname.Equals(FilterUsers)
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.User.Fullname.Equals(FilterUsers) && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.User.Fullname.Equals(FilterUsers) && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*2*/

            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
               && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Finishdate >= FilterDatesEnd
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Finishdate >= FilterDatesEnd && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Finishdate >= FilterDatesEnd && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*1*/

            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Finishdate >= FilterDatesEnd && m.User.Fullname.Equals(FilterUsers)
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Finishdate >= FilterDatesEnd && m.User.Fullname.Equals(FilterUsers) && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Finishdate >= FilterDatesEnd && m.User.Fullname.Equals(FilterUsers) && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*2*/

            /*****************/
            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*1*/

            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin && m.User.Fullname.Equals(FilterUsers)
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin && m.User.Fullname.Equals(FilterUsers) && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin && m.User.Fullname.Equals(FilterUsers) && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*2*/

            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
               && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin && m.Finishdate >= FilterDatesEnd
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin && m.Finishdate >= FilterDatesEnd && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            /*1*/

            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == null)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin && m.Finishdate >= FilterDatesEnd && m.User.Fullname.Equals(FilterUsers)
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == true)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin && m.Finishdate >= FilterDatesEnd && m.User.Fullname.Equals(FilterUsers) && m.Student.Count > 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else if (string.IsNullOrEmpty(Filter) && !string.IsNullOrEmpty(FilterDatesBegin.ToString())
                && !string.IsNullOrEmpty(FilterDatesEnd.ToString()) && !string.IsNullOrEmpty(FilterUsers) && FilterRegisters == false)
            {
                var filtered = from m in App.Model.course
                               where m.Startdate >= FilterDatesBegin && m.Finishdate >= FilterDatesEnd && m.User.Fullname.Equals(FilterUsers) && m.Student.Count == 0
                               select m;
                Courses = new ObservableCollection<Course>(filtered);
            }
            else
            {
                Courses = new ObservableCollection<Course>(App.Model.course);
            }

        }
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class IntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case 0:
                    return "lundi";
                case 1:
                    return "mardi";
                case 2:
                    return "mercredi";
                case 3:
                    return "jeudi";
                case 4:
                    return "vendredi";
                case 5:
                    return "samedi";
                default:
                    return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case "lundi":
                    return 0;
                case "mardi":
                    return 1;
                case "mercredi":
                    return 2;
                case "jeudi":
                    return 3;
                case "vendredi":
                    return 4;
                case "samedi":
                    return 5;

                default:
                    return Binding.DoNothing;
            }
        }
    }
    public class PercentageIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {

            var presence = 0.0;
            var nbroccurence = 0.0;
            foreach (var u in App.Model.courseoccurrence)
            {
                if (u.Course.Code == (int)value)
                {
                    ++nbroccurence;

                    if (u.Presence.Count > 0)
                    {
                        ++presence;
                    }
                }

            }



            return presence / nbroccurence + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {

            return Binding.DoNothing;

        }
    }
}
