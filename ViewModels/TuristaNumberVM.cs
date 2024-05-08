using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Turismo.Domain.Entities;

namespace Turismo.web.ViewModels
{
    public class TuristaNumberVM
    {
        public TuristaNumbers TuristaNumbers { get; set; } = null!;

        [ValidateNever]
        public IEnumerable<SelectListItem>? TuristaList { get; set; }
    }
}