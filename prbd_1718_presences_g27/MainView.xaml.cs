using PRBD_Framework;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Input;
using System.Reflection;
using System;
using System.Data.Entity;
using System.Windows;

namespace prbd_1718_presences_g27
{
    public partial class MainView : WindowBase
    {       
        private bool isNew;
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));
            }
        }
        public ICommand SaveChanges { get; set; }
        public ICommand CancelChanges { get; set; }
        public ICommand Logout { get; set; }
        public ICommand AddStudent { get; set; }
        /*jusqu'à là tout va bien*/

        public MainView()
        {
            InitializeComponent();

            DataContext = this;
            App.Messenger.Register(App.MSG_NEW_COURSE,
                () =>
                {
                  
                    var course = App.Model.course.Create();
                    App.CurrentCourse = newTabForCourse(course, true);
                    IsNew = true;


                });

            App.Messenger.Register<string>(App.MSG_CODE_CHANGED, (s) =>
            {
               
                  (tabControl.SelectedItem as TabItem).Header = s;
            });
            App.Messenger.Register<Course>(App.MSG_DISPLAY_COURSE, course =>
            {
                if (course != null)
                {
                    var tab = (from TabItem t in tabControl.Items where (string)t.Header == course.Title select t).FirstOrDefault();
                    if (tab == null)
                    {
        
                        App.CurrentCourse = newTabForCourse(course, false);
                        IsNew = false;


                    }
                        
                    else
                        //Dispatcher.InvokeAsync(() => tab.Focus());
                    tab.Focus();
                }
            });       
            App.Messenger.Register<int>(App.MSG_DISPLAY_PRESENCE, idOccurence =>
            {
                showPresence(idOccurence);             
                
            });

            App.Messenger.Register<TabItem>(App.MSG_CLOSE_TAB, tab =>
            {
                tabControl.Items.Remove(tab);
            }); 
            CancelChanges =new RelayCommand<string>((name) =>
                 {
                     App.Messenger.NotifyColleagues(App.MSG_CANCEL_ACTION, "<new Course>");
                     App.Messenger.NotifyColleagues(App.MSG_DISPLAY_PRESENCE_UPDATE, "<new Course>");
                 });
            Logout = new RelayCommand<string>((name) =>
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            });
            AddStudent = new RelayCommand<string>((name) =>
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            });
            SaveChanges = new RelayCommand<string>((name) =>
            {
               
                App.Messenger.NotifyColleagues(App.MSG_SAVE_ACTION, "<new Course>");
                App.Messenger.NotifyColleagues(App.MSG_DISPLAY_PRESENCE_UPDATE, "<new Course>");
                
            });

            if(App.CurrentUser.Role=="teacher")
            {
                newTabForPlanning();
            }





        }       
        private void showPresence(int idOccurence)
        {    
            if (idOccurence != 0)
            {
                Course c = null;
                foreach (var res in App.Model.courseoccurrence)
                {
                    if (res.Id == idOccurence)
                    {
                        c = res.Course;
                    }
                }
                if (c != null)
                {
                    var tab = (from TabItem t in tabControl.Items where (string)t.Header == c.Title select t).FirstOrDefault();
               
                    if (tab == null)
                    {
                        newTabForPresence(idOccurence);
                    }

                    else
                        tab.Focus();
                }

            }
        }
        private Course newTabForCourse(Course course, bool isNew)
        {
           var XCode = from m in App.Model.course

            select m.Code;
            int newCode = 1;
            while (XCode.Contains(newCode))
            {

                newCode += 1;
            }
            if (isNew)
            {
                course.Code = newCode;
            }
            var tmpCourse = "Course ";
            
            var tab = new TabItem()
            {
                
            Header = isNew ? tmpCourse + newCode : tmpCourse + course.Code,
                Content = new CourseDetailView(course, isNew)
            };

            tab.MouseDown += (o, e) =>
            {
                if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
                    tabControl.Items.Remove(o);
            };
            tab.KeyDown += (o, e) =>
            {
                if (e.Key == Key.W && Keyboard.IsKeyDown(Key.LeftCtrl))
                    tabControl.Items.Remove(o);
            };
            tabControl.Items.Add(tab);
            Dispatcher.InvokeAsync(() => tab.Focus());
            return course;
        }
        private void newTabForPresence(int idOccurence)
        {
            Course course = null;
            string dateOccurence = "?????";
            foreach (var res in App.Model.courseoccurrence)
            {
                if (res.Id == idOccurence)
                {
                    course = res.Course;
                    dateOccurence = res.Date.ToString("dd/MM/yyyy");
                }
            }
            var tmpCourse = "Presence - " + course.Code + " - "+ dateOccurence;

            var tab = new TabItem()
            {

                Header = tmpCourse ,
                Content = new PresenceView(idOccurence)
            };

            tab.MouseDown += (o, e) =>
            {
                if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
                    tabControl.Items.Remove(o);
            };
            tab.KeyDown += (o, e) =>
            {
                if (e.Key == Key.W && Keyboard.IsKeyDown(Key.LeftCtrl))
                    tabControl.Items.Remove(o);
            };
            tabControl.Items.Add(tab);
            Dispatcher.InvokeAsync(() => tab.Focus());
       
        }
        private void newTabForPlanning()
        {
            var tab = new TabItem()
            {

                Header = "Planning",
                Content = new PlanningView()
            };


            tabControl.Items.Add(tab);
            Dispatcher.InvokeAsync(() => tab.Focus());
          
        }
    }
}
