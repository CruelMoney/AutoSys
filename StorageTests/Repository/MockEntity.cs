#region Using

using Storage.Repository;

#endregion

namespace StorageTests.Repository
{
    public class MockEntity : IEntity
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
}