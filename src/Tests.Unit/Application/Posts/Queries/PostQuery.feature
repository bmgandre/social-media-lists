﻿@UnitTests
@Application
@Posts
@Queries

Feature: PostQuery

        Tests the ability for searching posts

Scenario: Search posts with no filter
        Given I have a request for searching posts
        And no data is provided for filtering the posts
        When I search the posts
        Then the post repository should be reached
        And the post filter validator should be reached

Scenario: Search posts with lists in the filter
        Given I have a request for searching posts
        And a set of social lists are provided for filtering the posts
        When I search the posts
        Then the post repository should be reached
        And the post filter validator should be reached
        And the social lists repository should be reached