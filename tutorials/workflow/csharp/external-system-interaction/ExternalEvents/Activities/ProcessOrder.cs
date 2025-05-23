using Dapr.Workflow;
using ExternalEvents;

namespace ExternalEvents.Activities;

internal sealed class ProcessOrder : WorkflowActivity<Order, bool>
{
    public override Task<bool> RunAsync(WorkflowActivityContext context, Order order)
    {
        Console.WriteLine($"{nameof(ProcessOrder)}: Processed order: {order.Id}.");
        // Imagine the order being processed by another system
        return Task.FromResult(true);
    }
}
