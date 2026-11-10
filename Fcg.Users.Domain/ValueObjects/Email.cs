using System.Net.Mail;

namespace Fcg.Users.Domain.ValueObjects;

public class Email
{
    public string Endereco { get; private set; } = string.Empty;

    protected Email()
    {
    }

    public Email(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco))
            throw new ArgumentException("O E-mail não pode ser vazio.");

        try
        {
            var email = new MailAddress(endereco);

            if (email.Address != endereco)
                throw new ArgumentException("O E-mail é inválido.");
        }
        catch
        {
            throw new ArgumentException("O formato do E-mail é inválido.");
        }

        Endereco = endereco;
    }

    public override string ToString()
    {
        return Endereco;
    }
}