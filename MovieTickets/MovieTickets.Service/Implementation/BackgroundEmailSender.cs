using MovieTickets.Domain.DomainModels;
using MovieTickets.Repository.Interface;
using MovieTickets.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTickets.Service.Implementation
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<EmailMessage> _mailRepository;
        public BackgroundEmailSender(IEmailService emailService, IRepository<EmailMessage> mailRepository) { 
            _emailService = emailService;
            _mailRepository = mailRepository;
        }
        public async Task DoWork()
        {
            var emailsToSend = _mailRepository.GetAll().Where(z => !z.Status).ToList();
            foreach (var email in emailsToSend)
            {
                email.Status = true;
                _mailRepository.Update(email);
            }
            await _emailService.SendEmailAsync(emailsToSend);
        }
    }
}
