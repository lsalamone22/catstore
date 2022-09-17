using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace catstore.Models
{
    [Table("t_proforma")]
    public class PRO
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id {get;set;}
        public string userID {get;set;}

        public Producto? Producto { get; set; }
        public int Cantidad { get; set; }

        public Decimal Precio { get; set; }
        public String Status { get; set; } ="PENDIENTE";
        
    }
}