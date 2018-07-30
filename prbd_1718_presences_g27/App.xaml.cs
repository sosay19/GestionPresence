using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Threading;
using System.Globalization;
using prbd_1718_presences_g27.Properties;

namespace prbd_1718_presences_g27
{
    public partial class App : Application
    {
        public const string MSG_NEW_COURSE = "MSG_NEW_COURSE";
        public const string MSG_NEW_USER = "MSG_NEW_USER";

        public const string MSG_TITLE_CHANGED = "MSG_TITLE_CHANGED";
        public const string MSG_CODE_CHANGED = "MSG_CODE_CHANGED";
        public const string MSG_CANCEL_ACTION = "MSG_CANCEL_ACTION";
        public const string MSG_SAVE_ACTION = "MSG_SAVE_ACTION";
        public const string MSG_LOGOUT_ACTION = "MSG_LOGOUT_ACTION";
        public const string MSG_COURSE_CHANGED = "MSG_COURSE_CHANGED";
        
        public const string MSG_USER_CHANGED = "MSG_USER_CHANGED";
        public const string MSG_DISPLAY_COURSE = "MSG_DISPLAY_COURSE";
        

        public const string MSG_DISPLAY_PRESENCE = "MSG_DISPLAY_PRESENCE";
        public const string MSG_DISPLAY_PRESENCE_UPDATE = "MSG_DISPLAY_PRESENCE_UPDATE";
        //public const string MSG_DISPLAY_PLANNING_MONDAY = "MSG_DISPLAY_PLANNING_MONDAY";
        //public const string MSG_DISPLAY_PLANNING_TUESDAY = "MSG_DISPLAY_PLANNING_TUESDAY";
        //public const string MSG_DISPLAY_PLANNING_WEDNESDAY = "MSG_DISPLAY_PLANNING_WEDNESDAY";
        //public const string MSG_DISPLAY_PLANNING_THURSDAY = "MSG_DISPLAY_PLANNING_THURSDAY";
        //public const string MSG_DISPLAY_PLANNING_FRIDAY = "MSG_DISPLAY_PLANNING_FRIDAY";
        //public const string MSG_DISPLAY_PLANNING_SATURDAY = "MSG_DISPLAY_PLANNING_SATURDAY";
        public const string MSG_DISPLAY_USER = "MSG_DISPLAY_USER";

        public const string MSG_CLOSE_TAB = "MSG_CLOSE_TAB";
        public const string MSG_LOGGED_IN = "MSG_LOGGED_IN";

        public static Messenger Messenger { get; } = new Messenger();
        public static Entities Model { get; private set; } = new Entities();

        public static User CurrentUser { get; set; }
        public static Course CurrentCourse { get; set; }
        public static DateTime DatesBeginPlanning { get; set; }
        public static List<Course> ListNewCourse = new List<Course>();
        public static List<Courseoccurrence> ListCourseOccurence = new List<Courseoccurrence>();
        
       

        public App()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Culture);

            PrepareDatabase();
            
            ColdStart();
            App.Messenger.Register<string>(App.MSG_CANCEL_ACTION, (s) =>
            {

                CancelChanges();

            });
            App.Messenger.Register<string>(App.MSG_SAVE_ACTION, (s) =>
            {
                if (CanSaveOrCancelAction())
                    SaveChanges();

            });
        }
        private bool CanSaveOrCancelAction() /* on va créer  une liste de vérification*/
        {
            
            return true;

        }
        public static void SaveChanges()
        {
            foreach(var course in ListNewCourse)
            {
                Console.WriteLine("course" + course);
                App.Model.course.Add(course);
            }
            foreach (var courseOc in ListCourseOccurence)
            {
                Console.WriteLine("courseOc " + courseOc.Date);
                //Model.courseoccurrence.Add(courseOc);
            }

            App.Model.SaveChanges();
            ListNewCourse.Clear();
        }
        public static void CancelChanges()
        {

            Model.Dispose(); // Retire de la mémoire le modèle actuel
            Model = new Entities(); // Recréation d'un nouveau modèle à partir de la DB
            Model.Database.Log = m => Console.Write(m);
        }

        private void ColdStart()
        {
            //Model.user.Find("DUMMY");
        }

        private void PrepareDatabase()
        {
            // Donne une valeur à la propriété "DataProperty" qui est utilisée comme dossier de base dans App.config pour
            // la connection string vers la DB. Cette valeur est calculée en chemin relatif à partir du dossier de 
            // l'exécutable, soit <dossier projet>/bin/Debug.
            var projectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\"));
            var dbPath = Path.GetFullPath(Path.Combine(projectPath, "database"));
            Console.WriteLine("Database path: " + dbPath);
            AppDomain.CurrentDomain.SetData("DataDirectory", projectPath);

            // Si la base de données n'existe pas, la créer en exécutant le script SQL
            if (!File.Exists(Path.Combine(dbPath, @"prbd_1718_presences_g27.mdf")))
            {
                Console.WriteLine("Creating database...");
                string script = File.ReadAllText(Path.Combine(dbPath, @"prbd_1718_presences_g27.sql"));

                // dans le script, on remplace "{DBPATH}" par le dossier où on veut créer la DB
                script = script.Replace("{DBPATH}", dbPath);

                // On splitte le contenu du script en une liste de strings, chacune contenant une commande SQL.
                // Pour faire le split, on se sert des commandes "GO" comme délimiteur.
                IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                // On se connecte au driver de base de données "(localdb)\MSSQLLocalDB" qui permet de travailler avec des
                // fichiers de données SQL Server attachés sans nécessiter qu'une instance de SQL Server ne soit présente.
                string sqlConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True";
                SqlConnection connection = new SqlConnection(sqlConnectionString);
                connection.Open();
                // On exécute les commandes SQL une par une.
                foreach (string commandString in commandStrings)
                    if (commandString.Trim() != "")
                        using (var command = new SqlCommand(commandString, connection))
                            command.ExecuteNonQuery();
                connection.Close();
            }
        }
        
    }
}
