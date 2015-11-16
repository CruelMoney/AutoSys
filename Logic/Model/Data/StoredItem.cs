using Storage.Repository;

namespace Logic.Model.Data
{
    /// <summary>
    ///     This class defines a bibliographic item.
    /// </summary>
    public class StoredItem:IEntity
    {
        public int Id { get; set; }
        public void Update(Item dataItem)
        {
            throw new System.NotImplementedException();
        }
    }
}