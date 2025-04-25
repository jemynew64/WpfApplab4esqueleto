using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplab4
{
    public class DetallePedido
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public int IdProducto { get; set; }
        public decimal PrecioUnidad { get; set; }
        public int Cantidad { get; set; }
        public float Descuento { get; set; }
    }
}
