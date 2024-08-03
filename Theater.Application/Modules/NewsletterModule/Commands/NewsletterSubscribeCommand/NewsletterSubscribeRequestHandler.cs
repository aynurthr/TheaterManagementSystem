using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;

namespace Theater.Application.Modules.NewsletterModule.Commands.NewsletterSubscribeCommand
{
    public class NewsletterSubscribeRequestHandler : IRequestHandler<NewsletterSubscribeRequest>
    {
        private readonly INewsletterRepository _repository;
        private readonly IEmailService _emailService;
        private readonly ICryptoService _cryptoService;
        private readonly IActionContextAccessor _ctx;


        public NewsletterSubscribeRequestHandler(INewsletterRepository repository, IEmailService emailService, ICryptoService cryptoService, IActionContextAccessor ctx)
        {
            _repository = repository;
            _emailService = emailService;
            _cryptoService = cryptoService;
            _ctx = ctx;

        }

        public async Task Handle(NewsletterSubscribeRequest request, CancellationToken cancellationToken)
        {
            var subscription = await _repository.GetAll()
                .FirstOrDefaultAsync(n => n.Email == request.Email, cancellationToken);

            if (subscription != null && subscription.IsSubscribed)
            {
                throw new Exception("Email is already subscribed.");
            }

            if (subscription == null)
            {
                subscription = new Newsletter
                {
                    Email = request.Email,
                    IsSubscribed = true,
                    SubscribedAt = DateTime.UtcNow,
                    LastModified = null
                };

                await _repository.AddAsync(subscription, cancellationToken);
            }
            else
            {
                subscription.IsSubscribed = true;
                subscription.LastModified = DateTime.UtcNow;

                _repository.Edit(subscription);
            }

            await _repository.SaveAsync(cancellationToken);

            var token = _cryptoService.Encrypt(request.Email);

            string host = $"{_ctx.ActionContext.HttpContext.Request.Scheme}://{_ctx.ActionContext.HttpContext.Request.Host}";
            var unsubscribeLink = $"{host}/unsubscribe?token={token}";

            var emailContent = $" <h2>Welcome to Kazan State Theater of The Young Spectators</h2>\r\n        " +
                $"<p>Hello, Dear Customer,</p>\r\n       " +
                $" <p>Thank you for subscribing to our newsletter.</p>\r\n       <i>Best regards,<br>Kazan State Theater of The Young Spectators</i>";

            await _emailService.SendEmailAsync(request.Email, "Newsletter Subscription", emailContent);

        }
    }
}
