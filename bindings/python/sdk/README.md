# Dapr Bindings (Dapr SDK)

In this quickstart, you'll create a microservice to demonstrate Dapr's bindings API to work with external systems as inputs and outputs. The service listens to input binding events from a system CRON and then outputs the contents of local data to a PostgreSQL output binding.

Visit [this](https://docs.dapr.io/developing-applications/building-blocks/bindings/) link for more information about Dapr and Bindings.

> **Note:** This example leverages the Dapr SDK.  If you are looking for the example using HTTP REST only [click here](../http).

This quickstart includes one service:
 
- Python service `batch`

### Run and initialize PostgreSQL container

1. Open a new terminal, navigate to the `bindings/db` directory relative to the quickstarts root (`cd ../../db` if in a language/app folder like `bindings/csharp/sdk/batch`, or `../../../db` if in `bindings/csharp/sdk`), and run the container with [Docker Compose](https://docs.docker.com/compose/):

<!-- STEP
name: Run and initialize PostgreSQL container
expected_return_code:
background: true
sleep: 60
timeout_seconds: 120
-->

```bash
# Ensure you are in the 'bindings/db' directory from the quickstart root
docker compose up
```

<!-- END_STEP -->

### Run Python service with Dapr

2. Open a new terminal window, navigate to the `./batch` directory (e.g., `cd ./batch`) and run:

<!-- STEP
name: Install python dependencies
-->

```bash
pip3 install -r requirements.txt
```

<!-- END_STEP -->
3. Run the Python service app with Dapr: 

<!-- STEP
name: Run batch-sdk service
working_dir: ./batch
expected_stdout_lines:
  - '== APP == insert into orders (orderid, customer, price) values (1, ''John Smith'', 100.32)'
  - '== APP == insert into orders (orderid, customer, price) values (2, ''Jane Bond'', 15.4)'
  - '== APP == insert into orders (orderid, customer, price) values (3, ''Tony James'', 35.56)'
  - '== APP == Finished processing batch'
expected_stderr_lines:
output_match_mode: substring
sleep: 11
timeout_seconds: 30
-->
    
```bash
dapr run --app-id batch-sdk --app-port 50051 --resources-path ../../../components -- python3 app.py
```

<!-- END_STEP -->
