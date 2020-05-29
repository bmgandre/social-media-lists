@UnitTests
@Application
@Posts
@Validators

Feature: PostSearchRequestValidator

	In order to avoid searches with empty
	result, I want the post query filters to be
	validated

Scenario Outline: Post search validation should accept empty date
	Given a search post request to validate
	And the post date filter is ['<Date Begin>', '<Date End>']
	When the request to search posts with the specified filter is checked
	Then the post filter validation should give no error

	Examples: 
		| Date Begin | Date End |
		|            | Today    |
		| last week  |          |
		| last week  | Today    |
		|            |          |

Scenario Outline: Post search validation should accept empty page
	Given a search post request to validate
	And the post page filter is ['<Page From>', '<Page Size>']
	When the request to search posts with the specified filter is checked
	Then the post filter validation should give no error

	Examples: 
		| Page From | Page Size |
		| 0         |           |
		|           | 10        |
		| 1         | 10        |
		|           |           |

Scenario Outline: Post search validation should restrict network by name
	Given a search post request to validate
	And the post network filter is '<Network>'
	When the request to search posts with the specified filter is checked
	Then the post filter validation should give the error '<Error>'

	Examples: 
		| Network  | Error           |
		| Facebook |                 |
		| Twitter  |                 |
		| LinkedIn | Invalid network |
		| Google+  | Invalid network |

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

Scenario Outline: Valid post search should pass
	Given a search post request to validate
	And the post filter text is '<Text>'
	And the post network filter is '<Network>'
	And the post date filter is ['<Date Begin>', '<Date End>']
	And the post page filter is ['<Page From>', '<Page Size>']
	When the request to search posts with the specified filter is checked
	Then the post filter validation should give no error

	Examples: 
		| Text          | Network  | Date Begin | Date End | Page From | Page Size |
		| My first post | Facebook | last week  | Today    | 0         | 10        |

Scenario Outline: Invalid post search should not pass
	Given a search post request to validate
	And the post filter text is '<Text>'
	And the post network filter is '<Network>'
	And the post date filter is ['<Date Begin>', '<Date End>']
	And the post page filter is ['<Page From>', '<Page Size>']
	When the request to search posts with the specified filter is checked
	Then the post filter validation should give at least one error

	Examples: 
		| Text          | Network  | Date Begin      | Date End | Page From | Page Size |
		| My first post | Facebook | 7 days from now | Today    | -1        | 10        |