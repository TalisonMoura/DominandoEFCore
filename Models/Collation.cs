using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore.Models;

public static class EntityFrameworkExtensions
{
    public static PropertyBuilder<string> WithCollation(this PropertyBuilder<string> property, ECollationType? type = null)
    {
        return property.UseCollation(type switch
        {
            ECollationType.UnSentitive => "SQL_Latin1_General_CP1_CI_AI",
            ECollationType.SentitiveCaseUnsensitiveAccent => "SQL_Latin1_General_CP1_CS_AI",
            ECollationType.UnSentitiveCaseSensitiveAccent => "SQL_Latin1_General_CP1_CI_AS",
            _ => "SQL_Latin1_General_CP1_CS_AS"
        });
    }
}

public enum ECollationType
{
    UnSentitive,
    UnSentitiveCaseSensitiveAccent,
    SentitiveCaseUnsensitiveAccent,
}