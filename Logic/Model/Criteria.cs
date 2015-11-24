using System;
using System.Collections.Generic;

namespace Logic.Model
{
    public class Criteria
    {
  
        public enum Rule
        {
           LargerThan,
           SmallerThan,
           Equals
        }

        public Rule CriteriaRule;
        public KeyValuePair<Item.ItemType, string> Field;


        public Criteria(Item.ItemType fieldType, Rule criteriaRule, string field)
        {
            CriteriaRule = criteriaRule;
            Field = new KeyValuePair<Item.ItemType, string>(fieldType, field);
        }


        //TODO implement using the rules
        public bool FieldMeetsRule(string field)
        {
            throw new NotImplementedException();
            return field == Field.Value;
        }
    }
}
