using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.Repository;

namespace StorageTests.Repository
{
    public class MockEntity : IEntity
    {
        public int Id { get; set; }
    }
}
