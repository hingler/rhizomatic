# rhizomatic

little graph based dialogue tree i need for a thing

it's small so might as well put it here

## syntax

lines represent individual statements

### label
labels are written like markdown header/subheader

```
# {label name} : {label description}
```

in certain contexts (ie branches), label description is exposed

### text
text spoken by a particular character.

```
{speaker}: {dialogue}
```

### branch
branches represent points at which the client can specify one of a series of choices, specified by label names

```
{label1}|{label2}|{label3}
```

### static lock
static locks are designed to be hidden unless a certain series of checks pass; these checks can be provided by `(TBA: write interface to provide info on dialogue checks)`

these can be substituted into branch statements.

```
({LockName1}, {LockName2}, ... : {Label})
```

### dynamic lock
dynamic locks are shown, but require the user to provide some piece of information in order to access their contents - as an example, a visual novel might require the player to provide an item; the attributes of said item can be provided to this dynamic lock, unlocking its contents

in this case, we provide specific dedupe strings which represent requirements - an entry which meets all of their requirements will enter the pass state - otherwise, we enter the fail state.

since dynamic locks are evaluated on a pass/fail basis, both passing/failing labels must be specified.

these can be substituted into branch statements.

```
[{Dedupe1}, {Dedupe2}, ... ? {PassLabel} : {FailLabel}]
```

### jump
a jump places dialogue at the specified label. useful for ending a dialogue sequence :3

```
^{Label}
```

labels mark the start of a new dialogue sequence, and correspondingly terminate the previous dialogue sequence.

labels are also communicated between files :3

### unlock
in some cases (ex. if the player passes a dynamic lock) we may want to unlock a relevant lock within the sequence without having to communicate with external code. this statement marks a given lock as "unlocked"

```
>{LockName}<
```

hypothetically (not sure if i need it yet) a lock can be locked as well with:

```
<{LockName}>
```

## jumptable
jumptables are designed to handle dialogue sequences more gracefully - file name is a label.
when starting from a jumptable, entries are evaluated in order:

```
({StaticLock1}, {StaticLock2}, ...)  -> {Label}
```

the last passing entry is the one which is handled

(TODO: does not gracefully handle branching, which is what re-locking a node is for - should be fine for now, but might get messier than i'm anticipating, somewhere down the line)
((TODO2: does not handle failure gracefully, thrown exceptions will knock everything off the rails - resolve a little down the line i think, right now it's not necessary))