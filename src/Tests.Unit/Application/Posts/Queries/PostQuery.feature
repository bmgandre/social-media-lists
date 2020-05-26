@UnitTests
@Application
@Repository
@Posts

Feature: PostQuery

        Tests the ability for searching posts

Scenario: Search posts with no filter
        Given I have a request for searching posts
        And no data is provided for filtering the posts
        When I search the posts
        Then the post repository should be reached