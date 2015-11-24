using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.DTO;

namespace Logic.Model
{
    public class Criteria
    {    
        public enum CriteriaType
        {
            Contains,
            Equals,
            LargerThan,
            SmallerThan    
        }

       
        public DataField Field { get; set; }

        public CriteriaType _CriteriaType { get; set; } 

        public bool CriteriaIsMet( DataField _Field)
        {
           
                
            if (_Field.FieldType == DataField.DataType.Enumeration)
            {
                return _Field.TypeInfo.First().Equals(_Field.Data.First());
            }
            else if (_Field.FieldType == DataField.DataType.Boolean)
            {
                return bool.Parse(_Field.Data.First());
            }
            return false;
        }

         

    }
}
