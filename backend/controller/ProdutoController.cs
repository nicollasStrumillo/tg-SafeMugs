using backend.database;
using backend.models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.controller
{
    [Route("api/produtos")]    
    [ApiController]
    public class ProdutoController:ControllerBase
    {
        ApplicationDBContext context;
        public ProdutoController(ApplicationDBContext _context)
        {
            context = _context;
        }
        [HttpGet("lista")]
        public async Task<IActionResult> getProduto( ){
            var produtoSearch =  await context.produtos.ToListAsync();
            if(produtoSearch != null ){
            return Ok(produtoSearch); 
            }
            return NotFound();
        }
    }

}
