using System;
using System.Collections.Generic;
using Logic.Model.Data;
using Storage.Repository;

namespace Logic.Model
{
    public class UserTask : IEntity
    {
        public int Id { get; set; }
        public User AssociatedUser { get; set; }
        public bool TaskDone { get; set; }
        public Item AssociatedItem { get; set; }
        public KeyValuePair<Item.FieldType, string> FieldToFillOut { get; set; }

        public UserTask(StoredTask storedTask)
        {
            throw new NotImplementedException();
        }

    }
}
