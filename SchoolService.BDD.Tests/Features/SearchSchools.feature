Feature: Search schools feature

Background:
	Given I have a list of schools in my database

Scenario: Search schools resturns restults
	When I search for schools using school name
	Then I should get results satisfying the search terms

Scenario: Search for schools returns no results
	When I search for school using invalid name
	Then I should get and empty school list result