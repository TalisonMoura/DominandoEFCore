using System.Net;

namespace DominandoEFCore.Models;

public class Converter
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public Status Status { get; set; }
    public bool IsDeleted { get; set; }
    public Version Version { get; set; }
    public IPAddress AddresIP { get; set; }

}

public enum Version
{
    EFCore1, EFCore2, EFCore3, EFCore4, EFCore5,
}

public enum Status
{
    Sent,
    Returned,
    InAnalysis,
}