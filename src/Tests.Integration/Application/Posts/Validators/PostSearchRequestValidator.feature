@IntegrationTests
@Application
@Posts
@Validators

Feature: PostSearchRequestValidator

	In order to avoid searches with empty
	result, I want the post query filters to be
	validated

Scenario Outline: Post search validation should check if a list exists
	Given the following lists are registered
	| Name    |
	| Rock    |
	| Pop     |
	| Country |
	And a search post request to validate
	And the post list filter contains
	| Name     |
	| <List 1> |
	| <List 2> |
	| <List 3> |
	When the request to search posts with the specified filter is checked
	Then the post filter validation should give the error '<Error>'
	
	Examples:
		| List 1  | List 2 | List 3  | Error        |
		|         |        |         |              |
		| Rock    |        |         |              |
		| Rock    | Pop    |         |              |
		| Rock    | Pop    | Country |              |
		| Unknown |        |         | Invalid list |
		| Unknown | Rock   |         | Invalid list |
