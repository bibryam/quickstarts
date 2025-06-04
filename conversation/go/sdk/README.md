# Dapr Conversation API (Go SDK)

In this quickstart, you'll send an input to a mock Large Language Model (LLM) using Dapr's Conversation API. This API is responsible for providing one consistent API entry point to talk to underlying LLM providers.

Visit [this](https://docs.dapr.io/developing-applications/building-blocks/conversation/conversation-overview/) link for more information about Dapr and the Conversation API.

> **Note:** This example leverages the Dapr SDK. If you are looking for the example using the HTTP API [click here](../http/).

This quickstart includes one app:

- `conversation.go`, responsible for sending an input to the underlying LLM and retrieving an output.

## Run the app with the template file

This section shows how to run the application using the [multi-app run template files](https://docs.dapr.io/developing-applications/local-development/multi-app-dapr-run/multi-app-overview/) with `dapr run -f .`.  

This example uses the default LLM Component provided by Dapr which simply echoes the input provided, for testing purposes. Here are other [supported Conversation components](https://docs.dapr.io/reference/components-reference/supported-conversation/).

Open a new terminal window and run the multi app run template:

<!-- STEP
name: Run multi app run template
expected_stdout_lines:
  - '== APP - conversation == Input sent: What is dapr?'
  - '== APP - conversation == Output response: What is dapr?'
expected_stderr_lines:
output_match_mode: substring
match_order: none
background: false
sleep: 15
timeout_seconds: 30
-->

```bash
dapr run -f .
```

The terminal console output should look similar to this, where:

- The app sends an input `What is dapr?` to the `echo` Component mock LLM.
- The mock LLM echoes `What is dapr?`.

```text
== APP - conversation == Input sent: What is dapr?
== APP - conversation == Output response: What is dapr?
```

<!-- END_STEP -->

2. Stop and clean up application processes.

<!-- STEP
name: Stop multi-app run 
sleep: 5
-->

```bash
dapr stop -f .
```

<!-- END_STEP -->

## Run the app individually

1. Navigate to the `./conversation` directory (e.g., `cd ./conversation`). If you haven't already, build the Go module:
<!-- STEP
name: Build go app sdk
working_dir: ./conversation
-->
```bash
go build .
```
<!-- END_STEP -->

2. Run the Dapr process alongside the application:
<!-- STEP
name: Run go app sdk
working_dir: ./conversation
expected_stdout_lines:
  - '== APP == Input sent: What is dapr?'
  - '== APP == Output response: What is dapr?'
expected_stderr_lines:
output_match_mode: substring
match_order: none
background: true
sleep: 15
timeout_seconds: 30
-->
```bash
dapr run --app-id conversation --resources-path ../../../components/ --app-port 8081 --dapr-http-port 3508 -- go run .
```
<!-- END_STEP -->

The terminal console output should look similar to this, where the app sends an input and the mock LLM echoes it.

<!-- STEP
name: Stop go app sdk
-->
```bash
dapr stop --app-id conversation
```
<!-- END_STEP -->