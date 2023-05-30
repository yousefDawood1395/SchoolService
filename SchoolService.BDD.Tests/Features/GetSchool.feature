Feature: Get school feature

Background:
	When I create a school

Scenario: Get school success
	When I get a school with id
	Then I should Get the school as a response

Scenario: Get school failed not found
	When I get a school with invalid id
	Then I get response not found