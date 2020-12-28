# MafiaGame

This is an experimental engine for the game of [Mafia](https://en.wikipedia.org/wiki/Mafia_(party_game)).
It is incomplete, but a lot of work has been done on what is probably the most complicated part of the game, which is resolution of night actions.

## Night action resolution

The resolver implemented here roughly follows [Natural Action Resolution](https://wiki.mafiascum.net/index.php?title=Natural_Action_Resolution/Normal_Game).

Each night action is modelled as a list of one or more dependencies between players.
If the outcome of a player's night action depends on the outcome of any other night actions, then a dependency is defined from that player to any other players whose night actions are depended upon.

For example, if Alice the Roleblocker blocks Bob the Cop, then there is a dependency from Bob to Alice: Bob may not carry out his night action if Alice's night action succeeds.

Complications arise because any player may generally target any other player in the game, and the varied nature of night action effects can create a web of dependencies.
If Chris the Bus Driver selects targets X and Y, then:
- Both X and Y's actions depend on Chris' action succeeding
- _Every other night action that targets X or Y_ will also depend on Chris' action succeeding

These dependencies are structured as a directed possibly-cyclic graph.
The resolution algorithm is as follows, for a given dependency graph `G`:

1. If `G` is empty, we are done
2. If there is a player `P` in `G` that does not depend on any other player (in other words, `P` has no outgoing edges):
   1. Resolve `P`'s action, remove `P` from `G` and go back to step 2
3. Otherwise, there is a cycle in `G`.
   Define a supergraph `S` of all strongly connected components of `G`.
4. By definition `S` is acyclic: there exists at least one component `C` in `S` that has no outgoing edges to any other component, otherwise those components would form a larger strongly connected component.
   Therefore a player action from `C` can be resolved before any other action because no player in `C` depends on any other player outside of `C`.
5. Because `C` contains cycles, select a player according to a [priority rule based on their ability](https://wiki.mafiascum.net/index.php?title=Natural_Action_Resolution#In_case_of_emergency_breakdown_of_the_Golden_Rule):
   1. Group players in `C` by role priority
   2. Pick the group `R` with the highest priority
   3. If `R` contains exactly one player `P`, pick `P`.
   4. Otherwise, pick `P` from `R` at random.
   5. Resolve `P`'s action, remove `P` from `G` and go back to step 2
