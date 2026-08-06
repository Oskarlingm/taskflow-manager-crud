using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.DTOs;

public class CreateTaskDto
{
    [Required(ErrorMessage = "El titulo es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El titulo debe tener entre 3 y 100 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripcion no puede superar 500 caracteres.")]
    public string Description { get; set; } = string.Empty;
}
