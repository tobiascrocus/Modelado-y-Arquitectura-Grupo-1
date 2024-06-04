using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Productos.Server.Models
{
    public class Producto
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18, 2")]
        [DisplayFormat(DataFormatString = "{0 : C2}")]
        public decimal Precio { get; set; }

    }
}
