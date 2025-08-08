using Abp.Domain.Values;

namespace FGC.Domain.Common.ValueObjects
{
    public class DateUtc : ValueObject
    {
        public DateTime Value { get; }

        public DateUtc(DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Date should be in UTC.");
             
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator DateTime(DateUtc d) => d.Value;
        public override string ToString() => Value.ToString("O");
    }
}       
