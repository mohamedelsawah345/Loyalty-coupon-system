using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.Service.Implementation.Validations
{
    public class GreaterThanLastSequenceAttribute : ValidationAttribute
    {
        private readonly Func<string> _getLastSequenceNumber;

        public GreaterThanLastSequenceAttribute(Func<string> getLastSequenceNumber)
        {
            _getLastSequenceNumber = getLastSequenceNumber;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("SequenceStart is required.");
            }

            var sequenceStart = value.ToString();
            var lastSequenceNumber = _getLastSequenceNumber();

            if (string.IsNullOrEmpty(lastSequenceNumber) || string.Compare(sequenceStart, lastSequenceNumber) <= 0)
            {
                return new ValidationResult($"SequenceStart must be greater than the last recorded sequence number: {lastSequenceNumber}");
            }

            return ValidationResult.Success;
        }
    }
}
