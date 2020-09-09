# HiraCreatures

### What?

A UE4 like framework to have a more defined model for controllers & pawns, which are sort of non-existent in Unity.

This plugin basically promotes a cleaner object-oriented approach to players, player characters, player controllers, and all that, without relying too much on singletons.

### How?

#### 1. Setup

> Create your own custom HiraCreature and HiraController classes.

> Store your custom HiraController in a HiraControllerTemplate, and use it to possess a HiraCreature.

> Try your best to minimize a direct bonding between a specific type of HiraCreature, and a specific type of HiraController, and instead rely more on interfaces.