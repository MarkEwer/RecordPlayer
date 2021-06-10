# Reviewer

Reviewers are team members that verify the correctness and/or completeness of work products at one or more steps in the product workflow. This can take the form of a Quality Assurance role or a work approver of some kind.

## Use Cases

### Approve or Reject a Work Task

#### Quickly identify the work cards that are waiting for my review

```Gherkin
Scenario: Discover cards waiting for my approval
  Given
    And
   When
    And
   Then
    And
```

#### View the details of a card's work requirements and the work products created during the work execution to verify if the requirements have been met and the result meets my quality expectations

```Gherkin
Scenario: Approve a work task waiting for my review
  Given
    And
   When
    And
   Then
    And
```

#### Comment on a work card so the worker assigned to this card will receive the message

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

#### Reject a work card so that it returns to the awaiting queue in the current project board column

```Gherkin
Scenario: Reject a work task waiting for my review
  Given
    And
   When
    And
   Then
    And
```

#### Approve a work card and specify which column it should move to next based on the workflow rules for the project board

```Gherkin
Scenario: Move a card from "review" to a new column per the workflow
  Given
    And
   When
    And
   Then
    And
```
