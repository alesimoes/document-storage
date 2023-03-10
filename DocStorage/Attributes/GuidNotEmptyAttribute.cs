using System.ComponentModel.DataAnnotations;

namespace DocStorage.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GuidNotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || Guid.Empty.Equals(value))
            {
                return false;
            }

            return true;
        }
    }
}
