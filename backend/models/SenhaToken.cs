using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.models
{
    public class SenhaToken
    {
        public int Id { get; set; }
        public int UsuarioId { get; set;}
        public string? Token { get; set; }
        public DateTimeOffset ExpiraEm {get; set;}
    
    }
}