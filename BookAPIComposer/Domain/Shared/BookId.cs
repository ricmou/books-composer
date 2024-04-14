using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace BookAPIComposer.Domain.Shared
{
    public class BookId : EntityId
    {

        [JsonConstructor]
        public BookId(String value) : base(IsValidBookId(value))
        {
        }
        
        protected override Object createFromString(String text){
            return text;
        }
        
        public override String AsString(){
            var obj = (string) base.ObjValue;
            return obj;
        }
        
        //This is most definitely a hack...
        private static string IsValidBookId(string isbn)
        {
            if (string.IsNullOrEmpty(isbn) || !Regex.IsMatch(isbn, @"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$"))
            {
                throw new BusinessRuleValidationException("Invalid ISBN");
            }

            return isbn;
        }
    }
}