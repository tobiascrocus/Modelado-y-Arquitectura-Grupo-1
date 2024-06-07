using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biodigestor.Server.Models
{
    public class Temperatura
    {
        public int Id { get; set; }
        public int Temp { get; set; }

    }
}
