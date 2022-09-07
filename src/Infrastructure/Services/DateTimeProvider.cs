namespace Onion.Infrastructure.Services;

internal sealed class DateTimeProvider : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
