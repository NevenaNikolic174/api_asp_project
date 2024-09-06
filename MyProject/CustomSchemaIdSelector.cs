using Swashbuckle.AspNetCore.SwaggerGen;
using System;

public class CustomSchemaIdSelector 
{
    public string SelectSchemaId(Type type)
    {
        return type.FullName ?? type.Name;
    }
}
