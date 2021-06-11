# Worker

Workers are the heart of a product team because they actually do the work to move the project forward. Workers define the contents of the work item cards, update their status, and move the cards to the project board column that indicates the work being performed.

## Use Cases

### EPIC: Define a Card

#### STORY: Define the kinds of cards tracked on our project board

```Gherkin
Scenario: Define a new card type
  Given
    And
   When
    And
   Then
    And
    
Scenario: Edit the definition of an existing card type
  Given
    And
   When
    And
   Then
    And

Scenario: Organize the field presentation for a card type
  Given
    And
   When
    And
   Then
    And
```

#### STORY: Specify the requirements to complete the work indicated by this card

```Gherkin
Scenario: Specify data input restrictions for a field on a card type
  Given
    And
   When
    And
   Then
    And
```

#### STORY: Provide a scope estimate of the work associated with this card

```Gherkin
Scenario: Estimate a card
  Given
    And
   When
    And
   Then
    And
```

#### STORY: Link external dependencies to this card by URL such as specifications

```Gherkin
Scenario: Link a URL to a card
  Given
    And
   When
    And
   Then
    And
```

### EPIC: Perform Work Against a Card

#### STORY: Append to a journal of activity for this card's associated work

```Gherkin
Scenario: Comment on a card
  Given
    And
   When
    And
   Then
    And

Scenario: Enter a message to journal the work performed on a card
  Given
    And
   When
    And
   Then
    And

Scenario: Attach an image to a card
  Given
    And
   When
    And
   Then
    And
```

#### STORY: Refine the estimate-to-complete for this card to indicate work remaining

```Gherkin
Scenario: Refine the estimate-to-complete value
  Given
    And
   When
    And
   Then
    And
```

#### STORY: Link work products to this card by URL

```Gherkin
Scenario: Link a URL to a card
  Given
    And
   When
    And
   Then
    And
```

#### STORY: Make a comment on the card that others will see when they view the card

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

#### STORY: Indicate the the work on a card for the current column has been completed and that work is ready to be verified by the Reviewer

```Gherkin
Scenario: Move a card from "in-work" to "review" for the current column
  Given
    And
   When
    And
   Then
    And
```

