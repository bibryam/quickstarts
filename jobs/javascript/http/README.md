# Dapr Jobs

In this quickstart, you'll schedule, get, and delete a job using Dapr's Job API. This API is responsible for scheduling and running jobs at a specific time or interval.

Visit [this](https://docs.dapr.io/developing-applications/building-blocks/jobs/) link for more information about Dapr and the Jobs API.

This quickstart includes two apps:

- `job-scheduler`, responsible for scheduling, retrieving and deleting jobs.
- `job-service`, responsible for handling the triggered jobs.

## Run the app with the template file

This section shows how to run both applications at once using [multi-app run template files](https://docs.dapr.io/developing-applications/local-development/multi-app-dapr-run/multi-app-overview/) with `dapr run -f .`. This enables to you test the interactions between multiple applications and will `schedule`, `run`, `get`, and `delete` jobs within a single process.

1. Install dependencies:

<!-- STEP
name: Install Node dependencies for job-service
-->

```bash
cd ./job-service
npm install
```

<!-- END_STEP -->

<!-- STEP
name: Install Node dependencies for job-scheduler
-->

```bash
cd ./job-scheduler
npm install
```

<!-- END_STEP -->

2. Open a new terminal window and run the multi app run template:

<!-- STEP
name: Run multi app run template
expected_stdout_lines:
  - '== APP - job-scheduler == Job Scheduled: R2-D2'
  - '== APP - job-scheduler == Job Scheduled: C-3PO'
  - '== APP - job-service == Received job request...'
  - '== APP - job-service == Starting droid: R2-D2'
  - '== APP - job-service == Executing maintenance job: Oil Change'
  - '== APP - job-service == Received job request...'
  - '== APP - job-service == Starting droid: C-3PO'
  - '== APP - job-service == Executing maintenance job: Limb Calibration'
expected_stderr_lines:
output_match_mode: substring
match_order: none
background: true
sleep: 60
timeout_seconds: 120
-->

```bash
dapr run -f .
```

The terminal console output should look similar to this, where:

- The `R2-D2` job is being scheduled.
- The `R2-D2` job is being retrieved.
- The `C-3PO` job is being scheduled.
- The `C-3PO` job is being retrieved.
- The `R2-D2` job is being executed after 15 seconds.
- The `C-3PO` job is being executed after 20 seconds.

```text
== APP - job-scheduler == Job Scheduled: R2-D2
== APP - job-scheduler == Job details: {"name":"R2-D2", "dueTime":"15s", "data":{"value":{"value":"R2-D2:Oil Change"}}}
== APP - job-scheduler == Job Scheduled: C-3PO
== APP - job-scheduler == Job details: {"name":"C-3PO", "dueTime":"20s", "data":{"value":{"value":"C-3PO:Limb Calibration"}}}
== APP - job-service == Received job request...
== APP - job-service == Starting droid: R2-D2
== APP - job-service == Executing maintenance job: Oil Change
```

After 20 seconds, the terminal output should present the `C-3PO` job being processed:

```text
== APP - job-service == Received job request...
== APP - job-service == Starting droid: C-3PO
== APP - job-service == Executing maintenance job: Limb Calibration
```

<!-- END_STEP -->

3. Stop and clean up application processes.

<!-- STEP
name: Stop multi-app run
-->

```bash
dapr stop -f .
```

<!-- END_STEP -->

## Run the apps individually

### Schedule Jobs

1. Open a terminal and run the `job-service` app:

```bash
dapr run --app-id job-service --app-port 6200 --dapr-http-port 6280 -- npm run start
```

2. On a new terminal window, schedule the `R2-D2` Job using the Jobs API.

```bash
curl -X POST \
  http://localhost:6280/v1.0-alpha1/jobs/r2-d2 \
  -H "Content-Type: application/json" \
  -d '{
        "data": {
          "value": "R2-D2:Oil Change"
        },
        "dueTime": "2s"
    }'
```

Back at the `job-service` app terminal window, the output should be:

```text
== APP - job-app == Received job request...
== APP - job-app == Starting droid: R2-D2
== APP - job-app == Executing maintenance job: Oil Change
```

3. On the same terminal window, schedule the `C-3PO` Job using the Jobs API.

```bash
curl -X POST \
  http://localhost:6280/v1.0-alpha1/jobs/c-3po \
  -H "Content-Type: application/json" \
  -d '{
    "data": {
      "value": "C-3PO:Limb Calibration"
    },
    "dueTime": "30s"
  }'
```

### Get a scheduled job

1. On the same terminal window, run the command below to get the recently scheduled `C-3PO` job.

```bash
curl -X GET http://localhost:6280/v1.0-alpha1/jobs/c-3po -H "Content-Type: application/json"
```

You should see the following:

```text
{"name":"C-3PO", "dueTime":"30s", "data":{"@type":"type.googleapis.com/google.protobuf.StringValue", "expression":"C-3PO:Limb Calibration"}}
```

### Delete a scheduled job

1. On the same terminal window, run the command below to deleted the recently scheduled `C-3PO` job.

```bash
curl -X DELETE http://localhost:6280/v1.0-alpha1/jobs/c-3po -H "Content-Type: application/json"
```

2. Run the command below to attempt to retrieve the deleted job:

```bash
curl -X GET http://localhost:6280/v1.0-alpha1/jobs/c-3po -H "Content-Type: application/json"
```

Back at the `job-service` app terminal window, the output should be:

```text
ERRO[0249] Error getting job c-3po due to: rpc error: code = Unknown desc = job not found: app||default||job-service||c-3po  instance=diagrid.local scope=dapr.api type=log ver=1.14.0-rc.2
```
