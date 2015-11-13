using System.Collections.Generic;
using Storage.Repository;

namespace Logic.Data
{
    /// <summary>
    ///     This class defines a bibliographic item.
    /// </summary>
    public class StoredItem:IEntity
    {
        public int Id { get; set; }
    }
}