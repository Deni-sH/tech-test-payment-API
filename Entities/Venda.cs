using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tech_test_payment_api.Entities
{
    public class Venda
    {
        public int Id { get; set; }
        public ICollection<Item> Itens { get; set; }
        public DateTime Data { get; set; }
        public Vendedor Vendedor { get; set; }
        public string Status { get; set; }
          
    }

     

}
