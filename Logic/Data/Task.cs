using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.Repository;

namespace Logic.Data
{
    public class Task
    {
        public User AssociatedUser { get; set; }
        public bool TaskDone { get; set; }
        public Item AssociatedItem { get; set; }
        public KeyValuePair<Item.FieldType, string> FieldToFillOut { get; set; }



    }
}
