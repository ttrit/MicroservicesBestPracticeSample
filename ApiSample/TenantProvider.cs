namespace MCR.App
{
    public sealed class TenantProvider
    {
        private const string TenantIdHeaderName = "X-TenantId";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string TenantId => _httpContextAccessor
            .HttpContext
            .Request
            .Headers[TenantIdHeaderName];
    }
}
