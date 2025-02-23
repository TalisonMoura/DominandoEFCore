using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Modules;

public static class Functions
{
    [DbFunction(name: "LEFT", IsBuiltIn = true)]
    public static string Left(string data, int quantity)
        => throw new NotImplementedException();

    public static void RegisterFunctions(this ModelBuilder modelBuilder)
    {
        foreach (var function in typeof(Functions).GetMethods().Where(x => Attribute.IsDefined(x, typeof(DbFunctionAttribute))))
            modelBuilder.HasDbFunction(function);
    }
}