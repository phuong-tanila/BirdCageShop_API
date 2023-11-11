using BusinessObjects.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Helper
{
    //public class OrderValidationContext : AuthorizationValidationContext<Order>
    //{
    //    public OrderValidationContext(ValidationContext validationContext)
    //      : base(validationContext)
    //    {
    //    }
    //}
    //public class ValidOrderDetailsAttribute : ValidationAttribute
    //{
    //    private ValidationContext _context;

    //    public ValidOrderDetailsAttribute(IValidationContext contextAccessor)
    //    {
    //        _context = contextAccessor;
    //    }
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        var orderDetails = (List<OrderDetail>)value;

    //        foreach (var detail in orderDetails)
    //        {
    //            if (!validationContext.ModelState.IsValid(detail))
    //            {
    //                return new ValidationResult("Invalid order detail");
    //            }
    //        }

    //        return ValidationResult.Success;

    //    }

    //}

    //public class OrderValidator : AbstractValidator<Order>
    //{
    //    public OrderValidator()
    //    {
    //        RuleFor(x => x.OrderDetails).SetValidator(new OrderDetailsValidator());
    //    }
    //}

    //public class OrderDetailsValidator : AbstractValidator<ICollection<OrderDetail>>
    //{
    //    public OrderDetailsValidator()
    //    {
    //        RuleForEach(x => x)
    //          .SetValidator(new OrderDetailValidator());
    //    }
    //}

    //public class OrderDetailValidator : AbstractValidator<OrderDetail>
    //{
    //    public OrderDetailValidator()
    //    {
    //        RuleFor(x => x.Quantity).NotEmpty(); // validate property Required
    //    }
    //}
}
