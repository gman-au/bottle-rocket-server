@DEVICE:API
Feature: Scans
Description: Testing the Scans and Captures API endpoint and associated functionality.

    Background:
        Given the user John has been added as an admin
        Given the user Paula has been added as a non-admin via user John

    @UNAUTHORIZED
    Scenario: Fetch scans (unauthorized)
        Given an API request is created against endpoint "/api/scans/fetch"
        When the request is sent via "POST"
        Then the request should have failed with status code 401

    @INVALID
    Scenario: Send new capture (no attachment data)
        Given an API multipart request is created against endpoint "/api/capture/process"
        And the multipart file data is set to base64 string ""
        And the multipart request authorization is set to the user John
        When the multipart request is sent via "POST"
        Then the multipart request should have failed with status code 500
        And the multipart response with path "error_code" should have a value of "1001"

    @CREATE-RECORD
    Scenario: Send new capture (include attachment data)
        Given an API multipart request is created against endpoint "/api/capture/process"
        And the multipart file data is set to base64 string "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAACklEQVR42mMAAQAABQABoIJXOQAAAABJRU5ErkJggg=="
        And the multipart request authorization is set to the user John
        When the multipart request is sent via "POST"
        Then the multipart request should have succeeded

    @CREATE-RECORD @SEGREGATION
    Scenario: Ensure captures are segregated
        Given an API multipart request is created against endpoint "/api/capture/process"
        And the multipart file data is set to base64 string "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAACklEQVR42mMAAQAABQABoIJXOQAAAABJRU5ErkJggg=="
        And the multipart request authorization is set to the user John
        When the multipart request is sent via "POST"
        Then the multipart request should have succeeded
        # Access as the user that created the scan
        Given an API request is created against endpoint "/api/scans/fetch"
        And the request authorization is set to the user John
        And the request body element "start_index" has value "0"
        And the request body element "record_count" has value "10"
        When the request is sent via "POST"
        Then the request should have succeeded
        And the response with path "total_records" should have a value of "1"
        # Access as the user that didn't create the scan
        Given an API request is created against endpoint "/api/scans/fetch"
        And the request authorization is set to the user Paula
        And the request body element "start_index" has value "0"
        And the request body element "record_count" has value "10"
        When the request is sent via "POST"
        Then the request should have succeeded
        And the response with path "total_records" should have a value of "0"
        