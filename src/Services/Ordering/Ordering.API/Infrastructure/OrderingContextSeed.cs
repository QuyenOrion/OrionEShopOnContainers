using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace Ordering.API.Infrastructure
{
    public class OrderingContextSeed
    {
        public async Task SeedAsync(OrderingContext context, ILogger<OrderingContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(OrderingContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                using (context)
                {
                    context.Database.Migrate();

                    if(!context.CardTypes.Any())
                    {
                        context.AddRange(GetPredefinedCardTypes());
                    }

                    if (!context.OrderStatus.Any())
                    {
                        context.AddRange(GetPredefinedOrderStatus());
                    }

                    await context.SaveChangesAsync();
                }
            });
        }

        private IEnumerable<OrderStatus> GetPredefinedOrderStatus()
        {
            return new List<OrderStatus>()
            {
                OrderStatus.Submitted,
                OrderStatus.AwaitingValidation,
                OrderStatus.StockConfirmed,
                OrderStatus.Paid,
                OrderStatus.Shipped,
                OrderStatus.Cancelled
            };
        }

        private IEnumerable<CardType> GetPredefinedCardTypes()
        {
            return Enumeration.GetAll<CardType>();
        }


        private AsyncRetryPolicy CreatePolicy(ILogger<OrderingContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Error seeding database (attempt {retry} of {retries})", prefix, retry, retries);
                    }
                );
        }
    }
}
