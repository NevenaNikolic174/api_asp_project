using FluentValidation;
using MyProject.Application.DTO;
using MyProject.Application.Email;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.UseCases;
using MyProject.Implementation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Implementation.UseCases
{
    public class EfUserCreateCommand : EfUseCase
    {
        private readonly CreateUserValidator _validator;
        private readonly IEmailSender _sender;
        public EfUserCreateCommand(MyDBContext context, CreateUserValidator validator, IEmailSender sender) : base(context)
        {
            _validator = validator;
            _sender = sender;
        }

        public void Execute(UserDTO data)
        {
            _validator.ValidateAndThrow(data);

            User user = new User
            {
                Username = data.Username,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                RoleId = 1,
                IsActive = true,
                Password = BCrypt.Net.BCrypt.HashPassword(data.Password),
                UseCases = new List<UseCaseUser>()
                {
                    new UseCaseUser { UseCaseUserId = 5 },
                    new UseCaseUser { UseCaseUserId = 7 },
                    new UseCaseUser { UseCaseUserId = 12 },
                    new UseCaseUser { UseCaseUserId = 14 },
                    new UseCaseUser { UseCaseUserId = 15 },
                    new UseCaseUser { UseCaseUserId = 22 },
                    new UseCaseUser { UseCaseUserId = 25 },
                    new UseCaseUser { UseCaseUserId = 28 },
                    new UseCaseUser { UseCaseUserId = 30 }
                }
            };

            Context.Users.Add(user);
            Context.SaveChanges();

            _sender.Send(new SendEmailDto
            {
                Content = $@" <h3>Registration Successful!</h3>
                                <p>Hi {data.FirstName},</p>
                                <p>Thank you for signing up. Your registration is complete.</p>
                                <p>If you need assistance, contact us at nevena.nikolic.174.21@ict.edu.rs .</p>",
                SendTo = data.Email,
                Subject = "Welcome :)"
            });

        }
    }
}
