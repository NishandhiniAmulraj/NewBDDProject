@Login
Feature: User Login

  Scenario Outline: Valid login
    Given I navigate to the login page
    When I login with "<username>" and "<password>"
    Then I should see the products page

  Examples:
    | username                | password      |
    | standard_user           | secret_sauce  |
    | performance_glitch_user | secret_sauce  |
