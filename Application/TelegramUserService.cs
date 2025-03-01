using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;

namespace Application
{
    public class TelegramUserService : ITelegramUserService
    {
        private readonly ITelegramUserRepository _userRepository;
        public TelegramUserService(ITelegramUserRepository telegramUser)
        {
            _userRepository = telegramUser;
        }
        public async Task<string> GetNameById(long id, CancellationToken cancellationToken)
        {
            try
            {
                return await _userRepository.GetNameById(id, cancellationToken);
            }
            catch (Exception ex) 
            {
                throw;
            }
        }
        public async Task AddUser(TelegramUser user, CancellationToken cancellationToken) 
        {
            await _userRepository.AddUser(user, cancellationToken);
        }
    }
}
