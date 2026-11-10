using Fcg.Users.Domain.ValueObjects;

namespace Fcg.Users.Tests;

public class EmailTests
{
    [Fact]
    public void Deve_criar_email_quando_endereco_for_valido()
    {
        // Arrange
        var endereco = "usuario@exemplo.com";

        // Act
        var email = new Email(endereco);

        // Assert
        Assert.Equal(endereco, email.Endereco);
    }

    [Fact]
    public void Deve_rejeitar_email_quando_formato_for_invalido()
    {
        // Arrange
        var endereco = "email-invalido";

        // Act
        var excecao = Assert.Throws<ArgumentException>(() => new Email(endereco));

        // Assert
        Assert.NotNull(excecao);
    }

    [Fact]
    public void Deve_rejeitar_email_vazio()
    {
        // Arrange
        var endereco = string.Empty;

        // Act
        var excecao = Assert.Throws<ArgumentException>(() => new Email(endereco));

        // Assert
        Assert.NotNull(excecao);
    }

    [Fact]
    public void Deve_rejeitar_email_com_apenas_espacos()
    {
        // Arrange
        var endereco = "   ";

        // Act
        var excecao = Assert.Throws<ArgumentException>(() => new Email(endereco));

        // Assert
        Assert.NotNull(excecao);
    }

    [Fact]
    public void Deve_retornar_endereco_no_ToString()
    {
        // Arrange
        var endereco = "usuario@exemplo.com";
        var email = new Email(endereco);

        // Act
        var resultado = email.ToString();

        // Assert
        Assert.Equal(endereco, resultado);
    }
}