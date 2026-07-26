using backend.Data;
using backend.models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementations
{
    public class SenhaTokenRepository : ISenhaTokenRepository
    {
        private readonly ApplicationDBContext _context;

        public SenhaTokenRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task SalvarAsync(SenhaToken senhaToken)
        {
            await _context.SenhaTokens.AddAsync(senhaToken);
            await _context.SaveChangesAsync();
        }

        public async Task<SenhaToken?> ObterPorTokenEUsuarioAsync(string token, int usuarioId)
        {
            return await _context.SenhaTokens
                .FirstOrDefaultAsync(t => t.Token == token && t.UsuarioId == usuarioId);
        }

        public async Task DeletarAsync(SenhaToken senhaToken)
        {
            _context.SenhaTokens.Remove(senhaToken);
            await _context.SaveChangesAsync();
        }
    }
}
