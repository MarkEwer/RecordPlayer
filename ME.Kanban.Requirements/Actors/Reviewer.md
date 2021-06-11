# Reviewer

Reviewers are team members that verify the correctness and/or completeness of work products at one or more steps in the product workflow. This can take the form of a Quality Assurance role or a work approver of some kind.

## Use Cases

### EPIC: Approve or Reject a Work Task

```Gherkin
Background: Approve or Reject a Work Task
  Given a project called "Unit Test" exists
    And "Unit Test" has only one board called "Board 1"
    And "Board 1" has the default columns called "New" and "Done"
    And "Board 1" has a third columns called "In Work"
    And a user called "Bill" is a worker in the "Unit Test" project
    And a user called "Janet" is a reviewer in the "Unit Test" project
    And "Janet" is the reviewer for the "In Work" column
    And the "In Work" column contains nine cards
    And there are three cards in "To Do" called "Card 1", "Card 2", and "Card 3"
    And there are three cards assigned to Bill called "Card 4", "Card 5", and "Card 6"
    And there are three cards in "To Review" called "Card 7", "Card 7", and "Card 9"
```

#### STORY: Quickly identify the work cards that are waiting for my review

```Gherkin
Scenario: Discover cards waiting for my approval
  Given background "Approve or Reject a Work Task"
   When "Janet" opens the "Board 1" project board
   Then "Janet" will see the following in the "In Work" column
   
        | To Do  | Doing         | To Review |
        |--------|---------------|-----------|
        | Card 1 | Card 4 (Bill) | *Card 7*  |
        | Card 2 | Card 5 (Bill) | *Card 8*  |
        | Card 3 | Card 6 (Bill) | *Card 9*  |

    And cards 7, 8 and 9 will be highlighted to indicate Janet can act on them
```

#### STORY: View the details of a card's work requirements and the work products created during the work execution to verify if the requirements have been met and the result meets my quality expectations

```Gherkin
Scenario: Approve a work task waiting for my review
  Given background "Approve or Reject a Work Task"
    And "Card 7" has a description of "Some Stuff Here"
    And "Card 7" has a journal entry that says "I did stuff today"
    And "Card 7" has a linked PNG graphic
   When Janet selects to view the card
   Then the card's description is visible #See UX Prototype 
    And the card's title is visible
    And the card indicates a linked journal entry
    And the card indicates a linked PNG file
```

#### STORY: Comment on a work card so the worker assigned to this card will receive the message

```Gherkin
@Reviewer, @Worker
Scenario: Comment on a card
  Given background "Approve or Reject a Work Task"
   When Janet opens "Card 4"
    And Janet chooses the "Add Comment" option
    And Janet enters "This is a new comment"
   Then a new comment is appended to "Card 4"
    And Bill gets a notification that a new comment was added
```

#### STORY: Reject a work card so that it returns to the awaiting queue in the current project board column

```Gherkin
Scenario: Reject a work task waiting for my review
  Given background "Approve or Reject a Work Task"
   When Janet opens "Card 8"
    And Janet chooses the "Reject" option
    And Janet supplies a reason of "This is no good"
   Then a new comment is appended the card from Janet with text "This is no good"
    And "Card 8" is moved back to the "To Do" sub-column
    And Bill gets a notification that a new comment was added
```

#### STORY: Approve a work card and specify which column it should move to next based on the workflow rules for the project board

```Gherkin
Scenario: Move a card from "review" to a new column per the workflow
  Given background "Approve or Reject a Work Task"    
    And the "In Work" column has a workflow option that points to "New"
    And the "In Work" column has a workflow option that points to "Done"
   When Janet opens "Card 7"
    And Janet chooses the "Approve" option
    And Janet supplies a reason of "this looks good"
    And Janet chooses "Done" from the two workflow options of "New" and "Done"
   Then a new comment is appended the card from Janet with text "this looks good"
    And "Card 7" is moved to the "Done" column
    And Bill gets a notification that a new comment was added
```
