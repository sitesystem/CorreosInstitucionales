namespace CorreosInstitucionales.Client
{
    public static class AuthorizationPolicyExtensions
    {
        public static IServiceCollection ConfigureAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("[Rol] Developer / Jefe de UDI / Administrador", rol => rol.RequireClaim("Rol", "1", "2", "3"));
            });

            return services;
        }
    }
}
