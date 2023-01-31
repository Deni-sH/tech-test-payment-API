using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tech_test_payment_api.Context;
using tech_test_payment_api.Entities;


namespace tech_test_payment_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistroController : ControllerBase
    {
        public static readonly string APROVADO = "Pagamento Aprovado";
        public static readonly string AGUARDANDO = "Aguardando Pagamento";
        public static readonly string CANCELADO = "Cancelada";
        public static readonly string ENVIADO = "Enviado para Transportadora";
        public static readonly string ENTREGUE ="Entregue";
        private readonly VendaContext _context;
        public RegistroController(VendaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult RegistrarVenda(Venda registroVendedor)
        {

            if (registroVendedor.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            if (registroVendedor.Itens.Count <= 1)
                return NotFound("Quantidade de produtos não pode ser menor que 1");

            registroVendedor.Status = AGUARDANDO;

            if(registroVendedor == null)
                return BadRequest();

            _context.Add(registroVendedor);           
            _context.SaveChanges();
            
            return CreatedAtAction(nameof(BuscarVenda), new {id = registroVendedor.Id},registroVendedor);
        }

        [HttpGet("{id}")]
        public IActionResult BuscarVenda(int id)
        {
            var registroVendedor = _context.Vendas.Find(id);
            if (registroVendedor == null)
                return NotFound();

                return Ok(registroVendedor);
        }
 

        [HttpPut("{id}")]
        public IActionResult AtualizarVenda(int id, string status)
        {
            var registroBanco = _context.Vendas.Find(id);
            if (registroBanco == null)
            return NotFound();

            if ((status == APROVADO && registroBanco.Status == AGUARDANDO) ||
           (status == CANCELADO && (registroBanco.Status == AGUARDANDO || registroBanco.Status == APROVADO)) ||
           (status == ENVIADO && registroBanco.Status == APROVADO) ||
           (status == ENTREGUE && registroBanco.Status == ENVIADO))
           {
                _context.Vendas.Update(registroBanco);
                _context.SaveChanges();
                return Ok(registroBanco);
            }
            else {
                return Conflict("Conflict");
            }





        }

     }
    
}   