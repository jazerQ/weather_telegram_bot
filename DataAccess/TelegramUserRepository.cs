using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class TelegramUserRepository : ITelegramUserRepository
    {
        private readonly TelegramDbContext _context;
        public TelegramUserRepository(TelegramDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(TelegramUser user, CancellationToken cancellationToken)
        {
            if (await _context.TelegramUser.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken) == null)
            {
                await _context.TelegramUser.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                await Update(user.Id, user.Name, cancellationToken);
            }
        }
        public async Task<string> GetNameById(long id, CancellationToken cancellationToken) 
        {
            try
            {
                return await _context.TelegramUser.Where(u => u.Id == id).Select(u => u.Name).SingleOrDefaultAsync(cancellationToken) ?? throw new Exception("Не нашел имени, введите /setname {name}");
            }
            catch (Exception ex) 
            {
                throw;
            }
        }
        public async Task Update(long id, string name, CancellationToken cancellationToken)
        {
            await _context.TelegramUser
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u.SetProperty(tu => tu.Name, name), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
