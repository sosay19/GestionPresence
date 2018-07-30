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
using System.Collections;

namespace prbd_1718_presences_g27
{

    public partial class CourseDetailView : UserControlBase
    {

        private List<DateTime> listDate = new List<DateTime>();
        public Course Course { get; set; }
        private ObservableCollection<User> users;
        private ObservableCollection<Student> registeredStudents;
        private ObservableCollection<Student> unregisteredStudents;
        public ICommand RegisterAll { get; set; }
        public ICommand Register { get; set; }
        public ICommand UnregisterAll { get; set; }
        public ICommand Unregister { get; set; }
        public ICommand Delete { get; set; }
        public ICommand DisplayPresence { get; set; }
        public ICommand GetPlanning { get; set; }
        private List<Student> studentListRegister = new List<Student>();
        private List<Student> studentListRegisterLi = new List<Student>();
        private List<Student> studentListUnregister = new List<Student>();
        private List<Student> studentListUnregisterLi = new List<Student>();
        private bool isNew;
        //private bool isDatesBegin;
        private bool isDaysBegin;
        private bool isUsersBegin;
        //private bool isDatesEnd;
        private bool isStarttime;
        private bool isEndtime;
        private DateTime dbegin = DateTime.Today;
        private DateTime dend = DateTime.Today.AddDays(1);
        private TimeSpan tend = new TimeSpan(18, 00, 00);
        private TimeSpan tstart = new TimeSpan(8, 00, 00);
        private void DeleteAction()
        {
            foreach (var osu in Course.Courseoccurrence.ToList())
            {
                App.Model.courseoccurrence.Remove(osu);
            }


            App.Model.course.Remove(Course);
            App.Model.SaveChanges();
            App.Messenger.NotifyColleagues(App.MSG_COURSE_CHANGED, Course);
            App.Messenger.NotifyColleagues(App.MSG_CLOSE_TAB, Parent);
        }
        private DateTime datesBegin;
        public DateTime DatesBegin
        {
            get
            {

                return datesBegin;
            }
            set
            {

                datesBegin = (DateTime)value;
                ValidateDatesBegin();
                RaisePropertyChanged(nameof(DatesBegin));
            }
        }
        public DateTime datesEnd;
        public DateTime DatesEnd
        {
            get
            {

                return datesEnd;

            }
            set
            {

                datesEnd = (DateTime)value;
                ValidateDatesEnd();
                RaisePropertyChanged(nameof(DatesEnd));
            }
        }
        public TimeSpan starttime;
        public TimeSpan Starttime
        {
            get { return starttime; }
            set
            {
                starttime = (TimeSpan)value;
                ValidateStarttime();
                RaisePropertyChanged(nameof(Starttime));
            }
        }
        public TimeSpan endtime;
        public TimeSpan Endtime
        {
            get { return endtime; }
            set
            {
                endtime = (TimeSpan)value;
                ValidateEndtime();
                RaisePropertyChanged(nameof(Endtime));
            }
        }
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));
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
        public ObservableCollection<Student> RegisteredStudents
        {
            get
            {
                return registeredStudents;
            }
            set
            {
                registeredStudents = value;
                RaisePropertyChanged(nameof(RegisteredStudents));
            }
        }
        public ObservableCollection<Student> UnregisteredStudents
        {
            get
            {
                return unregisteredStudents;
            }
            set
            {
                unregisteredStudents = value;
                RaisePropertyChanged(nameof(UnregisteredStudents));
            }
        }
        public List<DayEntry> list = new List<DayEntry>();
        public List<String> listDay = new List<String>();
        public List<String> listDayEn = new List<String>();
        private string filterUsers;
        public string FilterUsers
        {
            get { return filterUsers; }
            set
            {
                filterUsers = value;
                ValidateFilterUsers();
                RaisePropertyChanged(nameof(FilterUsers));
            }
        }
        private string filterDays;
        public string FilterDays
        {
            get { return filterDays; }
            set
            {
                filterDays = value;
                ValidateFilterDays();
                RaisePropertyChanged(nameof(FilterDays));
            }
        }
        public bool IsExisting { get { return !IsNew; } }
        public IQueryable<int> xcode = from m in App.Model.course select m.Code;
        public int fcode;
        public IQueryable<int> XCode
        {
            get { return xcode; }
            set
            {
                xcode = value;

                RaisePropertyChanged(nameof(XCode));
            }
        }
        public int code;
        public int Code
        {
            get { return code; }
            set
            {
                code = value;
                ValidateCode();
                RaisePropertyChanged(nameof(Code));
                App.Messenger.NotifyColleagues(App.MSG_CODE_CHANGED, "Course " + value.ToString());

            }
        }
        private DataView location;
        public DataView Location
        {
            get { return location; }
            set
            {
                location = value;
                RaisePropertyChanged(nameof(Location));
            }
        }
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {

                title = value;
                ValidateTitle();
                RaisePropertyChanged(nameof(Title));
            }
        }
        private void ValidateFilterDays()
        {
            ClearErrors();
            if (string.IsNullOrEmpty(FilterDays))
            {
                AddError("FilterDays", Properties.Resources.Error_Required);
            }
            else
            {
                if (isDaysBegin)
                {


                    MessageBoxResult result = MessageBox.Show("Voulez vous changer le prof du cours ?", "Confirmation", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Course.Dayofweek = listDay.IndexOf(FilterDays);
                            Location = toLocation(Course).DefaultView;
                            App.CurrentCourse = Course;
                            break;
                        case MessageBoxResult.No:
                            FilterDays = "";
                            break;

                    }
                }
                else
                {
                    Course.Dayofweek = listDay.FindIndex(x => x.StartsWith(FilterDays));
                    App.CurrentCourse = Course;
                    isDaysBegin = true;
                }

            }
            RaiseErrors();

        }
        private void ValidateFilterUsers()
        {
            ClearErrors();
            if (string.IsNullOrEmpty(FilterUsers))
            {
                AddError("FilterUsers", Properties.Resources.Error_Title_Required);
            }
            else
            {

                if (isUsersBegin)
                {
                    MessageBoxResult result = MessageBox.Show("Voulez vous changer le prof du cours ?", "Confirmation", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:

                            var q = from u in App.Model.user
                                    where u.Fullname == FilterUsers
                                    select u;
                            foreach (var qq in q)
                            {
                                Course.User = qq;
                            }

                            App.CurrentCourse = Course;
                            break;
                        case MessageBoxResult.No:
                            FilterUsers = "";
                            break;

                    }
                }
                else
                {

                    var q = from u in App.Model.user
                            where u.Fullname == FilterUsers
                            select u;
                    foreach (var qq in q)
                    {
                        Course.User = qq;
                    }

                    App.CurrentCourse = Course;
                    isUsersBegin = true;
                }



            }
            RaiseErrors();
        }
        private void ValidateTitle()
        {
            ClearErrors();
            if (string.IsNullOrEmpty(Title))
            {
                AddError("Title", Properties.Resources.Error_Title_Required);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Voulez vous changer le titre du cours ?", "Confirmation", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Course.Title = title;
                        App.CurrentCourse = Course;
                        break;
                    case MessageBoxResult.No:
                        title = Course.Title;
                        break;

                }

            }
            RaiseErrors();
        }
        private void ValidateDatesBegin()
        {
            ClearErrors();
            if (DatesBegin != Course.Startdate)
                if (DatesBegin >= DatesEnd)
                {
                    AddError("DatesBegin", Properties.Resources.Error_DatesBeginMustBeLower);
                }
                else
                {
                  
                        MessageBoxResult result = MessageBox.Show("Voulez vous changer la date de la début du cours ?", "Confirmation", MessageBoxButton.YesNo);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:

                                Course.Startdate = datesBegin;
                                Location = toLocation(Course).DefaultView;
                                App.CurrentCourse = Course;

                                break;
                            case MessageBoxResult.No:
                                datesBegin = Course.Startdate;

                                break;

                        }
                   
                }
            RaiseErrors();
        }
        private void ValidateDatesEnd()
        {
            ClearErrors();
            if (DatesEnd != Course.Finishdate)
                if (DatesBegin >= DatesEnd)
                {
                    AddError("DatesEnd", Properties.Resources.Error_DatesEndMustBeHigher);
                }
                else
                {
                   
                        MessageBoxResult result = MessageBox.Show("Voulez vous changer la date de la fin du cours ?", "Confirmation", MessageBoxButton.YesNo);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                Course.Finishdate = datesEnd;
                                Location = toLocation(Course).DefaultView;
                                App.CurrentCourse = Course;
                                break;
                            case MessageBoxResult.No:
                                datesEnd = Course.Finishdate;

                                break;

                        }

                }
            RaiseErrors();
        }
        private void ValidateStarttime()
        {
            ClearErrors();
            if ((Starttime) >= (Endtime))
            {
                AddError("Starttime", Properties.Resources.Error_StartTimeMustBeLower);
            }
            else
            {
                if (!isStarttime)
                {
                    MessageBoxResult result = MessageBox.Show("Voulez vous changer l'heure du début du cours ?", "Confirmation", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Course.Starttime = starttime;
                            App.CurrentCourse = Course;
                            break;
                        case MessageBoxResult.No:
                            starttime = Course.Starttime;
                            break;

                    }
                }
                else
                {
                    Course.Starttime = tstart;

                    App.CurrentCourse = Course;
                    isStarttime = false;

                }



            }
            RaiseErrors();
        }
        private void ValidateEndtime()
        {
            ClearErrors();
            if ((Starttime) >= (Endtime))
            {
                AddError("Endtime", Properties.Resources.Error_EndTimeMustBeHigher);
            }
            else
            {
                if (!isEndtime)
                {
                    MessageBoxResult result = MessageBox.Show("Voulez vous changer l'heure de la fin du cours ?", "Confirmation", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Course.Endtime = endtime;
                            break;
                        case MessageBoxResult.No:
                            endtime = Course.Endtime;
                            break;

                    }
                }
                else
                {
                    Course.Endtime = tend;

                    App.CurrentCourse = Course;
                    isEndtime = false;

                }



            }
            RaiseErrors();
        }
        private void ValidateCode()
        {
            ClearErrors();
            int ce = Code;
            if ((ce == null))
            {
                AddError("Code", Properties.Resources.Error_Required);
            }
            else if ((XCode.Contains(ce)))
            {
                AddError("Code", Properties.Resources.Error_CodeAlreadyExist);
            }


            else
            {

                if (fcode != Code)
                {

                    MessageBoxResult result = MessageBox.Show("Voulez vous changer le code du cours ?", "Confirmation", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Course.Code = (int)code;

                            App.CurrentCourse = Course;
                            break;
                        case MessageBoxResult.No:
                            break;

                    }
                }


            }
            RaiseErrors();
        }
        private void RegisterAllAction()
        {
            MessageBoxResult result = MessageBox.Show("Voulez vous inscrire tous les étudiants non-inscrits ?", "Confirmation", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    foreach (var stu in unregisteredStudents)
                    {
                        Course.Student.Add(stu);
                    }
                    Location = toLocation(Course).DefaultView;
                    studentListRegister.AddRange(studentListUnregister);
                    studentListUnregister.Clear();
                    RegisteredStudents = new ObservableCollection<Student>(studentListRegister);
                    UnregisteredStudents = new ObservableCollection<Student>(studentListUnregister);
                    break;
                case MessageBoxResult.No:
                    break;

            }

        }
        private void RegisterAction()
        {
            if (dgUnregisteredStudents.SelectedItems.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Voulez vous inscrire ces étudiants non-inscrits ?", "Confirmation", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        for (int i = 0; i < dgUnregisteredStudents.SelectedItems.Count; i++)
                        {
                            Course.Student.Add((Student)dgUnregisteredStudents.SelectedItems[i]);
                            studentListRegister.Add((Student)dgUnregisteredStudents.SelectedItems[i]);
                            studentListUnregister.Remove((Student)dgUnregisteredStudents.SelectedItems[i]);
                        }
                        Location = toLocation(Course).DefaultView;
                        RegisteredStudents = new ObservableCollection<Student>(studentListRegister);
                        UnregisteredStudents = new ObservableCollection<Student>(studentListUnregister);
                        break;
                    case MessageBoxResult.No:
                        break;

                }
            }

        }
        private void UnregisterAllAction()
        {
            MessageBoxResult result = MessageBox.Show("Voulez vous inscrire tous les étudiants non-inscrits " +
                "Attention, ce changement aura un impact sur" +
                "les occurences du cours et sur les éventuelles présences déjà " +
                "encodées." +
                "Confirmez-vous le changement ?", "Confirmation", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Course.Student.Clear();
                    studentListUnregister.AddRange(studentListRegister);
                    studentListRegister.Clear();
                    Location = toLocation(Course).DefaultView;
                    RegisteredStudents = new ObservableCollection<Student>(studentListRegister);
                    UnregisteredStudents = new ObservableCollection<Student>(studentListUnregister);
                    break;
                case MessageBoxResult.No:
                    break;

            }

        }
        private void UnregisterAction()
        {
            if (dgRegisteredStudents.SelectedItems.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Voulez vous inscrire tous les étudiants non-inscrits " +
                "Attention, ce changement aura un impact sur" +
                "les occurences du cours et sur les éventuelles présences déjà " +
                "encodées. " +
                "Confirmez-vous le changement ?", "Confirmation", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        for (int i = 0; i < dgRegisteredStudents.SelectedItems.Count; i++)
                        {
                            Course.Student.Remove((Student)dgRegisteredStudents.SelectedItems[i]);
                            studentListUnregister.Add((Student)dgRegisteredStudents.SelectedItems[i]);
                            studentListRegister.Remove((Student)dgRegisteredStudents.SelectedItems[i]);
                        }
                        Location = toLocation(Course).DefaultView;
                        RegisteredStudents = new ObservableCollection<Student>(studentListRegister);
                        UnregisteredStudents = new ObservableCollection<Student>(studentListUnregister);
                        break;
                    case MessageBoxResult.No:


                        break;

                }
            }

        }
        public CourseDetailView(Course course, bool isNew)
        {
            InitializeComponent();

            DataContext = this;
            listDay.Add("lundi"); listDay.Add("mardi"); listDay.Add("mercredi"); listDay.Add("jeudi"); listDay.Add("vendredi"); listDay.Add("samedi");
            listDayEn.Add("Monday"); listDayEn.Add("Tuesday"); listDayEn.Add("Wednesday"); listDayEn.Add("Thursday"); listDayEn.Add("Friday"); listDayEn.Add("Saturday"); listDayEn.Add("Sunday");
            Course = course;
            IsNew = isNew;
            Course.Startdate = dbegin;
            Course.Finishdate = dend;
            location = toLocation(Course).DefaultView;
            ICollection<Student> filteredUser = Course.Student;
            foreach (var fuser in filteredUser)
            {
                studentListRegister.Add(fuser);
                studentListRegisterLi.Add(fuser);

            }
            var badCodes = new List<int>();
            foreach (var qq in Course.Student)
            {
                badCodes.Add(qq.Id);
            }
            IQueryable<Student> filteredUser2 =

                           (
                            from m in App.Model.student
                            where !badCodes.Contains(m.Id)
                            select m).Distinct();
            foreach (var fuser2 in filteredUser2)
            {
                studentListUnregister.Add(fuser2);
                studentListUnregisterLi.Add(fuser2);
            }
            RegisteredStudents = new ObservableCollection<Student>(studentListRegister);
            UnregisteredStudents = new ObservableCollection<Student>(studentListUnregister);
            string jour = listDay[Course.Dayofweek];
            list.Add(new DayEntry(jour));
            foreach (var day in listDay)
            {

                if (day != jour)
                {
                    list.Add(new DayEntry(day));
                }
            }
            _dayEntries = new CollectionView(list);
            IList<ProfessorEntry> listUsers = new List<ProfessorEntry>();
            if (App.CurrentUser.Role == "admin")
            {
                foreach (var p in App.Model.user)
                {
                    if (p.Role != "admin")
                    {
                        listUsers.Add(new ProfessorEntry(p.Fullname));
                    }
                }


            }
            else
            {

                listUsers.Add(new ProfessorEntry(App.CurrentUser.Fullname));

            }
            _professorEntries = new CollectionView(listUsers);




            title = Course.Title;
            starttime = Course.Starttime;
            endtime = Course.Endtime;
            datesBegin = Course.Startdate;
            datesEnd = Course.Finishdate;
            code = Course.Code;
            txtCode.IsEnabled = false;
            cmb1.SelectedIndex = 0;
            if (isNew)
            {
                fcode = Course.Code;
            
              
                isUsersBegin = true;
                isDaysBegin = true;
                isEndtime = true;
                isStarttime = true;
                DatesEnd = dend;
                DatesBegin = dbegin;
                Endtime = tend;
                Starttime = tstart;
                txtCode.IsEnabled = true;
                App.ListNewCourse.Add(Course);
            }
            else
            {
                FilterUsers = Course.User.Fullname;
                FilterDays = listDay[Course.Dayofweek];
            }

            App.Messenger.Register<string>(App.MSG_DISPLAY_PRESENCE_UPDATE, (s) =>
            {
                //Course = App.CurrentCourse;
                Location = toLocation(Course).DefaultView;

            });
            Delete = new RelayCommand(DeleteAction, () => { return IsExisting; });
            if (App.CurrentUser.Role == "teacher")
            {
                txtCode.IsEnabled = false;
                txtTitle.IsEnabled = false;
                cmb.IsEnabled = false;
                cmb1.IsEnabled = false;
                datePicker1.IsEnabled = false;
                datePicker2.IsEnabled = false;
                StartTime.IsEnabled = false;
                EndTime.IsEnabled = false;
                btnDelete.IsEnabled = false;

            }
            RegisterAll = new RelayCommand(RegisterAllAction);
            Register = new RelayCommand(RegisterAction);
            UnregisterAll = new RelayCommand(UnregisterAllAction);
            Unregister = new RelayCommand(UnregisterAction);
           
            DisplayPresence = new RelayCommand<string>(m => {

                
                if (App.CurrentUser.Role == "teacher")
                {
                    int idOccurence = 0;
                    foreach (var val in Course.Courseoccurrence)
                    {
                        Console.WriteLine(val.Date.ToString("dd-MM-yyyy"));
                        if (val.Course.Code == Course.Code && val.Date.ToString("dd-MM-yyyy") == m)
                            idOccurence = val.Id;
                    }
                    Console.WriteLine(idOccurence);
                    App.Messenger.NotifyColleagues(App.MSG_DISPLAY_PRESENCE, idOccurence);
                }
                


            });


        }
        private List<Presence> listPresences = new List<Presence>();
        private DataTable toLocation(Course c)
        {
          
            var table = new DataTable();
            var columns = new Dictionary<int, DateTime>();
            table = new DataTable();
            table.Columns.Add("Etudiant");
            var totald = (c.Finishdate - c.Startdate).TotalDays;
            
            var dayName = c.Startdate.ToString("dddd"); 
            var intDayName = listDayEn.IndexOf(dayName);
            var str = listDay[c.Dayofweek];
            int tmpFr = listDay.IndexOf(str);
            int tmp = tmpFr- intDayName; /* 0 5*/
            if(tmp<0)
            {
                tmp += 7;
            }
            int i = 1;

            //var maxId = 0;
            //foreach (var maxIdx in App.Model.courseoccurrence)
            //{
            //    if (maxIdx.Id >= maxId)
            //    {
            //        maxId = maxIdx.Id;
            //    }

            //}

            while (totald >= tmp)
            {
                table.Columns.Add(c.Startdate.AddDays(tmp).ToString("dd-MM-yyyy"));
                columns[i] = c.Startdate.AddDays(tmp);
                if (!listDate.Contains(c.Startdate.AddDays(tmp)))
                {
                    

                    listDate.Add((c.Startdate.AddDays(tmp)));
                  
                    Courseoccurrence co = new Courseoccurrence();
                    co.Date = c.Startdate.AddDays(tmp);
                    co.Course = c;
                    //co.Id = maxId;
                    //++maxId;
                    c.Courseoccurrence.Add(co);
                }
                    
                


                tmp += 7;
                ++i;

            }
            
          
            foreach (var lect in c.Student)
            {

                var row = table.NewRow();
                row[0] = lect.Firstname + "," + lect.Lastname;
                for (int j = 1; j < table.Columns.Count; ++j)
                {
                    DateTime idL = columns[j];

                    string txt = "?";
            
                    foreach (var x in lect.Presence)
                    {
                        
                        foreach (var xx in c.Courseoccurrence)
                        {
                            if (x.Courseoccurence == xx.Id)
                            {
                       
                                if (idL == xx.Date)
                                {
                                    
                                    if (x.Present == 0)
                                    {
                                        txt = "A";
                                    }
                                    else if (x.Present == 1)
                                    {
                                        txt = "P";
                                    }

                                }
                            }

                        }
          

                    }

                    row[j] = txt;
                }
                table.Rows.Add(row);
            }
            Course = c;
            
            return table;

        }
        private readonly CollectionView _professorEntries;
        private readonly CollectionView _dayEntries;
        private string _professorEntry;
        private string _dayEntry;
        public CollectionView ProfessorEntries
        {
            get { return _professorEntries; }
        }
        public CollectionView DayEntries
        {
            get { return _dayEntries; }
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
        public string DayEntry
        {
            get { return _dayEntry; }
            set
            {
                if (_dayEntry == value) return;
                _dayEntry = value;
                OnPropertyChanged("DayEntry");
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;       
    }
    public class ProfessorEntry
    {
        public string Name { get; set; }

        public ProfessorEntry(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
    public class DayEntry
    {
        public string Name { get; set; }

        public DayEntry(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
    public class ValueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            string input;
            try
            {
                DataGridCell dgc = (DataGridCell)value;
                System.Data.DataRowView rowView = (System.Data.DataRowView)dgc.DataContext;
                input = (string)rowView.Row.ItemArray[dgc.Column.DisplayIndex];
            }
            catch (InvalidCastException e)
            {
                return DependencyProperty.UnsetValue;
            }
            switch (input)
            {
                case "A": return Brushes.Red;
                case "P": return Brushes.Green;
                case "?": return Brushes.White;
                default: return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }


}
