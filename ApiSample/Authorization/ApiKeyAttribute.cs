using Microsoft.AspNetCore.Mvc;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthorizationFilter))
        {
            
        }
}