namespace Infrastructure;

public class ApiBase
{
	protected readonly SecurityGateway SecurityGateway;

	protected ApiBase(SecurityGateway security)
	{
		SecurityGateway = security;
	}
}
