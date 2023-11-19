using System.Data;
using Lucky9.Application.Commands;
using Lucky9.Application.Interfaces;
using Lucky9.Application.IServices;
using Lucky9.Domain.Entities;
using FluentValidation;

namespace Lucky9.Application._BusinessRules;


public static  class ClientUpdatePolicy
{

    /// <summary>
    ///rule #2.1 Edit a client All fields are required
    /// </summary>
    public class EditClientCommandValidator : AbstractValidator<EditClientCommand>
    {        
        public EditClientCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.Email)
             .NotEmpty()
             .WithMessage("Email address is required.")
             .EmailAddress()
             .WithMessage("Please enter a valid email address.");


            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
            //RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.");
        }
    }

    /// <summary>
    /// rule 2.2 If the email has changed, send an email and sync documents after a client is updated (using the mock repositories provided)
    /// </summary>
    public static async Task<bool> AssertClientChangePolicy(EditClientCommand request        
         , IPlayerRepository repo)
    {

        //Client
        var list = await repo.FindAsync(c => c.Email == request.Email);
        var client = list.FirstOrDefault();



        if (client != null)
        {

            var oldEmail = client.Email;

            //ready up update
            repo.Update(client);
            client.FirstName = request.FirstName;
            client.LastName = request.LastName;
            client.Email = request.Email;
            //client.PhoneNumber = request.PhoneNumber;
            

            if(oldEmail.ToLower() != request.Email.ToLower()) {
                //_HACK safe to delete 
                #region---TEST ONLY: Compiler will  automatically erase this in RELEASE mode
#if OVERRIDE

#else

                //send email
                //await emailService.Send(request.Email, "Hi, your profile is updated.");


#endif
                #endregion //////////////END TEST 

            }

            //execute update 
            await repo.SaveChangesAsync();

        }

        return true;
    }

}
