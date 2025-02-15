namespace DominandoEFCore.Models;

public class Document
{
    public Guid Id { get; set; }

    private string _cpf;
    public string Cpf() => _cpf;

    public void SetCpf(string cpf)
    {
        ArgumentException.ThrowIfNullOrEmpty(cpf);
        _cpf = cpf;
    }
}