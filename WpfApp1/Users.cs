using System;
namespace WpfApp1
{
	public class Users
	{
        public int Id { get; set; }
        private string name, sname, age;
        public Users(string name, string sname, string age)
        {
            this.name = name;
            this.sname = sname;
            this.age = age;
        }
        public string Name { get { return name; } set { name = value; } }
        public string SName { get { return sname; } set { sname = value; } }
        public string Age { get { return age; } set { age = value; } }

    }
}


