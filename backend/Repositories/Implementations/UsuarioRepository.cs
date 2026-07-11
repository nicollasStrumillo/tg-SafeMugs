using backend.Data;
using backend.models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Implementations
{
    public class UsuarioRepository:IUsuarioRepository
    {
        private readonly ApplicationDBContext _context;

        public UsuarioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> BuscaPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}