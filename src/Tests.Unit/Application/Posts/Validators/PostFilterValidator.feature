@UnitTests
@Application
@Posts
@Validators

Feature: PostFilterValidator

	In order to avoid searches with empty
	result, I want the post query filters to be
	validated

Scenario Outline: Post filter validation should accept empty date
	Given I have post filter model to be validated
	And the post date filter is ['<Date Begin>', '<Date End>']
	When I validate the post filter
	Then the post filter validation should give no error

	Examples: 
		| Date Begin | Date End |
		|            | Today    |
		| last week  |          |
		| last week  | Today    |
		|            |          |

Scenario Outline: Post filter validation should accept empty page
	Given I have post filter model to be validated
	And the post page filter is ['<Page From>', '<Page Size>']
	When I validate the post filter
	Then the post filter validation should give no error

	Examples: 
		| Page From | Page Size |
		| 0         |           |
		|           | 10        |
		| 1         | 10        |
		|           |           |

Scenario Outline: Post filter validation should restrict network by name
	Given I have post filter model to be validated
	And the post network filter is '<Network>'
	When I validate the post filter
	Then the post filter validation should give the error '<Error>'

	Examples: 
		| Network  | Error           |
		| Facebook |                 |
		| Twitter  |                 |
		| LinkedIn | Invalid network |
		| Google+  | Invalid network |

Scenario Outline: Valid post filter should pass
	Given I have post filter model to be validated
	And the post filter text is '<Text>'
	And the post network filter is '<Network>'
	And the post date filter is ['<Date Begin>', '<Date End>']
	And the post page filter is ['<Page From>', '<Page Size>']
	When I validate the post filter
	Then the post filter validation should give no error

	Examples: 
		| Text          | Network  | Date Begin | Date End | Page From | Page Size |
		| My first post | Facebook | last week  | Today    | 0         | 10        |

Scenario Outline: Invalid post filter should not pass
	Given I have post filter model to be validated
	And the post filter text is '<Text>'
	And the post network filter is '<Network>'
	And the post date filter is ['<Date Begin>', '<Date End>']
	And the post page filter is ['<Page From>', '<Page Size>']
	When I validate the post filter
	Then the post filter validation should give at least one error

	Examples: 
		| Text          | Network  | Date Begin      | Date End | Page From | Page Size |
		| My first post | Facebook | 7 days from now | Today    | -1        | 10        |