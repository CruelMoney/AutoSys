#region Using

using Storage.Repository;

#endregion

namespace StudyConfigurationServer.Models
{
    public class StoredString : IEntity
    {
        public StoredString(string s)
        {
            Value = s;
        }

        public StoredString()
        {
        }

        public string Value { get; set; }
        public int ID { get; set; }
    }
}