# Reviewer

Reviewers are team members that verify the correctness and/or completeness of work products at one or more steps in the product workflow. This can take the form of a Quality Assurance role or a work approver of some kind.

## Use Cases

### EPIC: Approve or Reject a Work Task

#### STORY: Quickly identify the work cards that are waiting for my review

```Gherkin
Scenario: Discover cards waiting for my approval
  Given a project called "Unit Test" exists
    And "Unit Test" has only one board called "Board 1"
    And "Board 1" has the default columns called "New" and "Done"
    And "Board 1" has a third columns called "In Work"
    And a user called "Janet" is a reviewer in the "Unit Test" project
    And "Janet" is the reviewer for the "In Work" column
    And the "In Work" column contains nine cards
    And there are three cards that have not been assigned a worker
    And there are three incomplete cards
    And there are three complete cards that have not been assigned a reviewer        
   When "Janet" opens the "Board 1" project board
   Then "Janet" will see the following in the "In Work" column
   
        | To Do  | Doing  | To Review |
        |--------|--------|-----------|
        | Card 1 | Card 4 | *Card 7*  |
        | Card 2 | Card 5 | *Card 8*  |
        | Card 3 | Card 6 | *Card 9*  |

    And cards 7, 8 and 9 will be highlighted to indicate Janet can act on them
```

#### STORY: View the details of a card's work requirements and the work products created during the work execution to verify if the requirements have been met and the result meets my quality expectations

```Gherkin
Scenario: Approve a work task waiting for my review
  Given
    And
   When
    And
   Then
    And
```

#### STORY: Comment on a work card so the worker assigned to this card will receive the message

```Gherkin
@Reviewer, @Worker
Scenario: Comment on a card
  Given
    And
   When
    And
   Then
    And
```

#### STORY: Reject a work card so that it returns to the awaiting queue in the current project board column

```Gherkin
Scenario: Reject a work task waiting for my review
  Given
    And
   When
    And
   Then
    And
```

#### STORY: Approve a work card and specify which column it should move to next based on the workflow rules for the project board

```Gherkin
Scenario: Move a card from "review" to a new column per the workflow
  Given
    And
   When
    And
   Then
    And
```
