using ClassLibrary.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Validators
{
    public class ProductValidator : AbstractValidator<ProductItem>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Name).NotNull();
            RuleFor(product => product.Name).MaximumLength(50);

            RuleFor(product => product.Price).NotNull();
            RuleFor(product => product.Price).GreaterThanOrEqualTo(0);

            RuleFor(product => product.Stock).NotNull();
            RuleFor(product => product.Stock).GreaterThanOrEqualTo(0);

            RuleFor(product => product.ImageUrl).MaximumLength(250);

            RuleFor(product => product.Description).MaximumLength(250);
        }
    }
}
