# Worker

Workers are the heart of a product team because they actually do the work to move the project forward. Workers define the contents of the work item cards, update their status, and move the cards to the project board column that indicates the work being performed.

## Initial Use Cases

### Define a Card

#### Define the kinds of cards tracked on our project board

```Gherkin
Scenario: Define a new card type
Scenario: Edit the definition of an existing card type
Scenario: Organize the field presentation for a card type
```

#### Specify the requirements to complete the work indicated by this card

```Gherkin
Scenario: Specify data input restrictions for a field on a card type
```

#### Provide a scope estimate of the work associated with this card

```Gherkin
Scenario: Estimate a card
```

#### Link external dependencies to this card such as specifications or work products

```Gherkin
Scenario: Link a URL to a card
```


### Perform Work Against a Card

#### Append to a journal of activity for this card's associated work

```Gherkin
Scenario: Comment on a card
Scenario: Enter a message to journal the work performed on a card
Scenario: Attach an image to a card
```

#### Refine the estimate-to-complete for this card to indicate work remaining

```Gherkin
Scenario: Refine the estimate-to-complete value
```

#### Link external dependencies to this card such as specifications or work products

```Gherkin
```

#### Make a comment on the card that others will see when they view the card

```Gherkin
@Reviewer, @Worker
Scenario: Comment on a card
```

#### Indicate the the work on a card for the current column has been completed and that work is ready to be verified by the Reviewer

```Gherkin
Scenario: Move a card from "in-work" to "review" for the current column
```

