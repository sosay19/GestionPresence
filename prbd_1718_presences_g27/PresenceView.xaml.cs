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
using System.Globalization;

namespace prbd_1718_presences_g27
{

    public partial class PresenceView : UserControlBase
    {
        public static readonly RoutedCommand CommandPresent = new RoutedCommand();
        public static readonly RoutedCommand CommandAbsent = new RoutedCommand();

        private ObservableCollection<Student> presenceStudents;
        public ObservableCollection<Student> PresenceStudents
        {
            get { return presenceStudents; }
            set
            {
                presenceStudents = value;
                RaisePropertyChanged(nameof(PresenceStudents));
            }
        }
        private Course course;

        public Course Course
        {
            get
            {
                return course;
            }
            set
            {
                course = value;
                RaisePropertyChanged(nameof(Course));
            }
        }
        private List<PersonalInfo> listPersonalInfo = new List<PersonalInfo>();
        public List<PersonalInfo> ListPersonalInfo
        {
            get { return listPersonalInfo; }
            set
            {
                listPersonalInfo = value;
                RaisePropertyChanged(nameof(ListPersonalInfo));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand MyCommand { get; set; }
        private int courseOcc;
        public PresenceView(int idCourseoccurence)
        {
            InitializeComponent();

            DataContext = this;

            var q = from c in App.Model.courseoccurrence
                    where c.Id == idCourseoccurence
                    select c.Course;
            foreach (var res in q)
            {
                course = res;
            }

            PresenceStudents = new ObservableCollection<Student>(course.Student);
            Course = course;
            courseOcc = idCourseoccurence;
            getPresences();
            dataGridPresence.ItemsSource = ListPersonalInfo;
        }
        private void getPresences()
        {
            foreach (var stu in Course.Student)
            {
                int? res = null;
                foreach (var s in App.Model.presence)
                {
                    if (stu.Id == s.Student && s.Courseoccurrence.Id == courseOcc)
                    {
                        res = s.Present;

                    }

                }
                if (res == 1)
                {
                    ListPersonalInfo.Add(new PersonalInfo { Gender = Genders.Male, Lastname = stu.Lastname, Firstname = stu.Firstname, Id = stu.Id });
                }
                else if (res == 0)
                {
                    ListPersonalInfo.Add(new PersonalInfo { Gender = Genders.Female, Lastname = stu.Lastname, Firstname = stu.Firstname, Id = stu.Id });
                }
                else
                {
              
                    ListPersonalInfo.Add(new PersonalInfo { Gender = Genders.ND, Lastname = stu.Lastname, Firstname = stu.Firstname, Id = stu.Id });
                }

            }

        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            if (e.Command == CommandPresent)
            {
                foreach (var stu in Course.Student)
                {

                    if (stu.Id == (int)e.Parameter) // au bon étudiant
                    {
                        Presence presence = null;
                        
                        foreach (var pres in App.Model.presence)
                        {
                            if (stu.Id == pres.Student && pres.Courseoccurrence.Id == courseOcc)
                            {
                                presence = pres;
                            }

                        }
                        if (presence == null)// n'est pas dans la base de donnée
                        {
                            Presence pres = null;
                            foreach (var s in stu.Presence)
                            {
                                if (s.Courseoccurence ==courseOcc && s.Student==stu.Id)
                                {
                                    pres = s;                                                                 
                                }                               
                            }
                            if(pres ==null)
                            {
                                pres = new Presence();
                                pres.Student = stu.Id;
                                pres.Present = 1;
                                pres.Courseoccurence = courseOcc;
                                stu.Presence.Add(pres);
                            }
                            else
                            {

                                pres.Student = stu.Id;
                                pres.Present = 1;
                                pres.Courseoccurence = courseOcc;
                            }
                            

                        }
                        else
                        {
                            presence.Student = stu.Id;
                            presence.Present = 1;
                            presence.Courseoccurence = courseOcc;
                        }
                    }
                }
                
            }
            else if (e.Command == CommandAbsent)
            {
                foreach (var stu in Course.Student)
                {

                    if (stu.Id == (int)e.Parameter) // au bon étudiant
                    {
                        Presence presence = null;

                        foreach (var pres in App.Model.presence)
                        {
                            if (stu.Id == pres.Student && pres.Courseoccurrence.Id == courseOcc)
                            {
                                presence = pres;
                            }

                        }
                        if (presence == null)// n'est pas dans la base de donnée
                        {
                            Presence pres = null;
                            foreach (var s in stu.Presence)
                            {
                                if (s.Courseoccurence == courseOcc && s.Student == stu.Id)
                                {
                                    pres = s;
                                }

                            }
                            if (pres == null)
                            {
                                pres = new Presence();
                                pres.Student = stu.Id;
                                pres.Present = 0;
                                pres.Courseoccurence = courseOcc;
                                stu.Presence.Add(pres);
                            }
                            else
                            {

                                pres.Student = stu.Id;
                                pres.Present = 0;
                                pres.Courseoccurence = courseOcc;
                            }


                        }
                        else
                        {
                            presence.Student = stu.Id;
                            presence.Present = 0;
                            presence.Courseoccurence = courseOcc;
                        }
                    }
                }


            }
            //App.CurrentCourse = Course;
        }
        public class PersonalInfo : UserControlBase
        {
            public string firstname;
            public string Firstname
            {
                get { return firstname; }
                set
                {
                    firstname = value;

                    RaisePropertyChanged(nameof(Firstname));

                }
            }

            public string lastname;
            public string Lastname
            {
                get { return lastname; }
                set
                {
                    lastname = value;

                    RaisePropertyChanged(nameof(Lastname));

                }
            }
            public int id;
            public int Id
            {
                get { return id; }
                set
                {
                    id = value;

                    RaisePropertyChanged(nameof(Id));

                }
            }
            public Genders gender;
            public Genders Gender
            {
                get
                {

                    return gender;
                }
                set
                {
                    gender = value;

                    RaisePropertyChanged(nameof(Gender));

                }
            }
            private List<PersonalInfo> listPersonalInfo;
            public List<PersonalInfo> ListPersonalInfo
            {
                get { return listPersonalInfo; }
                set
                {
                    listPersonalInfo = value;
                    RaisePropertyChanged(nameof(ListPersonalInfo));

                }
            }
        }

        public enum Genders
        {
            Male = 1,
            Female = 0,
            ND = 2
        }

    }


    public class EnumBooleanConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var ParameterString = parameter as string;
            if (ParameterString == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object paramvalue = Enum.Parse(value.GetType(), ParameterString);

            return paramvalue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ParameterString = parameter as string;
            if (ParameterString == null)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, ParameterString);
        }


    }



}
