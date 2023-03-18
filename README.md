# rhizomatic

little graph based dialogue tree i need for a thing

it's small so might as well put it here

## syntax

lines represent individual statements

### text
text spoken by a particular character.

```
{speaker}: {dialogue}
```

### label
labels are written like markdown header/subheader

```
# label name

## optional label description
```

in certain contexts (ie branches), label description is exposed

### branch
branches represent points at which the client can specify one of a series of choices, specified by label names

```
{label1}|{label2}|{label3}
```

### static lock
static locks are designed to be hidden unless a certain series of checks pass; these checks can be provided by `(TBA: write interface to provide info on dialogue checks)`

these can be substituted into branch statements.

```
{Label}({LockName1}, {LockName2}, ...)
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

### unlock
in some cases (ex. if the player passes a dynamic lock) we may want to unlock a relevant lock within the sequence without having to communicate with external code. this statement marks a given lock as "unlocked"

```
>{LockName}<
```

hypothetically (not sure if i need it yet) a lock can be locked as well with:

```
<{LockName}>
```