@UnitTests
@Application
@Posts
@Validators

Feature: PostFilterValidator

	In order to avoid searches with empty
	result, I want the post query filters to be
	validated

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