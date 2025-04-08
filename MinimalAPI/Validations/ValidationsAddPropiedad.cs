using FluentValidation;
using MinimalAPI.Models.DTOs;

namespace MinimalAPI.Validations
{
    public class ValidationsAddPropiedad : AbstractValidator<AddPropiedadDTO>
    {
        public ValidationsAddPropiedad()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage("El nombre es requerido")
                .Length(3, 50)
                .WithMessage("El nombre debe tener entre 3 y 50 caracteres");
            RuleFor(x => x.Descripcion)
                .NotEmpty()
                .WithMessage("La descripcion es requerida")
                .Length(3, 200)
                .WithMessage("La descripcion debe tener entre 3 y 200 caracteres");
            RuleFor(x => x.Ubicacion)
                .NotEmpty()
                .WithMessage("La direccion es requerida")
                .Length(3, 100)
                .WithMessage("La direccion debe tener entre 3 y 100 caracteres");
        }
    }
}
