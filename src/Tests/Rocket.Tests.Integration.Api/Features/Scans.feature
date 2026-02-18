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

    Scenario: Send new capture
        Given the extended method is called against endpoint "/api/capture/process"