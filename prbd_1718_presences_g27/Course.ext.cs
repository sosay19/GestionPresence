using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_1718_presences_g27
{
    public partial class Course : ErrorManager
    {
        //public Course()
        //{
        //    DateTime = DateTime.Now;
        //}

        //public bool IsPrivate
        //{
        //    get { return Private != 0; }
        //    set { Private = (byte)(value ? 1 : 0); }
        //}
        //public int IsPrivate
        //{
        //    get { return  5; }
        //    set {Student. =   56; }
        //}

        public override bool Validate()
        {
            ClearErrors();

            if (Title == null)
                AddError("Author", "Author is required");

            RaiseErrors();

            return !HasErrors;
        }

        public override string ToString()
        {
            return $"<Course >";

        }
    }
}
