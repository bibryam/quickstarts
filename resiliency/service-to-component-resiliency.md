# Dapr Resiliency: State Management

In this QuickStart, you will run a microservice application that continuously persists and retrieves state via Dapr's state management API. When operations to the state store begin to fail, Dapr resiliency policies are applied.

Visit the documentation about [Dapr resiliency](https://docs.dapr.io/operations/resiliency/resiliency-overview/) link for more information

This quickstart includes one service:

- Client service `order-processor`
- Redis component spec `statestore.yaml`
- Resiliency spec `resiliency.yaml`

### Run the client service with Dapr and resiliency enabled

1. Navigate to the app directory, install dependencies, and run the service with resiliency.

Note: Dapr loads components from all specified resource paths. Here, we're loading the application's standard state store component (from the `state_management` quickstart's `resources` directory) and the `resiliency.yaml` spec (from the `resiliency` quickstart's `components` directory) which applies policies to it. The `resiliency.yaml` is located at `quickstarts/resiliency/components/resiliency.yaml`.

### C# example:

```bash
cd ../state_management/csharp/sdk/order-processor
dotnet restore
dotnet build
# The paths assume execution from state_management/csharp/sdk/order-processor:
# ../../../resources/ points to state_management/resources/
# ../../../../resiliency/components/ points to resiliency/components/
dapr run --app-id order-processor \
  --resources-path ../../../resources/ \
  --resources-path ../../../../resiliency/components/ \
  -- dotnet run
```

### Go example:

```bash
cd ../state_management/go/sdk/order-processor
go build .
# The paths assume execution from state_management/go/sdk/order-processor:
dapr run --app-id order-processor \
  --resources-path ../../../resources/ \
  --resources-path ../../../../resiliency/components/ \
  -- go run .
```

### Java example:

```bash
cd ../state_management/java/sdk/order-processor
mvn clean install
# The paths assume execution from state_management/java/sdk/order-processor:
dapr run --app-id order-processor \
  --resources-path ../../../resources/ \
  --resources-path ../../../../resiliency/components/ \
  -- java -jar target/OrderProcessingService-0.0.1-SNAPSHOT.jar
```

### JavaScript example:

```bash
cd ../state_management/javascript/sdk/order-processor
npm install
# The paths assume execution from state_management/javascript/sdk/order-processor:
dapr run --app-id order-processor \
  --resources-path ../../../resources/ \
  --resources-path ../../../../resiliency/components/ \
  -- npm start
```

### Python example:

```bash
cd ../state_management/python/sdk/order-processor
pip3 install -r requirements.txt
# The paths assume execution from state_management/python/sdk/order-processor:
dapr run --app-id order-processor \
  --resources-path ../../../resources/ \
  --resources-path ../../../../resiliency/components/ \
  -- python3 app.py
```

### Expected output:

```bash
== APP == Saving Order:  { orderId: '1' }
== APP == Getting Order:  { orderId: '1' }
== APP == Saving Order:  { orderId: '2' }
== APP == Getting Order:  { orderId: '2' }
== APP == Saving Order:  { orderId: '3' }
== APP == Getting Order:  { orderId: '3' }
== APP == Saving Order:  { orderId: '4' }
== APP == Getting Order:  { orderId: '4' }
```
<!-- END_STEP -->

### Simulate a component failure by stopping the Redis container instance

In a new terminal window, stop the Redis container that's running on your machine:

```bash
docker stop dapr_redis
```

### Observe retry and circuit breaker policies are applied:

Policies defined in the resiliency.yaml spec:

```yaml
retryForever:
  policy: constant
  duration: 5s
  maxRetries: -1

circuitBreakers:
  simpleCB:
  maxRequests: 1
  timeout: 5s
  trip: consecutiveFailures >= 5
```

Applied policies:

```bash
INFO[0006] Error processing operation component[statestore] output. Retrying...
INFO[0026] Circuit breaker "simpleCB-statestore" changed state from closed to open
INFO[0031] Circuit breaker "simpleCB-statestore" changed state from open to half-open
INFO[0031] Circuit breaker "simpleCB-statestore" changed state from half-open to open
```

### Simulate the component recovering by restarting the Redis container instance:

```bash
docker start dapr_redis
```

### Observe orders have resumed sequentially

```bash
INFO[0036] Recovered processing operation component[statestore] output.
== APP == Saving Order:  { orderId: '5' }
== APP == Getting Order:  { orderId: '5' }
== APP == Saving Order:  { orderId: '6' }
== APP == Getting Order:  { orderId: '6' }
== APP == Saving Order:  { orderId: '7' }
== APP == Getting Order:  { orderId: '7' }
== APP == Saving Order:  { orderId: '8' }
== APP == Getting Order:  { orderId: '8' }
== APP == Saving Order:  { orderId: '9' }
== APP == Getting Order:  { orderId: '9' }
```

### Stop the app with Dapr

```bash
dapr stop --app-id order-processor
```

