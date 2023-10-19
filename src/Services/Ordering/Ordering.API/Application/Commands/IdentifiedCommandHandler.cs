namespace OrionEShopOnContainer.Services.Ordering.API.Application.Commands;

public abstract class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R> where T : IRequest<R>
{
    public Task<R> Handle(IdentifiedCommand<T, R> request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
