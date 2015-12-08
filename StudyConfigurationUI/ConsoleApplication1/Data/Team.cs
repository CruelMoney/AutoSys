using System.Collections.Generic;

namespace ConsoleApplication1.Data
{
    public class Team : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] UserIDs { get; set; } 
        public virtual List<Study> Studies { get;  set;}  // reference til Study (one to many)
        public virtual List<User> Users { get; set; }     // reference til Users (one to many)
        public string Metadata { get; set; }
    }

    
}
