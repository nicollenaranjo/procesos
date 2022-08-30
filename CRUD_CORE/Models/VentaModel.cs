using System.ComponentModel.DataAnnotations;

namespace CRUD_CORE.Models
{
    public class VentaModel
    {
        public int idVenta { get; set; }
        public string? Nombre { get; set; }
        public int Precio { get; set; }
        public DateTime DiaVenta { get; set; }
    }
}
