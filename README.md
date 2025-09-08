# Digosaur

Project for the advanced graphics and interaction course DH2413 at KTH

## GitHub Workflow Guidelines

### Branch Names

First issue number, then some description

**Example:**
79-Fix_my_broken_thing

### Commits

#### 1. Tag

**feat (for adding a new feature):**
feat: Add function LCIx #x
Details: The implementation of the LIC9xfunction (including helper function)

**fix (for fixing a bug):**
fix: Resolve null pointer exception #x
Details: Adds a null check

**doc (for documentation changes):**
doc: add documentation and comments for LICx #x
Details: The detailed comments and documentation explaining the function, its inputs, and outputs.

**refactor: (for code restructuring without changing behaviour):**
refactor: Rename variable to use lowercase #x

**test: (for adding a test)**
test: Add test for LIC14

#### 2. At the end of the message:

If done: Resolves issue #45
If part of the solution: Relates to issue #45

### Pull request:

**Title: [tag]: Description of what was added**
Example: fix: Changes the parameter types of LIC5 and LIC7

**Comment:**
[Eventual info explaining the reason for this pull request]
Closes issue #45

### Merges

When the time to merge [Press green merge button]

**Title: Merge pull request # from "branch"**
Example: Merge pull request #69 from wys/issue/60

**Comment:**
[Eventual info explaining the reason for this pull request]
Closes issue #45

### Other things to think about

**Small letters for tags:** fix, feat doc, etc.

**Do not merge your own branch:** Make somebody else merge yours
